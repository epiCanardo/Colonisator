using ColanderSource;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

namespace Colfront.GamePlay
{
    public class PopulateWorld : MonoBehaviour
    {
        private Button button;
        private bool waitForPopulate;

        private List<string> technicalreport = new List<string>();

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(DoPopulateWorld);
        }

        void Update()
        {
            //if (waitForPopulate && GameManager.Instance.squaresShowed)
            //{
            //    waitForPopulate = false;
            //    PopulateWorldTask();                
            //}
        }

        public void DoPopulateWorld()
        {
            Stopwatch timerGlobal = Stopwatch.StartNew();

            Stopwatch timer = Stopwatch.StartNew();
            GenerateMap();            
            timer.Stop();
            technicalreport.Add($"Temps de cr�ation total du monde : {timer.Elapsed.TotalSeconds}");
            //waitForPopulate = true;

            Stopwatch timer2 = Stopwatch.StartNew();
            PopulateWorldTask();
            timer2.Stop();
            technicalreport.Add($"Temps de population total du monde : {timer2.Elapsed.TotalSeconds}");

            timerGlobal.Stop();
            technicalreport.Add($"Temps total : {timerGlobal.Elapsed.TotalSeconds}");

            using (StreamWriter sW = new StreamWriter("Reports/technicLog.txt"))
            {
                foreach (var item in technicalreport)
                {
                    sW.WriteLine(item);
                }
                sW.Close();
            }
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            ServiceGame.GenerateGame(100);
            timer.Stop();
            technicalreport.Add($"Temps de g�n�ration de la partie : {timer.Elapsed.TotalSeconds}");

            // cr�ation des cases
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.CreateSquares();
            timer2.Stop();
            technicalreport.Add($"Temps de g�n�ration de la map : {timer2.Elapsed.TotalSeconds}");
        }

        public void PopulateWorldTask()
        {
            // spawn du joueur
            Stopwatch timer = Stopwatch.StartNew();            
            GameManager.Instance.PlayerSpawn();
            timer.Stop();
            technicalreport.Add($"Temps de cr�ation du joueur humain : {timer.Elapsed.TotalSeconds}");
            //yield return null;

            // spawn des navires npc
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.NpcsSpawn();
            timer2.Stop();
            technicalreport.Add($"Temps de cr�ation des autres factions : {timer2.Elapsed.TotalSeconds}");
            //yield return null;

            Stopwatch timer3 = Stopwatch.StartNew();
            GameManager.Instance.StartGame();
            timer3.Stop();
            technicalreport.Add($"Temps de d�marrage du premier tour : {timer3.Elapsed.TotalSeconds}");

            //Debug.Log($"Le monde a �t� peupl� avec succ�s !");
            //Debug.Log($"Nombre d'�les : {ServiceGame.Islands.Count()}");
            //Debug.Log($"Nombre de factions : {ServiceGame.Factions.Count()}");
            //Debug.Log($"Nombre de PNJ sur les �les: {ServiceGame.GetGroundNpcs.Count()}");
            //Debug.Log($"Nombre de PNJ � bord des navires: {ServiceGame.GetNpcs(ServiceGame.Ships).Count()}");
            ////Debug.Log($"C'est au tour du navire {GameManager.Instance.CurrentShipToPlay.name } / faction : { ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).name } de jouer");

            //foreach (var faction in ServiceGame.Factions)
            //{
            //    Debug.Log($"Nombre de vaisseau de la faction {faction.name} : {ServiceGame.GetShipsFromFaction(faction).Count()}");
            //}

            DevOptionsManager.Instance.currentState = DevOptionsManager.GameState.GameStarted;
            DevOptionsManager.Instance.RefreshGameState();
        }
    }
}