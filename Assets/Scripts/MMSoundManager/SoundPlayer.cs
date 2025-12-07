using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private SoundSource music_source;
    [SerializeField] private SoundSource UI_source;
    [SerializeField] private SoundSource sfx_source;
    [SerializeField] private SoundSource master_source;

    private void Start()
    {
        UI_source.Play(fadeIn: true, fadeOut: true, crossfade: true, forceRestart: true);
        //music_source.Play(GlobalSoundNameHolder.ost_01, fadeIn: true, fadeOut: true, crossfade: true, forceRestart: true);
    }
}