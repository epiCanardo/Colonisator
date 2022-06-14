using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{

    public class MenusManager : UnityEngine.MonoBehaviour
    {
        private Dictionary<MenuType, GameObject> menus;

        public static MenusManager Instance { get; private set; }

        public MenusManager()
        {
            menus = new Dictionary<MenuType, GameObject>();
        }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public void TryOpenMenu(MenuType menuType, GameObject objectToOpen)
        {
            if (!menus.ContainsKey(menuType))
            {
                var menu = Instantiate(objectToOpen, GameManager.Instance.canvas.transform);
                menu.transform.SetAsLastSibling();
                menus.Add(menuType, menu);
            }
        }

        public void TryDestroyMenu(MenuType menuType)
        {
            if (menus.ContainsKey(menuType))
            {
                Destroy(menus[menuType]);
                menus.Remove(menuType);
            }
        }

        public void HideMenu(MenuType menuType)
        {
            menus[menuType].transform.GetComponent<Image>().DOFade(0,0);
        }

        public enum MenuType
        {
            Options,
            Loading
        }
    }
}