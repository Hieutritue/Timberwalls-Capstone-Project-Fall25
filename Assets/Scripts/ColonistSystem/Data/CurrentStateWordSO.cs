using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    [CreateAssetMenu(fileName = "New Current State Word", menuName = "ScriptableObjects/Colonist System/Current State Word")]
    public class CurrentStateWordSO : ScriptableObject
    {
        public string IdleWord;
        public string RunningToWord;
        public string AtWord;
    }
}