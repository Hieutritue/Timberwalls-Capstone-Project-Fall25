using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Stat RoD Multiplier Effect",
        menuName = "Colonist System/Affliction System/Affliction Effects/Stat RoD Multiplier Effect")]
    public class StatRodMultiplierSO : AAfflictionEffect
    {
        public StatType StatType;
        public float Value;

        public override void ApplyEffect(Colonist colonist)
        {
            colonist.AfflictionStatRodModifiers[StatType] *= Value;
        }

        public override void RemoveEffect(Colonist colonist)
        {
            colonist.AfflictionStatRodModifiers[StatType] /= Value;
        }
    }
}