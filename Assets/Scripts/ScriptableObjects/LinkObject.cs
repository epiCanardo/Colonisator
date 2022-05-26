using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public abstract class LinkObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public Transform target;
        protected Transform targetInstance;

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<TextMeshProUGUI>().color = Color.cyan;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<TextMeshProUGUI>().color = Color.black;
        }

        /// <summary>
        /// création de la cible et passage au premier plan
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            targetInstance = Instantiate(target, GameManager.Instance.canvas.transform);
            targetInstance.SetAsLastSibling();
            GameManager.Instance.CurrentOpenedBoard += 1;
            targetInstance.GetComponent<RectTransform>().anchoredPosition += GameManager.Instance.CurrentOpenedBoard * new Vector2(50, -50);
        }
    }
}