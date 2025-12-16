using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectPlacer : MonoBehaviour
    {
        public GameObject PlaceObject(GameObject prefab, Vector3 spawnPosition)
        {
            var newObject = Instantiate(prefab,
                spawnPosition,
                Quaternion.identity);
            return newObject;
        }
    }
}