using UnityEngine;

namespace DefaultNamespace.Enemy.SO
{
    [CreateAssetMenu(fileName = "AnimSet", menuName = "ScriptableObjects/Enemy/AnimationStateSet", order = 3)]
    public class AnimationStateSetSO : ScriptableObject
    {
        public string Idle = "Idle";
        public string Walk = "Walk";
        public string Attack = "Attack";
        public string Death = "Death";
    }
}
