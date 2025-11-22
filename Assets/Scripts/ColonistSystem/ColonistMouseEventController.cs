using System;
using DefaultNamespace.ColonistSystem.UI;
using UnityEngine;
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
            ColonistDetailPanel.Instance.ClosePanel();
            ColonistDetailPanel.Instance.OpenPanel(_colonist);
            
            CameraController.Instance.Follow(_colonist.transform);
        }

        private void OnMouseOver()
        {
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
    }
}