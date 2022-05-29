using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace Colfront.GamePlay
{
    public class NewGameLink : LinkObject
    {
        public GameObject loadingScreen;
        private List<string> technicalreport = new List<string>();

        public override void OnPointerDown(PointerEventData eventData)
        {         
            StartCoroutine("LoadingSequence");       
        }

        IEnumerator LoadingSequence()
        {
            //MenusManager.Instance.HideMenu(MenusManager.MenuType.Options);

            var loading = Instantiate(loadingScreen, GameManager.Instance.canvas.transform);
            yield return loading.transform.GetComponent<Image>().DOFade(1, 1).WaitForCompletion();
            //yield return new WaitForSeconds(1);
            //yield return new WaitForSeconds(1);

            DoPopulateWorld();


            yield return loading.transform.GetComponent<Image>().DOFade(0, 1).WaitForCompletion();
            //yield return new WaitForSeconds(1);
            Destroy(loading);
            //MenusManager.Instance.TryDestroyMenu(MenusManager.MenuType.Options);
        }

        void DoPopulateWorld()
        {
            Stopwatch timerGlobal = Stopwatch.StartNew();

            Stopwatch timer = Stopwatch.StartNew();
            GenerateMap();
            timer.Stop();
            technicalreport.Add($"Temps de création total du monde : {timer.Elapsed.TotalSeconds}");
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
            //ServiceGame.GenerateGame("GameData/v1testdata_100npc.txt");
            Stopwatch timer = Stopwatch.StartNew();
            ServiceGame.GenerateGame(100);
            timer.Stop();
            technicalreport.Add($"Temps de génération de la partie : {timer.Elapsed.TotalSeconds}");
            //gm.game = new ServiceGame();
            //gm.game.GenerateGame(50);
            //gm.game.GenerateGame("GameData/v1testdata_100npc.txt");

            // création des cases
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.CreateSquares();
            timer2.Stop();
            technicalreport.Add($"Temps de génération de la map : {timer2.Elapsed.TotalSeconds}");
        }

        public void PopulateWorldTask()
        {
            // spawn du joueur
            Stopwatch timer = Stopwatch.StartNew();
            GameManager.Instance.PlayerSpawn();
            timer.Stop();
            technicalreport.Add($"Temps de création du joueur humain : {timer.Elapsed.TotalSeconds}");
            //yield return null;

            // spawn des navires npc
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.NpcsSpawn();
            timer2.Stop();
            technicalreport.Add($"Temps de création des autres factions : {timer2.Elapsed.TotalSeconds}");
            //yield return null;

            Stopwatch timer3 = Stopwatch.StartNew();
            GameManager.Instance.StartGame();
            timer3.Stop();
            technicalreport.Add($"Temps de démarrage du premier tour : {timer3.Elapsed.TotalSeconds}");

            //Debug.Log($"Le monde a été peuplé avec succès !");
            //Debug.Log($"Nombre d'îles : {ServiceGame.Islands.Count()}");
            //Debug.Log($"Nombre de factions : {ServiceGame.Factions.Count()}");
            //Debug.Log($"Nombre de PNJ sur les îles: {ServiceGame.GetGroundNpcs.Count()}");
            //Debug.Log($"Nombre de PNJ à bord des navires: {ServiceGame.GetNpcs(ServiceGame.Ships).Count()}");
            ////Debug.Log($"C'est au tour du navire {GameManager.Instance.CurrentShipToPlay.name } / faction : { ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).name } de jouer");

            //foreach (var faction in ServiceGame.Factions)
            //{
            //    Debug.Log($"Nombre de vaisseau de la faction {faction.name} : {ServiceGame.GetShipsFromFaction(faction).Count()}");
            //}
        }
    }
}