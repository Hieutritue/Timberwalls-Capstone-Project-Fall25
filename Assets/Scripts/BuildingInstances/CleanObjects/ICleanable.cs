using UnityEngine;

namespace BuildingSystem.CleanObjects
{
    public interface ICleanable
    {
        public float TimeToClean { get; }
        public Transform CleanPoint { get; }
    }
}