using UnityEngine;

namespace DefaultNamespace.ScheduleSystem
{
    public class TimePanel : MonoBehaviour
    {
        public void SetTimeScale(float timeScale)
        {
            // set time scale
            GameTimeManager.Instance.SetTimeScale(timeScale);
        }   
    }
}