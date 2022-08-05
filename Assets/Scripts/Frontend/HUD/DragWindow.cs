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
            Debug.Log($"{eventData.position.x}-{eventData.position.y} / {Screen.width}-{Screen.height}");
            
            if (eventData.position.y > 10 && eventData.position.y < Screen.height - 10 &&
                eventData.position.x > 10 && eventData.position.x < Screen.width - 10)
                dragRectTransform.anchoredPosition += eventData.delta / GameManager.Instance.canvas.scaleFactor;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragRectTransform.SetAsLastSibling();
        }
    }
}
