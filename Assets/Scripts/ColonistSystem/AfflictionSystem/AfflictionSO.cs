using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "New Affliction", menuName = "Colonist System/Affliction System/Affliction")]
    public class AfflictionSO : ScriptableObject
    {
        public string AfflictionName;
        // public Sprite AfflictionIcon;
        public string Description;
        public StatType StatTypeCondition;
        public float MinCondition;
        public float MaxCondition;
        public List<AAfflictionEffect> AfflictionEffects;
        
        public void StartAffliction(Colonist colonist)
        {
            foreach (var effect in AfflictionEffects)
            {
                effect.ApplyEffect(colonist);
            }
        }
        public void TickAffliction(Colonist colonist)
        {
            AfflictionEffects.ForEach(effect => effect.TickEffect(colonist));
        }
        public void EndAffliction(Colonist colonist)
        {
            foreach (var effect in AfflictionEffects)
            {
                effect.RemoveEffect(colonist);
            }
        }
    }
}