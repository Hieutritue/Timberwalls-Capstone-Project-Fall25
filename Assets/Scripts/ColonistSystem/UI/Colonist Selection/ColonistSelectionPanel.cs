using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace.ColonistSystem.UI.Colonist_Selection
{
    public class ColonistSelectionPanel : MonoSingleton<ColonistSelectionPanel>
    {
        [SerializeField] private Button _openSelectionButton;
        [SerializeField] private GameObject _displayPanel;
        [SerializeField] private List<ColonistSelectionOption> _colonistOptions;
        
        [SerializeField] private List<ColonistSO> _colonistSos;

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
        
        public void OnOpenSelectionButtonPressed()
        {
            _displayPanel.SetActive(!_displayPanel.activeSelf);
        }
    }
}