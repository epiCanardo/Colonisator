using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using ColanderSource;
using System.Linq;

namespace Colfront.GamePlay
{
    public class CharacterButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler 
    {
        public Transform target;

        public void OnPointerEnter(PointerEventData eventData)
        {
            //transform.GetComponent<Image>().color = Color.red;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //transform.GetComponent<Image>().color = Color.black;
        }

        /// <summary>
        /// création de la cible et passage au premier plan
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            var targetInstance = Instantiate(target, GameManager.Instance.canvas.transform);
            targetInstance.SetAsLastSibling();
            GameManager.Instance.CurrentOpenedBoard += 1;
            targetInstance.GetComponent<RectTransform>().anchoredPosition += GameManager.Instance.CurrentOpenedBoard * new Vector2(50, -50);

            targetInstance.GetComponent<NpcDetailBoard>().npc = ServiceGame.Npcs.First(); // todo à remplacer par le personnage du joueur
        }
    }
}
