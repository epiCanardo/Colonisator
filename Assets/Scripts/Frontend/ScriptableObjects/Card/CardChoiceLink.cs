using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Front.ScriptableObjects.Ancestors
{
    public class CardChoiceLink : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private Color sourceColor;

        public CardChoice choice;
        public delegate void CarchoiceDelegate(CardChoice choice);
        public CarchoiceDelegate callbacKFunc;

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
        /// sélection du choix de la carte : application des conséquences
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            callbacKFunc(choice);
        }
    }
}