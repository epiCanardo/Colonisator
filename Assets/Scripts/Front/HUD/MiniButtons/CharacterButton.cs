using System.Linq;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Npc;
using Assets.Scripts.Service;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD.MiniButtons
{
    public class CharacterButton : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler 
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
            var targetInstance = MenusManager.Instance.TryOpenMenu("npcDetailBoard", target.gameObject);
            //var targetInstance = Instantiate(target, GameManager.Instance.canvas.transform);
            //targetInstance.SetAsLastSibling();
            //GameManager.Instance.CurrentOpenedBoard += 1;
            //targetInstance.GetComponent<RectTransform>().anchoredPosition += GameManager.Instance.CurrentOpenedBoard * new Vector2(50, -50);

            if (targetInstance != null)
                targetInstance.GetComponent<NpcDetailBoard>().npc = ServiceGame.Npcs.First(); // todo à remplacer par le personnage du joueur
        }
    }
}
