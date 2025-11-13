using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Refuse To Work Effect",
        menuName = "Colonist System/Affliction System/Affliction Effects/Refuse To Work Effect")]
    public class TaskCompletionEffectSO : AAfflictionEffect
    {
        [field: SerializeField] public string EffectName { get; }
        [field: SerializeField] public string Description { get; }
        public float Value { get; }

        public void ApplyEffect(Colonist colonist)
        {
            colonist.TaskCompletionSpeedMultiplier *= Value;
        }

        public void RemoveEffect(Colonist colonist)
        {
            colonist.TaskCompletionSpeedMultiplier /= Value;
        }
    }
}