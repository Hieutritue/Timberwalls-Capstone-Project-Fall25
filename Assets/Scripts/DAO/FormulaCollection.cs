using DefaultNamespace.ColonistSystem;
using UnityEngine;

namespace DefaultNamespace.General
{
    public static class FormulaCollection
    {
        [SerializeField]private static float _globalSkillMultiplier = 0.065f;

        public static float ProgressPerFrameBasedOnSkillLevel(float baseTime, int skillLevel, float taskCompletionMultiplier)
        {
            return baseTime * (1 - skillLevel * _globalSkillMultiplier) * (1 / taskCompletionMultiplier);
        }
        
        public static float GetRateOfDecrease(float baseRate, float laborMultiplier, float roomMultiplier, float afflictionMultiplier)
        {
            return baseRate * laborMultiplier * roomMultiplier * afflictionMultiplier;
        }

        public static float GetRateOfIncrease(float baseRate, float furnitureMultiplier, float roomMultiplier)
        {
            return baseRate * furnitureMultiplier * roomMultiplier;
        }
        
        public static float GetFireRate(float baseFireRate, int skillLevel)
        {
            return baseFireRate * (1 + skillLevel * _globalSkillMultiplier);
        }
        
        public static float GetTurretRotationSpeed(float baseSpeed, int skillLevel)
        {
            return baseSpeed * (1 + skillLevel * _globalSkillMultiplier);
        }
        
        public static float GetShieldRecoveryRate(float baseRecoveryRate, int totalEngineeringSkill)
        {
            return baseRecoveryRate * (1 - Mathf.Pow(0.9f, totalEngineeringSkill / 10f)) / (1 - 0.9f);
        }
    }
}