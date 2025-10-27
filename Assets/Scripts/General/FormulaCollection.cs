namespace DefaultNamespace.General
{
    public static class FormulaCollection
    {
        public static float ProgressPerFrameBasedOnSkillLevel(float baseTime, int skillLevel)
        {
            return baseTime * (1 - skillLevel * 0.05f);
        }
    }
}