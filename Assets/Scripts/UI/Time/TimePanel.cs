using UnityEngine;

namespace DefaultNamespace.ScheduleSystem
{
    public class TimePanel : MonoBehaviour
    {
        [SerializeField] private SoundSource sfxSource; //sound implementation
        public void SetTimeScale(float timeScale)
        {
            sfxSource.Play(GlobalSoundNameHolder.UI_clicking_sound_2, fadeIn:false, fadeOut:false, crossfade:false); //sound
            // set time scale
            Time.timeScale = timeScale;
        }   
    }
}