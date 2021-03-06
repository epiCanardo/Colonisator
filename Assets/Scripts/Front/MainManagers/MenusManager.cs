using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{

    public class MenusManager : UnityEngine.MonoBehaviour
    {
        private Dictionary<string, GameObject> menus;
        private int currentOpenedBoards;

        public static MenusManager Instance { get; private set; }

        public MenusManager()
        {
            menus = new Dictionary<string, GameObject>();
        }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public GameObject TryOpenMenu(string menuKey, GameObject objectToOpen)
        {
            if (!menus.ContainsKey(menuKey))
            {
                var menu = Instantiate(objectToOpen, GameManager.Instance.canvas.transform);
                menu.transform.SetAsLastSibling();
                menu.GetComponent<RectTransform>().anchoredPosition += currentOpenedBoards * new Vector2(50, -50);
                currentOpenedBoards++;
                menus.Add(menuKey, menu);
                return menu;
            }

            return null;
        }

        public void TryDestroyMenu(string menuKey)
        {
            if (menus.ContainsKey(menuKey))
            {
                Destroy(menus[menuKey]);
                menus.Remove(menuKey);
                currentOpenedBoards--;
            }
        }

        public void HideMenu(string menuKey)
        {
            menus[menuKey].transform.GetComponent<Image>().DOFade(0,0);
        }
    }
}