using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

namespace DefaultNamespace.TaskSystem
{
    public class PriorityBox : MonoBehaviour
    {
        public Image ImageIcon;
        public Sprite[] PrioritySprites;
        public TaskType TaskType;
        private int _priorityLevel;
        public int PriorityLevel
        {
            get => _priorityLevel;
            set
            {
                if (value > 4) value = 0;
                _priorityLevel = value;
                UpdateIcon(value);
            }
        }

        private void UpdateIcon(int priorityLevel)
        {
            ImageIcon.sprite = PrioritySprites[priorityLevel];
        }

        private void Start()
        {
            UpdateIcon(_priorityLevel);
        }
        
        public void IncreasePriority()
        {
            PriorityLevel++;
        }
    }
}