using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.TaskSystem
{
    public class PriorityRow : MonoBehaviour
    {
        public Colonist Colonist;
        public Image ColonistIcon;
        public TMP_Text ColonistName;
        public List<PriorityBox> PriorityBoxes;
        
        public void Setup(Colonist colonist)
        {
            Colonist = colonist;
            ColonistIcon.sprite = colonist.ColonistSo.Portrait;
            ColonistName.text = colonist.ColonistSo.NPCName;
        }
        
        public int GetPriorityForTaskType(TaskType taskType)
        {
            var box = PriorityBoxes.Find(pb => pb.TaskType == taskType);
            return box != null ? box.PriorityLevel : 0;
        }
    }
}