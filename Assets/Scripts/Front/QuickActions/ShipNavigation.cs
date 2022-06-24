using Assets.Scripts.Front.MainManagers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.QuickActions
{

    public class ShipNavigation : UnityEngine.MonoBehaviour
    {
        public GameObject dialog;
        public TextMeshProUGUI text;
        public NavigationModeManager navMode;

        private GameManager gm;
        private Button button;
        private DevOptionsManager dom;

        // Start is called before the first frame update
        void Start()
        {
            gm = GameManager.Instance;
            //dom = FindObjectOfType<DevOptionsManager>();

            button = GetComponent<Button>();
            button.onClick.AddListener(LaunchNavMode);
        }

        public void LaunchNavMode()
        {
            // si le mode navigation n'est pas actif, on le lance
            if (!gm.IsNavigationModeActive())
            {
                var ship = GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().ship;
                navMode.StartNavigationMode(ship);
                navMode.squaresRemaning = Random.Range(2, 13);
                navMode.windDirection = Random.Range(1, 5);
                navMode.UpdateMoveText();

                // affichage de la dialog de suivi du mouvement
                dialog.SetActive(true);

                // activation des cases dans la scene principale
                gm.ToggleNavigationMode(true);

                // refesh des dev options
                // TODO ne sert plus à rien
                //dom.currentState = DevOptionsManager.GameState.TurnStarted;
               // dom.RefreshGameState();
            }
        }
    }
}