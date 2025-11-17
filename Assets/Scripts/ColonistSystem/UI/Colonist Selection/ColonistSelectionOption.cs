using System.Collections.Generic;
using Pathfinding.Collections;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.ColonistSystem.UI.Colonist_Selection
{
    public class ColonistSelectionOption : MonoBehaviour
    {
        [SerializeField] private TMP_Text _colonistName;
        [SerializeField] private Image _colonistPortrait;
        [SerializeField] private List<TMP_Text> _colonistSkills;
        [SerializeField] private List<TMP_Text> _colonistCosts;
        [SerializeField] private Button _recruitButton;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private List<Color> _tierColors;
        private ColonistSO _colonistSo;
        
        [Button]
        public void Setup(ColonistSO colonistSo)
        {
            _colonistSo = colonistSo;
            _colonistName.text = colonistSo.NPCName;
            _colonistPortrait.sprite = colonistSo.Portrait;

            _colonistSkills.ForEach((s,i)=>s.text = $"{((SkillType)i).ToString()}: {_colonistSo.Skills[(SkillType)i]}");

            _colonistCosts.ForEach((c,i)=>
            {
                c.text = $"{_colonistSo.RecruitmentCosts[i].Resource.ResourceName}: {_colonistSo.RecruitmentCosts[i].Amount}";
            });
            
            _backgroundImage.color = _tierColors[colonistSo.Tier];
        }
        
        public void OnRecruitButtonPressed()
        {
            ColonistManager.Instance.SpawnColonist(_colonistSo, Vector3.zero);
        }
    }
}