using Sirenix.OdinInspector;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    public abstract class AAfflictionEffect : SerializedScriptableObject
    {
        public string EffectName;
        public string Description;

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