using UnityEngine;

namespace DefaultNamespace.NotificationSystem
{
    public class NotificationSystem : MonoSingleton<NotificationSystem>
    {
        [SerializeField] private NotificationPopup _notificationPopupPrefab;
        [SerializeField] private Transform _popupParent;
    }
}