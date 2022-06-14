using Assets.Scripts.Front.MainManagers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD
{
    public class DragWindow : UnityEngine.MonoBehaviour, IDragHandler, IPointerDownHandler
    {
        public RectTransform dragRectTransform;

        public void OnDrag(PointerEventData eventData)
        {
            dragRectTransform.anchoredPosition += eventData.delta / GameManager.Instance.canvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragRectTransform.SetAsLastSibling();
        }
    }
}
