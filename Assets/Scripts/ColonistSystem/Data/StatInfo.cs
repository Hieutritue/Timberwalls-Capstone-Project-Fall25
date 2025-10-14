using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.ColonistSystem
{
    [Serializable]
    public class StatInfo
    {
        public StatType StatType;
        public float MaxValue = 100;
        public float BaseDecreaseRate;
        public float FulfillmentThreshold = 30;
    }
    
    [System.Serializable]
    public class StatRuntime
    {
        public StatInfo Info;
        public float Current;
        public float IncreaseMultiplier = 1f;
        public float DecreaseMultiplier = 1f;
        public StatRuntime(StatInfo info)
        {
            Info = info;
        }

        public void Update(float deltaTime)
        {
            float change = Info.BaseDecreaseRate * DecreaseMultiplier * deltaTime;
            Current = Mathf.Clamp(Current + change, 0, Info.MaxValue);
        }
    }
    
    public enum StatType
    {
        Health,
        Hunger,
        Energy,
        Continence,
        Mood,
        Hygiene,
    }
}