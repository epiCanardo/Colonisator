using Assets.Scripts.Front.MainManagers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Front.ScriptableObjects.Ancestors
{
    public class CardChoiceLink : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private Color sourceColor;

        void Start()
        {
            sourceColor = GetComponent<Image>().color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //GetComponent<Image>().DOColor(Color.white, 1f);
            GetComponent<Image>().color = Color.yellow;

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<Image>().color = sourceColor;
        }

        /// <summary>
        /// création de la cible et passage au premier plan
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            
        }
    }
}