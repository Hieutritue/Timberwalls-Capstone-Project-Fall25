using UnityEngine;

namespace DefaultNamespace.ColonistSystem.AfflictionSystem
{
    [CreateAssetMenu(fileName = "Task Completion Speed Effect",
        menuName = "Colonist System/Affliction System/Affliction Effects/Task Completion Speed Effect")]
    public class TaskCompletionEffectSO : AAfflictionEffect
    {
        public float Value;

        public override void ApplyEffect(Colonist colonist)
        {
            colonist.TaskCompletionSpeedMultiplier *= Value;
        }

        public override void RemoveEffect(Colonist colonist)
        {
            colonist.TaskCompletionSpeedMultiplier /= Value;
        }
    }
}