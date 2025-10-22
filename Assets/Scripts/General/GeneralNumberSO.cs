using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.General
{
    [CreateAssetMenu(fileName = "NewGeneralNumber", menuName = "General/General Number")]
    public class GeneralNumberSO : ScriptableObject
    {
        public float DigestionPerSecond;
        public float EnergyPerSecond;
        public float ConstructionRange;
    }
}