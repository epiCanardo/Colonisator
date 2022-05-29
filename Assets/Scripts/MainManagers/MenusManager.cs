using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Colfront.GamePlay
{

    public class MenusManager : MonoBehaviour
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