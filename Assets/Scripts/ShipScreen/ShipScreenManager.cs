using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public class ShipScreenManager : MonoBehaviour
    {
        private ScreenState screenState;
        private GameManager gameManager;

        public Ship ship;

        [Header("Gestion des onglets")]
        public RectTransform mainPage;
        public RectTransform sailorsPage;

        [Header("Onglet Principal")]
        public TextMeshProUGUI Title;
        public RectTransform Dodris;
        public RectTransform Food;
        public RectTransform Order;
        public RectTransform Rigging;
        public RectTransform Hull;
        public RectTransform OilBarrels;

        // Start is called before the first frame update
        void Start()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            
        }

        /// <summary>
        ///  ouverture du panneau
        /// </summary>
        public void Show()
        {
            screenState = ScreenState.MainPage;            
            RefreshValues();
        }

        /// <summary>
        /// fermeture du panneau
        /// </summary>
        public void Close()
        {
            screenState = ScreenState.Closed;
            gameManager.ToggleShipScreen(false);
        }

        /// <summary>
        /// Activation de la page suivante selon la page actuelle
        /// </summary>
        public void NextPage()
        {
            switch (screenState)
            {
                case ScreenState.MainPage:
                    mainPage.gameObject.SetActive(false);
                    sailorsPage.gameObject.SetActive(true);
                    screenState = ScreenState.SailorsPage;
                    RefreshValues();
                    break;
   
                default:
                    break;
            }
        }

        /// <summary>
        /// Activation de la page précédente selon la page actuelle
        /// </summary>
        public void PrevisousPage()
        {
            switch (screenState)
            {
                case ScreenState.SailorsPage:
                    sailorsPage.gameObject.SetActive(false);
                    mainPage.gameObject.SetActive(true);                    
                    screenState = ScreenState.MainPage;
                    RefreshValues();
                    break;

                default:
                    break;
            }
        }

        public void RefreshValues()
        {
            switch (screenState)
            {
                case ScreenState.Closed:
                    break;
                case ScreenState.MainPage:
                    Title.text = $"Table de bord du navire {ship.name}";

                    FeedValue(Dodris, ship.shipBoard.dodris, 0, 100000);
                    FeedValue(Food, ship.shipBoard.food, 0, 100);
                    FeedValue(Order, ship.shipBoard.order, 0, 100);
                    FeedValue(Rigging, ship.shipBoard.rigging, 0, 100);
                    FeedValue(Hull, ship.shipBoard.hull, 0, 100);
                    FeedValue(OilBarrels, ship.shipBoard.oilBarrels, 0, 100);

                    break;
                case ScreenState.SailorsPage:
                    Title.text = $"Table de bord du navire {ship.name}";
                    break;
                default:
                    break;
            }
        }

        private void FeedValue(RectTransform panel, int value, int minValue, int maxValue)
        {
            panel.Find("Value").GetComponent<TextMeshProUGUI>().text = $"{value}";
            var statusBar = panel.Find("StatusBar").GetComponent<Slider>();
            statusBar.minValue = minValue;
            statusBar.maxValue = maxValue;
            statusBar.value = value;
        }
    }

    public enum ScreenState
    {
        Closed = 0,
        MainPage = 1,
        SailorsPage = 2
    }
}