using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ScheduleSystem
{
    public class ScheduleHourBox : MonoBehaviour
    {
        public Image IconImage;
        private ScheduleInfo _scheduleInfo;
        [SerializeField]
        private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            ScheduleMenu.Instance.SetScheduleBox(this);
        }

        public ScheduleInfo ScheduleInfo
        {
            get => _scheduleInfo;
            set
            {
                _scheduleInfo = value;
                if (_scheduleInfo != null)
                {
                    IconImage.sprite = _scheduleInfo.Icon;
                }
            }
        }
    }
}