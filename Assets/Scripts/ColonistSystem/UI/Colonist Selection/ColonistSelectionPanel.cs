using System.Collections.Generic;
using System.Data;
using DefaultNamespace.General;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.ColonistSystem.UI.Colonist_Selection
{
    public class ColonistSelectionPanel : MonoSingleton<ColonistSelectionPanel>
    {
        [SerializeField] public Button _openSelectionButton;
        [SerializeField] private GameObject _displayPanel;
        [SerializeField] private GameObject _startupAnimation;
        [SerializeField] private List<ColonistSelectionOption> _colonistOptions;
        
        [SerializeField] private List<ColonistSO> _colonistSos;

        private void Start()
        {
            _openSelectionButton.gameObject.SetActive(false);
            _displayPanel.SetActive(false);
            _startupAnimation.gameObject.SetActive(false);
        }

        [Button]
        public void SetColonists()
        {
            var colonistSos = _colonistSos.GetRange(0, 3);
            for (int i = 0; i < _colonistOptions.Count; i++)
            {
                _colonistOptions[i].Setup(colonistSos[i]);
            }
        }
        
        public void SetColonists(ColonistSO colonistSo1, ColonistSO colonistSo2, ColonistSO colonistSo3)
        {
            var colonistSos = new List<ColonistSO> { colonistSo1, colonistSo2, colonistSo3 };
            for (int i = 0; i < _colonistOptions.Count; i++)
            {
                _colonistOptions[i].Setup(colonistSos[i]);
            }
        }

        public void ShowSpawnChoices()
        {
            _openSelectionButton.gameObject.SetActive(true);
            _startupAnimation.SetActive(true);
        }

        public void HideSpawnChoices()
        {
            _openSelectionButton.gameObject.SetActive(false);
            _displayPanel.SetActive(false);
        }

        public void OnOpenSelectionButtonPressed()
        {
            _displayPanel.SetActive(!_displayPanel.activeSelf);
            _startupAnimation.SetActive(false);
        }
    }
}