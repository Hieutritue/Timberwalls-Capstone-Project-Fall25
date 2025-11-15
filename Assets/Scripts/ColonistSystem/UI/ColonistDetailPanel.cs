using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace DefaultNamespace.ColonistSystem.UI
{
    public class ColonistDetailPanel : MonoSingleton<ColonistDetailPanel>
    {
        private Colonist _colonist;
        [SerializeField] private Image _colonistPortrait;
        [SerializeField] private TMP_Text _colonistNameText;
        [SerializeField] private List<SkillInfoPair> _skillPairs;
        [SerializeField] private TMP_Text _afflictionsText;
        [SerializeField] private TMP_Text _currentStateText;
        [SerializeField] private List<StatInfoPair> _statPairs;

        public Colonist Colonist => _colonist;

        private void RenderColonist(Colonist colonist)
        {
            LayerUtils.SetLayerRecursively(colonist.MouseEventController.gameObject,
                LayerMask.NameToLayer("Selected Colonist"));

            _colonist = colonist;
            _colonistPortrait.sprite = colonist.ColonistSo.Portrait;
            _colonistNameText.text = colonist.ColonistSo.NPCName;
            
            // Skills
            _skillPairs.ForEach((sp, index) =>
            {
                sp.SkillNameText.text = ((SkillType)index).ToString();
                sp.SkillLevelText.text = colonist.ColonistSo.Skills.ContainsKey((SkillType)index)
                    ? colonist.ColonistSo.Skills[(SkillType)index].ToString()
                    : "0";
            });
            
            // Afflictions
            HandleAfflictionChange();
            colonist.OnActiveAfflictionsChanged += HandleAfflictionChange;
            
            // Current State
            ChangeStateText(colonist.CurrentStateWord);
            colonist.OnCurrentStateChanged += ChangeStateText;
            
            // Stats
            _statPairs.ForEach((statPair, index) =>
            {
                var statType = (StatType)index;
                statPair.StatNameText.text = statType.ToString();
                statPair.StatSlider.maxValue = colonist.ColonistSo.Stats.Find(s => s.StatType == statType).MaxValue;
                statPair.StatSlider.value = colonist.StatDict[statType];
            });
            colonist.OnStatChanged += HandleStatChange;
        }

        private void Unsubscribe()
        {
            if (_colonist == null) return;

            LayerUtils.SetLayerRecursively(_colonist.MouseEventController.gameObject,
                LayerMask.NameToLayer("Colonist"));
            _colonist.OnActiveAfflictionsChanged -= HandleAfflictionChange;
            _colonist.OnCurrentStateChanged -= ChangeStateText;
            _colonist.OnStatChanged -= HandleStatChange;
        }

        private void ChangeStateText(string state)
        {
            _currentStateText.text = state;
        }

        private void HandleStatChange(StatType statType, float newValue)
        {
            var statPair = _statPairs[(int)statType];
            if (statPair != null)
            {
                statPair.StatSlider.value = newValue;
            }
        }

        private void HandleAfflictionChange()
        {
            if (!_colonist) return;

            _afflictionsText.text = string
                .Join(", ", _colonist.ActiveAfflictions
                    .Where(a => a.Value)
                    .Select(ak => ak.Key.AfflictionName));
        }

        [Button]
        public void OpenPanel(Colonist colonist)
        {
            RenderColonist(colonist);

            gameObject.SetActive(true);
        }

        [Button]
        public void ClosePanel()
        {
            CameraController.Instance.StopFollowing();
            Unsubscribe();
            _colonist = null;

            gameObject.SetActive(false);

            _colonistNameText.text = "";
            _currentStateText.text = "";
            _afflictionsText.text = "";
            _colonistNameText.text = "";
        }

        private void Start()
        {
            gameObject.SetActive(false);
            // InputManager.Instance.OnMouseLeftClick += ClosePanel;
        }
    }

    [Serializable]
    public class SkillInfoPair
    {
        public TMP_Text SkillNameText;
        public TMP_Text SkillLevelText;
    }

    [Serializable]
    public class StatInfoPair
    {
        public TMP_Text StatNameText;
        public Slider StatSlider;
    }
}