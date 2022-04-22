using ColanderSource;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Colfront.GamePlay
{
    public class PopulateWorld : MonoBehaviour
    {
        private Button button;
        private DevOptionsManager dom;
        private bool waitForPopulate;

        // Start is called before the first frame update
        void Start()
        {
            dom = FindObjectOfType<DevOptionsManager>(true);

            button = GetComponent<Button>();
            button.onClick.AddListener(DoPopulateWorld);
        }

        void Update()
        {
            if (waitForPopulate && GameManager.Instance.squaresShowed)
            {
                waitForPopulate = false;
                PopulateWorldTask();
            }
        }

        public void DoPopulateWorld()
        {
            GenerateMap();
            waitForPopulate = true;
        }

        public void GenerateMap()
        {
            //if (gm == null) gm = FindObjectOfType<GameManager>(true);
            if (dom == null) dom = FindObjectOfType<DevOptionsManager>();

            //ServiceGame.GenerateGame("GameData/v1testdata_100npc.txt");
            ServiceGame.GenerateGame(100);
            //gm.game = new ServiceGame();
            //gm.game.GenerateGame(50);
            //gm.game.GenerateGame("GameData/v1testdata_100npc.txt");

            // création des cases
            GameManager.Instance.CreateSquares();
        }

        public void PopulateWorldTask()
        {
            // spawn du joueur
            GameManager.Instance.PlayerSpawn();
            //yield return null;

            // spawn des navires npc
            GameManager.Instance.NpcsSpawn();
            //yield return null;

            GameManager.Instance.StartGame();

            Debug.Log($"Le monde a été peuplé avec succès !");
            Debug.Log($"Nombre d'îles : {ServiceGame.Islands.Count()}");
            Debug.Log($"Nombre de factions : {ServiceGame.Factions.Count()}");
            Debug.Log($"Nombre de PNJ sur les îles: {ServiceGame.GetGroundNpcs.Count()}");
            Debug.Log($"Nombre de PNJ à bord des navires: {ServiceGame.GetNpcs(ServiceGame.Ships).Count()}");
            //Debug.Log($"C'est au tour du navire {GameManager.Instance.CurrentShipToPlay.name } / faction : { ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).name } de jouer");

            foreach (var faction in ServiceGame.Factions)
            {
                Debug.Log($"Nombre de vaisseau de la faction {faction.name} : {ServiceGame.GetShipsFromFaction(faction).Count()}");
            }

            dom.currentState = DevOptionsManager.GameState.GameStarted;
            dom.RefreshGameState();
        }
    }
}