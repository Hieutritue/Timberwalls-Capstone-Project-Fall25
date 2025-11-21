using DefaultNamespace.ColonistSystem.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace.NotificationSystem
{
    public class NotificationPopup : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _messageText;
        [SerializeField] private Button _button;
        private Colonist _colonist;

        public Colonist Colonist
        {
            get => _colonist;
            set
            {
                _colonist = value;
                _button.onClick.AddListener(() =>
                {
                    _colonist.MouseEventController.OnMouseDown();
                });
            }
        }

        public void SetText(string message)
        {
            _messageText.text = message;
        }
    }
}