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

        public void Setup(Colonist colonist)
        {
            Colonist = colonist;
            ColonistIcon.sprite = colonist.ColonistSo.Portrait;
            ColonistName.text = colonist.ColonistSo.NPCName;

            HourBoxes.ForEach(hb => ScheduleMenu.Instance.SetScheduleBox(hb));
        }
    }
}