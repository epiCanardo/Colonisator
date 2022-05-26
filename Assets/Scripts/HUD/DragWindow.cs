using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Colfront.GamePlay
{
    public class DragWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
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
