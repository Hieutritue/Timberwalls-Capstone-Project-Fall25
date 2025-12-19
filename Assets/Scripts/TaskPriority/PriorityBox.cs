using System;
using TMPro;
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
        public TMP_Text ColonistSkillLevelText;
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

        public void SetSkillLevelText(int skill)
        {
            ColonistSkillLevelText.text = $"{skill}";
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
            FeedbackManager.Instance.ButtonClickSmallFeedback.PlayFeedbacks();
            PriorityLevel++;
        }
    }
}