using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.NotificationSystem
{
    public class NotificationSystem : MonoSingleton<NotificationSystem>
    {
        [SerializeField] private NotificationPopup _notificationPopupPrefab;
        [SerializeField] private Transform _popupParent;

        private Dictionary<(Colonist, NotificationType), NotificationPopup> _notificationPopups =
            new Dictionary<(Colonist, NotificationType), NotificationPopup>();

        public void AddNotification(Colonist colonist, NotificationType notificationType, string message)
        {
            NotificationPopup popup = Instantiate(_notificationPopupPrefab, _popupParent);
            popup.SetText(message);
            popup.Colonist = colonist;
            _notificationPopups[(colonist, notificationType)] = popup;
        }
        
        public void RemoveNotification(Colonist colonist, NotificationType notificationType)
        {
            if (_notificationPopups.TryGetValue((colonist, notificationType), out NotificationPopup popup))
            {
                Destroy(popup.gameObject);
                _notificationPopups.Remove((colonist, notificationType));
            }
        }
    }

    public enum NotificationType
    {
        Unreachable,
        Affliction
    }
}