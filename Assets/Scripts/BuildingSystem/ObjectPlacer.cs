using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectPlacer : MonoBehaviour
    {
        
        private List<GameObject> _placedObjects = new();

        public GameObject PlaceObject(GameObject prefab, Vector3 spawnPosition)
        {
            var newObject = Instantiate(prefab,
                spawnPosition,
                Quaternion.identity);
            _placedObjects.Add(newObject);
            return newObject;
        }
    }
}