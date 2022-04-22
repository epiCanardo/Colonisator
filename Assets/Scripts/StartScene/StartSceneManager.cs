using System.Collections.Generic;
using UnityEngine;

namespace Colfront.GamePlay
{

    public class StartSceneManager : MonoBehaviour
    {
        public static StartSceneManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public MenuState CurrentState { get; set; }
        public MenuState PrevisousState { get; set; }
        public GameObject MainMenu;
        public GameObject NewGameMenu;

        private Dictionary<MenuState, GameObject> dico;

        private void Start()
        {
            dico = new Dictionary<MenuState, GameObject>();
            dico.Add(MenuState.MainMenu, MainMenu);
            dico.Add(MenuState.NewGameMenu, NewGameMenu);

            CurrentState = MenuState.MainMenu;
            PrevisousState = MenuState.MainMenu;
            ShowMenu();
        }

        public void ShowMenu()
        {
            dico[PrevisousState].SetActive(false);
            dico[CurrentState].SetActive(true);
        }
    }

    public enum MenuState
    {
        MainMenu,
        NewGameMenu
    }

}