using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.TaskSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace General
{
    [CreateAssetMenu(fileName = "New Personal Action", menuName = "ScriptableObjects/General/Colonist Action")]
    public class ColonistActionCollectionSo : SerializedScriptableObject
    {
        // Personal action table in sheet
        // effectValue is unit per second
        // oke
        public Dictionary<TaskType, Dictionary<StatType, float>> PersonalActionsWithEffects = new();
        public Dictionary<TaskType, Dictionary<StatType, float>> ColonyActionsWithEffects = new();
    }
}