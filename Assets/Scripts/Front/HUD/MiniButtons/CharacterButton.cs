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
            if (targetInstance != null)
            {
                targetInstance.GetComponent<NpcDetailBoard>().npc = GameManager.Instance.GetPlayerCharacter;
            }
        }
    }
}