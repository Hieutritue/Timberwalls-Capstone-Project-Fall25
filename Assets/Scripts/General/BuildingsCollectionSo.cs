using System.Collections.Generic;
using UnityEngine;

namespace General
{
    [UnityEngine.CreateAssetMenu(fileName = "New Buildings Collection", menuName = "ScriptableObjects/General/Buildings Collection")]
    public class BuildingsCollectionSo : ScriptableObject
    {
        public List<PlaceableSO> AllBuildings;
    }
}