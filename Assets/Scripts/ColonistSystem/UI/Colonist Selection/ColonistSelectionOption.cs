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

            _colonistSkills.ForEach((s, i) =>
                s.text = $"{((SkillType)i).ToString()}: {_colonistSo.Skills[(SkillType)i]}");

            _colonistCosts.ForEach((c, i) =>
            {
                var resource = _colonistSo.RecruitmentCosts[i].Resource;
                var cost = _colonistSo.RecruitmentCosts[i].Amount;
                var availableAmount = ResourceManager.Instance.Get(resource.ResourceType);
                var color = cost < availableAmount ? "white" : "red";


                c.text =
                    $"{resource.ResourceName}: {cost} (<color=\"{color}\">{availableAmount}</color>)";
            });
            _backgroundImage.color = _tierColors[colonistSo.Tier];
        }

        public void OnRecruitButtonPressed()
        {
            if (_colonistSo == null)
            {
                Debug.LogError("ColonistSO is not set up for this option.");
                return;
            }

            foreach (var cost in _colonistSo.RecruitmentCosts)
            {
                if (ResourceManager.Instance.Get(cost.Resource.ResourceType) < cost.Amount)
                {
                    Debug.Log("Not enough resources to recruit this colonist.");
                    return;
                }
            }

            foreach (var cost in _colonistSo.RecruitmentCosts)
            {
                ResourceManager.Instance.Set(cost.Resource.ResourceType,
                    ResourceManager.Instance.Get(cost.Resource.ResourceType) - cost.Amount);
            }

            ColonistManager.Instance.SpawnColonist(_colonistSo, Vector3.zero);
            ColonistSelectionPanel.Instance.HideSpawnChoices();
        }
    }
}