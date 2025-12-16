using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.ScheduleSystem
{
    public class GameTimeManager : MonoSingleton<GameTimeManager>
    {
        [Header("Time Settings")] public int HoursPerDay = 24;
        public float RealSecondsPerInGameHour = 1f; // 1sec IRL = 1 in-game hour

        public int NightStartHour;
        public int NightEndHour;

        public bool IsNight => CurrentHour <= NightEndHour && CurrentHour >= NightStartHour;
        
        [Header("State (Read Only)")] [ReadOnly]
        public int CurrentDay = 1;

        [ReadOnly] public int CurrentHour = 0;
        [ReadOnly] public float HourProgress = 0f; // 0-1 inside hour

        public enum TimeSpeed
        {
            Pause,
            Normal,
            Fast,
            Ultra
        }

        public TimeSpeed timeSpeed = TimeSpeed.Normal;

        private float speedMultiplier = 1f;
        private float lastTimeScale = 1f;

        // Events
        public event Action<int, int> OnHourChanged; // (day, hour)
        public event Action<int> OnDayChanged; // (day)

        private void Update()
        {
            if (timeSpeed == TimeSpeed.Pause) return;

            float speed = GetSpeedMultiplier() * Time.deltaTime;
            HourProgress += speed / RealSecondsPerInGameHour;

            while (HourProgress >= 1f)
            {
                HourProgress -= 1f;
                AdvanceHour();
            }
        }

        float GetSpeedMultiplier()
        {
            switch (timeSpeed)
            {
                case TimeSpeed.Normal: return 1f;
                case TimeSpeed.Fast: return 1.5f;
                case TimeSpeed.Ultra: return 2f;
            }

            return 0f;
        }

        void AdvanceHour()
        {
            CurrentHour++;

            if (CurrentHour >= HoursPerDay)
            {
                CurrentHour = 0;
                CurrentDay++;
                OnDayChanged?.Invoke(CurrentDay);
            }

            OnHourChanged?.Invoke(CurrentDay, CurrentHour);
        }

        // Public API

        public void SetSpeed(TimeSpeed newSpeed)
        {
            timeSpeed = newSpeed;
        }

        public void TogglePause()
        {
            timeSpeed = (timeSpeed == TimeSpeed.Pause) ? TimeSpeed.Normal : TimeSpeed.Pause;
        }

        public float GetCurrentTimeOfDayPercent()
        {
            return (CurrentHour + HourProgress) / HoursPerDay;
        }

        public void SetTimeScale(float timeScale)
        {
            lastTimeScale = Time.timeScale;
            UIManager.Instance.InvokeTimeButtonOnClick((int)timeScale);
        }

        public void RestoreLastTimeScale()
        {
            UIManager.Instance.InvokeTimeButtonOnClick((int)lastTimeScale);
        }

        private void Start()
        {
            SetTimeScale(0);
        }
    }
}