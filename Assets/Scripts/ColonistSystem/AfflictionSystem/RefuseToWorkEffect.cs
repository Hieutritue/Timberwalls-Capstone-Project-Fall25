using System.Linq;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Effect Affliction", menuName = "Colonist System/Affliction System/Affliction Effects/Refuse To Work Effect")]
    public class RefuseToWorkEffect : AAfflictionEffect
    {
        public string EffectName { get; }
        public string Description { get; }
        public float Value { get; }

        public override void ApplyEffect(Colonist colonist)
        {
            colonist.CanWork = false;
        }

        public override void RemoveEffect(Colonist colonist)
        {
            if (colonist.ActiveAfflictions.Any(
                    a => a.Value && a.Key.AfflictionEffects.Any(e => e is RefuseToWorkEffect)))
                return;
            colonist.CanWork = true;
        }
    }
}