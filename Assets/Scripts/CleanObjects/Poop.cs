using System;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace BuildingSystem.CleanObjects
{
    public class Poop : MonoBehaviour, ICleanable
    {
        [field:SerializeField]
        public float TimeToClean { get; set; }
        [field:SerializeField]
        public Transform CleanPoint { get; set; }

        private void Start()
        {
            var cleaningTask = new CleaningTask(null, this, TaskType.Cleaning);
        }
    }
}