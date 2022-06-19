using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.ModsDTO;
using Assets.Scripts.Service;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Front.ScriptableObjects.OptionsMenu
{
    public class NewGameLink : LinkObject
    {
        public GameObject loadingScreen;
        private readonly List<string> _technicalreport = new List<string>();

        public override void OnPointerDown(PointerEventData eventData)
        {         
            StartCoroutine("LoadingSequence");       
        }

        IEnumerator LoadingSequence()
        {
            var loading = Instantiate(loadingScreen, GameManager.Instance.canvas.transform);
            yield return loading.transform.GetComponent<Image>().DOFade(1, 1).WaitForCompletion();

            DoPopulateWorld();
            yield return loading.transform.GetComponent<Image>().DOFade(0, 1).WaitForCompletion();

            Destroy(loading);
            MenusManager.Instance.TryDestroyMenu("systemOptions");
        }

        void DoPopulateWorld()
        {
            Stopwatch timerGlobal = Stopwatch.StartNew();

            Stopwatch timer = Stopwatch.StartNew();
            GenerateMap();
            timer.Stop();
            _technicalreport.Add($"Temps de création total du monde : {timer.Elapsed.TotalSeconds}");

            // chargement des mods
            LoadMods();

            // envoi des cartes au back
            ServiceGame.SendCards();

            Stopwatch timer2 = Stopwatch.StartNew();
            PopulateWorldTask();
            timer2.Stop();
            _technicalreport.Add($"Temps de population total du monde : {timer2.Elapsed.TotalSeconds}");

            timerGlobal.Stop();
            _technicalreport.Add($"Temps total : {timerGlobal.Elapsed.TotalSeconds}");

            using (StreamWriter sW = new StreamWriter("Reports/technicLog.txt"))
            {
                foreach (var item in _technicalreport)
                {
                    sW.WriteLine(item);
                }
                sW.Close();
            }
        }

        public void GenerateMap()
        {
            Stopwatch timer = Stopwatch.StartNew();
            ServiceGame.GenerateGame(1000, string.Empty);
            timer.Stop();
            _technicalreport.Add($"Temps de génération de la partie : {timer.Elapsed.TotalSeconds}");

            // création des cases
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.CreateSquares();
            timer2.Stop();
            _technicalreport.Add($"Temps de génération de la map : {timer2.Elapsed.TotalSeconds}");
        }
        private void PopulateWorldTask()
        {
            // spawn du joueur
            Stopwatch timer = Stopwatch.StartNew();
            GameManager.Instance.PlayerSpawn();
            timer.Stop();
            _technicalreport.Add($"Temps de création du joueur humain : {timer.Elapsed.TotalSeconds}");

            // spawn des navires npc
            Stopwatch timer2 = Stopwatch.StartNew();
            GameManager.Instance.NpcsSpawn();
            timer2.Stop();
            _technicalreport.Add($"Temps de création des autres factions : {timer2.Elapsed.TotalSeconds}");
            
            Stopwatch timer3 = Stopwatch.StartNew();
            GameManager.Instance.StartGame();
            timer3.Stop();
            _technicalreport.Add($"Temps de démarrage du premier tour : {timer3.Elapsed.TotalSeconds}");
        }

        private void LoadMods()
        {
            Stopwatch timer = Stopwatch.StartNew();
            ModManager.Instance.Initialization();
            timer.Stop();
            _technicalreport.Add($"Temps de chargement des mods : {timer.Elapsed.TotalSeconds}");
        }
    }
}