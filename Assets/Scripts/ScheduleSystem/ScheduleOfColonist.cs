using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ScheduleSystem
{
    public class ScheduleOfColonist : MonoBehaviour
    {
        [ReadOnly] public Colonist Colonist;
        public Image ColonistIcon;
        public TMP_Text ColonistName;
        public List<ScheduleHourBox> HourBoxes;
        [SerializeField] private Slider _slider;

        public void Setup(Colonist colonist, ScheduleOfColonist defaultScheduleOfColonist)
        {
            Colonist = colonist;
            ColonistIcon.sprite = colonist.ColonistSo.Portrait;
            ColonistName.text = colonist.ColonistSo.NPCName;

            SetScheduleBoxes(defaultScheduleOfColonist);
            // HourBoxes.ForEach(hb => ScheduleMenu.Instance.SetScheduleBox(hb));
        }
        
        private void SetScheduleBoxes(ScheduleOfColonist defaultScheduleOfColonist)
        {
            if (defaultScheduleOfColonist != null)
            {
                for (int i = 0; i < HourBoxes.Count; i++)
                {
                    var defaultBox = defaultScheduleOfColonist.HourBoxes[i];
                    HourBoxes[i].ScheduleInfo = defaultBox.ScheduleInfo;
                }
            }
        }

        private void Update()
        {
            _slider.value = (GameTimeManager.Instance.CurrentHour + GameTimeManager.Instance.HourProgress) / 24f;
        }
    }
}