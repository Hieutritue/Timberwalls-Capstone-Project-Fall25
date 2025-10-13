using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistSO : ScriptableObject
    {
        [Header("Identity")]
        public string NPCName;
        public Sprite Portrait;
        public string Description;

        [Header("Stats")]
        public List<StatInfo> Stats = new();

        [Header("Skills")]
        public List<SkillInfo> Skills = new();

        [Header("Afflictions (Possible)")]
        public List<AfflictionInfo> Afflictions = new();
    }
}