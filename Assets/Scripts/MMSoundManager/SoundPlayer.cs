using UnityEngine;
using DefaultNamespace.ScheduleSystem;
using System.Collections.Generic;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundSource music_source;
    [SerializeField] private SoundSource UI_source;
    [SerializeField] private SoundSource sfx_source;
    [SerializeField] private SoundSource master_source;

    private bool wasNight = false;
    private bool isPlaying = false;

    private void Start()
    {
        // Subscribe to time change events
        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.OnHourChanged += OnHourChanged;

            // Set initial ambience based on current time
            UpdateAmbience(GameTimeManager.Instance.IsNight);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe when destroyed
        if (GameTimeManager.Instance != null)
        {
            GameTimeManager.Instance.OnHourChanged -= OnHourChanged;
        }
    }

    private void OnHourChanged(int day, int hour)
    {
        bool isNight = GameTimeManager.Instance.IsNight;

        // Only change music when transitioning between day/night
        if (isNight != wasNight)
        {
            UpdateAmbience(isNight);
            wasNight = isNight;
        }
    }

    private void UpdateAmbience(bool isNight)
    {
        if (isNight)
        {
            // Shuffle from night_ambiences
            UI_source.Play(
                clipKey: null,
                fadeIn: true,
                fadeOut: true,
                crossfade: true,
                forceRestart: false,
                shuffleList: GlobalSoundNameHolder.night_ambiences
            );

            if (!isPlaying)
            {
                isPlaying = true;
                music_source.Play(
                clipKey: null,
                fadeIn: true,
                fadeOut: true,
                crossfade: true,
                forceRestart: false,
                shuffleList: GlobalSoundNameHolder.night_osts
            );
            }
        }
        else
        {
            // Shuffle from day_ambiences
            UI_source.Play(
                clipKey: null,
                fadeIn: true,
                fadeOut: true,
                crossfade: true,
                forceRestart: false,
                shuffleList: GlobalSoundNameHolder.day_ambiences
            );

            music_source.FadeOutAndStop();
            isPlaying = false;
        }
    }

    // Optional: Manual control methods
    public void PlayUISound(string clipName)
    {
        UI_source.Play(clipName);
    }

    public void PlaySFX(string clipName)
    {
        sfx_source.Play(clipName);
    }

    // Example: Play from a custom list
    public void PlayCustomList(List<string> customList)
    {
        music_source.Play(null, fadeIn: true, fadeOut: true, shuffleList: customList);
    }
}