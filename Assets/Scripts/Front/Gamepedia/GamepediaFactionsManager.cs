using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Faction;
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
            panel = MenusManager.Instance.TryOpenMenu("factionsBoard", panelPrefab);
            if (panel != null)
                    panel.GetComponent<FactionsBoard>().ShowBoard(x => x.longName);

            //var cardMenu = MenusManager.Instance.TryOpenMenu("card", cardPrefab);
            //cardMenu.GetComponent<CardBoard>().SetCard(new Card
            //{
            //    id = "test",
            //    description = "test de <b>description</b>",
            //    title = "test de titre !",
            //    choices = new List<CardChoice>
            //    {
            //        new CardChoice { label = "choix 1", shipBoardDelta = new ShipBoard()},
            //        new CardChoice { label = "choix 2", shipBoardDelta = new ShipBoard()}
            //    }
            //});
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) ShowFactions();
        }
    }
}
