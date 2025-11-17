using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Stat DoT Effect", menuName = "Colonist System/Affliction System/Affliction Effects/Stat DoT Effect")]
    public class StatDotEffectSO : AAfflictionEffect
    {
        public StatType StatType;
        public float Value;

        public override void TickEffect(Colonist colonist)
        {
            colonist.SetStat(StatType, colonist.StatDict[StatType] - Value);
        }
    }
}