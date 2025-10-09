using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "PlaceableCollectionSO", menuName = "ScriptableObjects/PlaceableCollectionSO", order = 1)]
    public class PlaceableCollectionSO : ScriptableObject
    {
        public List<PlaceableSO> Placeables = new ();
        public PlaceableSO GetPlaceableByName(string name)
        {
            foreach (var placeable in Placeables)
            {
                if (placeable.Name == name)
                {
                    return placeable;
                }
            }
            return null;
        }
        public PlaceableSO GetPlaceableByIndex(int index)
        {
            if (index < 0 || index >= Placeables.Count)
            {
                return null;
            }
            return Placeables[index];
        }
    }
}