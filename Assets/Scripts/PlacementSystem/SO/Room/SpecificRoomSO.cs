using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "SpecificRoomSO", menuName = "ScriptableObjects/SpecificRoomSO", order = 2)]
    public class SpecificRoomSo : ScriptableObject
    {
        public string Name;
        [Header("Transformation")]
        public int TransformPriority;
        public List<RoomEffect> RoomEffects;
        [Header("Condition")]
        public List<ItemWithCount> RequiredItems;
        public List<PlaceableSO> ForbiddenItems;
        [Header("Visual")]
        public GameObject Prefab;
    }
    
    [System.Serializable]
    public class ItemWithCount
    {
        public int Count;
        public PlaceableSO Item;
    }
}