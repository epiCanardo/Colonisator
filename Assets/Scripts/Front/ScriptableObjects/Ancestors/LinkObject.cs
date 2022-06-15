﻿using Assets.Scripts.Front.MainManagers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Ancestors
{
    public abstract class LinkObject : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public Transform target;
        protected Transform targetInstance;
        private Color sourceColor;
        void Start()
        {
            sourceColor = GetComponent<TextMeshProUGUI>().color;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            GetComponent<TextMeshProUGUI>().color = Color.cyan;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GetComponent<TextMeshProUGUI>().color = sourceColor;
        }

        /// <summary>
        /// création de la cible et passage au premier plan
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            MenusManager.Instance.TryOpenMenu(target.GetComponent<UIBoard>().key, target.gameObject);
            //targetInstance = Instantiate(target, GameManager.Instance.canvas.transform);
            //targetInstance.SetAsLastSibling();
            //GameManager.Instance.CurrentOpenedBoard += 1;
            targetInstance.GetComponent<RectTransform>().anchoredPosition += GameManager.Instance.CurrentOpenedBoard * new Vector2(50, -50);
        }
    }
}