using DefaultNamespace.ColonistSystem;

namespace DefaultNamespace.General
{
    public static class FormulaCollection
    {
        public static float ProgressPerFrameBasedOnSkillLevel(float baseTime, int skillLevel)
        {
            return baseTime * (1 - skillLevel * 0.05f);
        }
        
        public static float GetRateOfDecrease(float baseRate, float laborMultiplier, float roomMultiplier, float afflictionMultiplier)
        {
            return baseRate * laborMultiplier * roomMultiplier * afflictionMultiplier;
        }

        public static float GetRateOfIncrease(float baseRate, float furnitureMultiplier, float roomMultiplier)
        {
            return baseRate * furnitureMultiplier * roomMultiplier;
        }
    }
}