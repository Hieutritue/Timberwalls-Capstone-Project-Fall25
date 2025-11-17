using System.Collections.Generic;
using BuildingSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    [CreateAssetMenu(fileName = "New Colonist", menuName = "ScriptableObjects/Colonist System/Colonist")]
    public class ColonistSO : SerializedScriptableObject
    {
        [Header("Identity")]
        public string NPCName;
        public Sprite Portrait;
        public string Description;
        public int Tier;

        [Header("Stats")]
        public List<StatInfo> Stats = new();

        [Header("Skills")]
        public Dictionary<SkillType,int> Skills = new();
        
        [Header("Recruitment")]
        public List<ResourceWithAmount> RecruitmentCosts = new();
        
        [Header("Model")]
        public Colonist ColonistModelPrefab;
    }
}