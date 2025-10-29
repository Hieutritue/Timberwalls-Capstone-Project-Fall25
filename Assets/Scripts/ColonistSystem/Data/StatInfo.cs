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
        public float BaseRateOfDecrease;
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