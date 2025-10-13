using System;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    [Serializable]
    public class AfflictionInfo
    {
        public string Name;
        [TextArea] public string Description;
        public float ChancePerSecond;
        public float TaskSpeedMultiplier = 1f;
        public float MovementMultiplier = 1f;
        public float MoodRateMultiplier = 1f;
        public float HealthDamagePerSecond = 0f;
    }
}