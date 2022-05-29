using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public class ContinueLink : LinkObject
    {
        public GameObject optionsmenu;

        public override void OnPointerDown(PointerEventData eventData)
        {
            MenusManager.Instance.TryDestroyMenu(MenusManager.MenuType.Options);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                MenusManager.Instance.TryDestroyMenu(MenusManager.MenuType.Options);
        }
    }
}