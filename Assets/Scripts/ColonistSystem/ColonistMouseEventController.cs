using System;
using DefaultNamespace.ColonistSystem.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using Util;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistMouseEventController : MonoBehaviour
    {
        private Colonist _colonist;

        public void Setup(Colonist colonist)
        {
            _colonist = colonist;
        }

        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !IsPlacementIdleState()) return;
            OpenPanelDetail();
        }

        public void OpenPanelDetail()
        {
            FeedbackManager.Instance.ButtonClickSmallFeedback.PlayFeedbacks();
            ColonistDetailPanel.Instance.ClosePanel();
            ColonistDetailPanel.Instance.OpenPanel(_colonist);

            CameraController.Instance.Follow(_colonist.transform);
        }

        private void OnMouseOver()
        {
            if (EventSystem.current.IsPointerOverGameObject() || !IsPlacementIdleState()) return;
            if (ColonistDetailPanel.Instance.Colonist != _colonist)
                ChangeColonistLayer(LayerMask.NameToLayer("Hovering Colonist"));
        }

        private void OnMouseExit()
        {
            if (ColonistDetailPanel.Instance.Colonist != _colonist)
                ChangeColonistLayer(LayerMask.NameToLayer("Colonist"));
        }

        private void ChangeColonistLayer(LayerMask layerMask)
        {
            LayerUtils.SetLayerRecursively(gameObject, layerMask);
        }

        private bool IsPlacementIdleState()
        {
            return BuildingSystemManager.Instance.PlacementSystem.CurrentState is
                IdlePlacementState;
        }
    }
}