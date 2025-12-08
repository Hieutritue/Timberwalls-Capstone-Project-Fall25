using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class SoundSource : MonoBehaviour
{
    [Header("Clips in this group")]
    [SerializeField] private List<AudioClip> sounds = new List<AudioClip>();

    [Header("Playback")]
    [SerializeField] private bool fadeInOnPlay = false;
    [SerializeField] private bool fadeOutOnStop = false;
    [SerializeField] private bool looping = false;
    [SerializeField] private float fadeInDuration = 2f;
    [SerializeField] private float fadeOutDuration = 2f;

    [Header("Routing")]
    [SerializeField] private Tracks chosenTrack = Tracks.Master;
    public enum Tracks { Sfx, Music, UI, Master }

    private AudioSource _currentAudioSource;
    private MMSoundManagerPlayOptions _baseOptions;
    private List<string> stored_ambs = new List<string>();

    // Track IDs and clips
    private int _currentSoundID = 0;
    private int _previousSoundID = 0;
    private string _currentClipKey;

    // To prevent multiple plays at once
    private Coroutine _playCoroutine;

    // clipKey → AudioClip
    private readonly Dictionary<string, AudioClip> _clipMap =
        new Dictionary<string, AudioClip>(StringComparer.OrdinalIgnoreCase);

    // ---------------------------------------------------------
    // INIT
    // ---------------------------------------------------------
    private void Awake()
    {
        // Build map: key = clip.name (as seen in Inspector)
        _clipMap.Clear();
        foreach (var clip in sounds)
        {
            if (clip == null) continue;
            if (!_clipMap.ContainsKey(clip.name))
                _clipMap.Add(clip.name, clip);
        }

        // Default MM options
        _baseOptions = MMSoundManagerPlayOptions.Default;
        _baseOptions.Persistent = false;
        _baseOptions.Loop = looping;

        // Route to proper MM track
        switch (chosenTrack)
        {
            case Tracks.Master:
                _baseOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Master;
                break;
            case Tracks.Music:
                _baseOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Music;
                break;
            case Tracks.Sfx:
                _baseOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.Sfx;
                break;
            case Tracks.UI:
                _baseOptions.MmSoundManagerTrack = MMSoundManager.MMSoundManagerTracks.UI;
                break;
        }
    }

    // ---------------------------------------------------------
    // PUBLIC API
    // ---------------------------------------------------------

    // Track previously played clips for shuffle functionality
    private List<string> _playedClips = new List<string>();

    /// <summary>
    /// Play a clip from this source's group by name.
    /// Uses AudioClip.name as key (case-insensitive).
    /// If clipKey is null, shuffles from the provided shuffleList.
    /// fadeIn : if null, uses fadeInOnPlay setting; if true/false, overrides it
    /// fadeOut : if null, uses fadeOutOnStop setting; if true/false, overrides it (for crossfade)
    /// crossfade : fade out the previous clip while fading in the new one
    /// forceRestart : if true, restarts the clip even if it's already playing
    /// shuffleList : the list to shuffle from when clipKey is null
    /// </summary>
    public void Play(string clipKey = null, bool? fadeIn = null, bool? fadeOut = null, bool crossfade = true, bool forceRestart = false, List<string> shuffleList = null)
    {
        if (chosenTrack == Tracks.Sfx || chosenTrack == Tracks.UI) forceRestart = true;

        // If no clipKey provided, shuffle from the provided list
        if (clipKey == null)
        {
            if (shuffleList == null)
            {
                Debug.LogWarning($"[SoundSource] No clipKey or shuffleList provided on {name}.");
                return;
            }

            clipKey = GlobalSoundNameHolder.Shuffle(shuffleList, _playedClips);

            if (clipKey == null)
            {
                Debug.LogWarning($"[SoundSource] No clips available to shuffle on {name}.");
                return;
            }

            // Add to played clips list
            _playedClips.Add(clipKey);
        }

        if (!_clipMap.TryGetValue(clipKey, out var clip))
        {
            Debug.LogWarning($"[SoundSource] Clip '{clipKey}' not found on {name}.");
            return;
        }

        // Check if this exact clip is already playing - if so, don't restart it (unless forceRestart is true)
        if (!forceRestart && _currentClipKey == clipKey && _currentSoundID != 0 && _currentAudioSource != null && _currentAudioSource.isPlaying)
        {
            Debug.Log($"[SoundSource] Clip '{clipKey}' is already playing on {name}.");
            return;
        }

        // Stop any existing play coroutine
        if (_playCoroutine != null)
        {
            StopCoroutine(_playCoroutine);
        }

        // Determine if we should fade in/out: parameters override Inspector settings
        bool shouldFadeIn = fadeIn ?? fadeInOnPlay;
        bool shouldFadeOut = fadeOut ?? fadeOutOnStop;

        // Start the play sequence
        _playCoroutine = StartCoroutine(PlayCoroutine(clip, clipKey, shouldFadeIn, shouldFadeOut, crossfade));
    }
    private IEnumerator PlayCoroutine(AudioClip clip, string clipKey, bool fadeIn, bool fadeOut, bool crossfade)
    {
        // Handle crossfade or immediate stop
        if (_currentSoundID != 0)
        {
            if (crossfade && fadeOut && _currentAudioSource != null && _currentAudioSource.isPlaying)
            {
                // Fade out the old sound manually
                _previousSoundID = _currentSoundID;
                AudioSource previousAudioSource = _currentAudioSource;

                // Manual fade out using coroutine
                yield return StartCoroutine(FadeOutCoroutine(previousAudioSource, fadeOutDuration));

                // Stop and free the AudioSource
                if (previousAudioSource != null)
                {
                    MMSoundManagerSoundControlEvent.Trigger(
                        MMSoundManagerSoundControlEventTypes.Stop,
                        _previousSoundID,
                        previousAudioSource
                    );

                    MMSoundManagerSoundControlEvent.Trigger(
                        MMSoundManagerSoundControlEventTypes.Free,
                        _previousSoundID,
                        previousAudioSource
                    );

                    // Release the ID back to the pool
                    SoundIDRegistry.Instance.ReleaseID(_previousSoundID);
                }
            }
            else
            {
                // No crossfade or no fade-out - stop and free immediately
                if (_currentAudioSource != null)
                {
                    MMSoundManagerSoundControlEvent.Trigger(
                        MMSoundManagerSoundControlEventTypes.Stop,
                        _currentSoundID,
                        _currentAudioSource
                    );

                    MMSoundManagerSoundControlEvent.Trigger(
                        MMSoundManagerSoundControlEventTypes.Free,
                        _currentSoundID,
                        _currentAudioSource
                    );

                    // Release the ID back to the pool
                    SoundIDRegistry.Instance.ReleaseID(_currentSoundID);
                }
            }
        }

        // Always get a new unique ID for the new sound
        _currentSoundID = SoundIDRegistry.Instance.GetUniqueID();

        // Create fresh play options for this sound
        var playOptions = MMSoundManagerPlayOptions.Default;
        playOptions.ID = _currentSoundID;
        playOptions.MmSoundManagerTrack = _baseOptions.MmSoundManagerTrack;
        playOptions.Persistent = false;
        playOptions.Loop = looping;
        playOptions.Volume = _baseOptions.Volume;

        // Determine if we should fade in
        bool shouldFadeIn = fadeIn;
        if (shouldFadeIn)
        {
            playOptions.Volume = 0f;
        }

        // Trigger playback through MMSoundManager
        _currentAudioSource = MMSoundManagerSoundPlayEvent.Trigger(clip, playOptions);
        _currentClipKey = clipKey;

        if (_currentAudioSource == null)
        {
            Debug.LogError($"[SoundSource] Failed to play clip '{clipKey}' from {name}.");
            _currentSoundID = 0;
            _currentClipKey = null;
            _playCoroutine = null;
            yield break;
        }

        // Optional fade-in using manual coroutine
        if (shouldFadeIn)
        {
            StartCoroutine(FadeInCoroutine(_currentAudioSource, fadeInDuration, _baseOptions.Volume));
        }

        _playCoroutine = null;
    }

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        if (source == null) yield break;

        float startVolume = source.volume;
        float elapsed = 0f;

        while (elapsed < duration && source != null)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // EaseInCubic curve
            t = t * t * t;
            source.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        if (source != null)
        {
            source.volume = 0f;
        }
    }

    private IEnumerator FadeInCoroutine(AudioSource source, float duration, float targetVolume)
    {
        if (source == null) yield break;

        float startVolume = 0f;
        float elapsed = 0f;

        while (elapsed < duration && source != null)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // EaseInCubic curve
            t = t * t * t;
            source.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        if (source != null)
        {
            source.volume = targetVolume;
        }
    }

    /// <summary>
    /// Fade out the currently playing sound for this SoundSource.
    /// </summary>
    public void FadeOutAndStop()
    {
        if (_currentSoundID == 0) return;

        StartCoroutine(FadeOutAndStopCoroutine());
    }

    private IEnumerator FadeOutAndStopCoroutine()
    {
        int soundIDToFade = _currentSoundID;
        AudioSource audioSourceToFree = _currentAudioSource;

        // Manual fade out
        if (audioSourceToFree != null)
        {
            yield return StartCoroutine(FadeOutCoroutine(audioSourceToFree, fadeOutDuration));
        }

        // Clear current references immediately
        _currentSoundID = 0;
        _currentAudioSource = null;
        _currentClipKey = null;

        // Stop and free the AudioSource
        if (audioSourceToFree != null)
        {
            MMSoundManagerSoundControlEvent.Trigger(
                MMSoundManagerSoundControlEventTypes.Stop,
                soundIDToFade,
                audioSourceToFree
            );

            MMSoundManagerSoundControlEvent.Trigger(
                MMSoundManagerSoundControlEventTypes.Free,
                soundIDToFade,
                audioSourceToFree
            );

            // Release the ID back to the pool
            SoundIDRegistry.Instance.ReleaseID(soundIDToFade);
        }
    }

    // ---------------------------------------------------------
    // UNITY EVENT COMPATIBLE METHODS (for Inspector/OnClick)
    // ---------------------------------------------------------

    /// <summary>
    /// Play a clip by name - uses default settings
    /// </summary>
    public void PlayClip(string clipKey)
    {
        Play(clipKey);
    }

    /// <summary>
    /// Play a clip with force restart enabled
    /// </summary>
    public void PlayClipForceRestart(string clipKey)
    {
        Play(clipKey, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with fade in enabled
    /// </summary>
    public void PlayClipFadeIn(string clipKey)
    {
        Play(clipKey, fadeIn: true);
    }

    /// <summary>
    /// Play a clip with fade in disabled
    /// </summary>
    public void PlayClipNoFadeIn(string clipKey)
    {
        Play(clipKey, fadeIn: false);
    }

    /// <summary>
    /// Play a clip with fade out enabled
    /// </summary>
    public void PlayClipFadeOut(string clipKey)
    {
        Play(clipKey, fadeOut: true);
    }

    /// <summary>
    /// Play a clip with fade out disabled
    /// </summary>
    public void PlayClipNoFadeOut(string clipKey)
    {
        Play(clipKey, fadeOut: false);
    }

    /// <summary>
    /// Play a clip with crossfade disabled
    /// </summary>
    public void PlayClipNoCrossfade(string clipKey)
    {
        Play(clipKey, crossfade: false);
    }

    /// <summary>
    /// Play a clip with fade in and force restart
    /// </summary>
    public void PlayClipFadeInForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: true, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with no fade in and force restart
    /// </summary>
    public void PlayClipNoFadeInForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: false, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with fade out and force restart
    /// </summary>
    public void PlayClipFadeOutForceRestart(string clipKey)
    {
        Play(clipKey, fadeOut: true, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with no fade out and force restart
    /// </summary>
    public void PlayClipNoFadeOutForceRestart(string clipKey)
    {
        Play(clipKey, fadeOut: false, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with no crossfade and force restart
    /// </summary>
    public void PlayClipNoCrossfadeForceRestart(string clipKey)
    {
        Play(clipKey, crossfade: false, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with fade in and fade out
    /// </summary>
    public void PlayClipFadeInFadeOut(string clipKey)
    {
        Play(clipKey, fadeIn: true, fadeOut: true);
    }

    /// <summary>
    /// Play a clip with fade in, fade out, and force restart
    /// </summary>
    public void PlayClipFadeInFadeOutForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: true, fadeOut: true, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with no fade in, no fade out, and force restart
    /// </summary>
    public void PlayClipNoFadesForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: false, fadeOut: false, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with fade in and no crossfade
    /// </summary>
    public void PlayClipFadeInNoCrossfade(string clipKey)
    {
        Play(clipKey, fadeIn: true, crossfade: false);
    }

    /// <summary>
    /// Play a clip with fade out and no crossfade
    /// </summary>
    public void PlayClipFadeOutNoCrossfade(string clipKey)
    {
        Play(clipKey, fadeOut: true, crossfade: false);
    }

    /// <summary>
    /// Play a clip with fade in, fade out, and no crossfade
    /// </summary>
    public void PlayClipFadeInFadeOutNoCrossfade(string clipKey)
    {
        Play(clipKey, fadeIn: true, fadeOut: true, crossfade: false);
    }

    /// <summary>
    /// Play a clip with all fades and options enabled
    /// </summary>
    public void PlayClipFadeInFadeOutCrossfadeForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: true, fadeOut: true, crossfade: true, forceRestart: true);
    }

    /// <summary>
    /// Play a clip with no fades, no crossfade, but force restart
    /// </summary>
    public void PlayClipImmediateForceRestart(string clipKey)
    {
        Play(clipKey, fadeIn: false, fadeOut: false, crossfade: false, forceRestart: true);
    }

    // ---------------------------------------------------------
    // BASIC CONTROLS
    // ---------------------------------------------------------

    public void StopImmediate()
    {
        if (_currentSoundID == 0) return;

        if (_currentAudioSource != null)
        {
            MMSoundManagerSoundControlEvent.Trigger(
                MMSoundManagerSoundControlEventTypes.Stop,
                _currentSoundID,
                _currentAudioSource
            );

            MMSoundManagerSoundControlEvent.Trigger(
                MMSoundManagerSoundControlEventTypes.Free,
                _currentSoundID,
                _currentAudioSource
            );

            // Release the ID back to the pool
            SoundIDRegistry.Instance.ReleaseID(_currentSoundID);
        }
    }

    public void Pause()
    {
        if (_currentSoundID == 0) return;

        MMSoundManagerSoundControlEvent.Trigger(
            MMSoundManagerSoundControlEventTypes.Pause,
            _currentSoundID
        );
    }

    public void Resume()
    {
        if (_currentSoundID == 0) return;

        MMSoundManagerSoundControlEvent.Trigger(
            MMSoundManagerSoundControlEventTypes.Resume,
            _currentSoundID
        );
    }

    /// <summary>
    /// Optional: exposes what keys are valid for this SoundSource.
    /// </summary>
    public IReadOnlyCollection<string> GetClipKeys() => _clipMap.Keys;

    /// <summary>
    /// Check if a specific clip is currently playing
    /// </summary>
    public bool IsPlaying(string clipKey)
    {
        return _currentClipKey == clipKey && _currentAudioSource != null && _currentAudioSource.isPlaying;
    }

    /// <summary>
    /// Check if any clip is currently playing
    /// </summary>
    public bool IsPlaying()
    {
        return _currentSoundID != 0 && _currentAudioSource != null && _currentAudioSource.isPlaying;
    }
}