using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Stat DoT Effect", menuName = "Colonist System/Affliction System/Affliction Effects/Stat DoT Effect")]
    public class StatDotEffectSO : AAfflictionEffect
    {
        public string EffectName { get; }
        public string Description { get; }
        public StatType StatType { get; }
        public float Value { get; }

        public void TickEffect(Colonist colonist)
        {
            colonist.SetStat(StatType, colonist.StatDict[StatType] - Value);
        }
    }
}