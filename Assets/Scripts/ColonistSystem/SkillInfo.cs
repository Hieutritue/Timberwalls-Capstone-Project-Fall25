using System;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    [Serializable]
    public class SkillInfo
    {
        public SkillType SkillType;
        [Range(1, 10)] public int Level = 1;
    }
    
    public enum SkillType
    {
        Metallurgy,
        Farming,
        Engineering,
        Housekeeping,
        Scholarship,
        Marksmanship
    }
}