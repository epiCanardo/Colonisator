using System.Collections.Generic;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Faction;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using UnityEngine;

namespace Assets.Scripts.Front.Gamepedia
{
    public class GamepediaFactionsManager : UnityEngine.MonoBehaviour
    {
        public GameObject panelPrefab;
        public GameObject cardPrefab;
        public Transform canvas;

        private GameObject panel;

        public static GamepediaFactionsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public void ShowFactions()
        {
            //panel = Instantiate(panelPrefab, canvas);
            //panel.GetComponent<FactionsBoard>().ShowBoard(x => x.longName);
            //MenusManager.Instance.TryOpenMenu(MenusManager.MenuType.FactionsBoard, panel);

            var cardMenu = MenusManager.Instance.TryOpenMenu("card", cardPrefab);
            cardMenu.GetComponent<CardBoard>().SetCard(new Card
            {
                id = "test",
                description = "test de <b>description</b>",
                title = "test de titre !",
                choices = new List<CardChoice>
                {
                    new CardChoice { label = "choix 1", shipBoardDelta = new ShipBoard()},
                    new CardChoice { label = "choix 2", shipBoardDelta = new ShipBoard()}
                }
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) ShowFactions();
        }
    }
}
