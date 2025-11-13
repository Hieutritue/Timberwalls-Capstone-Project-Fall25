using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Stat RoD Multiplier Effect",
        menuName = "Colonist System/Affliction System/Affliction Effects/Stat RoD Multiplier Effect")]
    public class StatRodMultiplierSO : AAfflictionEffect
    {
        public string EffectName { get; }
        public string Description { get; }
        public StatType StatType { get; }
        public float Value { get; }

        public void ApplyEffect(Colonist colonist)
        {
            colonist.AfflictionStatRodModifiers[StatType] *= Value;
        }

        public void RemoveEffect(Colonist colonist)
        {
            colonist.AfflictionStatRodModifiers[StatType] /= Value;
        }
    }
}