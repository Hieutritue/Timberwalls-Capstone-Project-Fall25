using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    public abstract class AAfflictionEffect : ScriptableObject
    {
        public string EffectName { get; }
        public string Description { get; }
        public float Value { get; }

        public virtual void ApplyEffect(Colonist colonist)
        {
        }

        public virtual void TickEffect(Colonist colonist)
        {
        }

        public virtual void RemoveEffect(Colonist colonist)
        {
        }
    }
}