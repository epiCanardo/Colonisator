using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.Front.ScriptableObjects.Npc;
using Assets.Scripts.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.ScriptableObjects.Faction
{
    public class FactionsBoard : UIBoard
    {
        public override string key => "factionsBoard";

        public GameObject parent;

        [Header("Titre")]
        public TextMeshProUGUI title;

        [Header("Lignes")]
        public Transform factionContentLine;
        public Transform factionNameSorter;
        public Transform bossNameSorter;
        public Transform bossFortuneSorter;
        public Transform peopleSorter;
        public Transform islandsSorter;
        public Transform shipsSorter;

        private List<Transform> lines = new List<Transform>();
        private bool isDescSort;

        private Transform activeSorter;

        void Start()
        {
            title.text = "Les Factions";

            factionNameSorter.gameObject.AddComponent<Button>().onClick.AddListener(FactionNameSort);
            bossNameSorter.gameObject.AddComponent<Button>().onClick.AddListener(BossNameSort);
            bossFortuneSorter.gameObject.AddComponent<Button>().onClick.AddListener(BossFortuneSort);
            peopleSorter.gameObject.AddComponent<Button>().onClick.AddListener(PeopleSort);
            islandsSorter.gameObject.AddComponent<Button>().onClick.AddListener(IslandsCountSort);
            shipsSorter.gameObject.AddComponent<Button>().onClick.AddListener(delegate { ShipsCountSort(shipsSorter); });
        }

        private void ShipsCountSort(Transform sorterObject)
        {
            activeSorter = sorterObject;
            SortBoard(x => ServiceGame.FactionOwnedIslands(x.id).Count());
        }

        private void IslandsCountSort()
        {
            SortBoard(x => ServiceGame.FactionOwnedIslands(x.id).Count());
        }

        private void FactionNameSort()
        {
            SortBoard(x => x.longName);
        }
        private void BossNameSort()
        {
            SortBoard(x => ServiceGame.FactionBoss(x.id).fullName);
        }
        private void BossFortuneSort()
        {
            SortBoard(x => ServiceGame.FactionBoss(x.id).money);
        }
        private void PeopleSort()
        {
            SortBoard(x => ServiceGame.FactionPopulation(x.id));
        }

        public void SortBoard<T>(Func<Model.Faction, T> sorter)
        {
            //activeSorter.GetComponent<RawImage>().color = Color.red;
            isDescSort = !isDescSort;
            int i = 0;
            IEnumerable<Model.Faction> sortedFactions = (isDescSort)
                ? ServiceGame.Factions.OrderByDescending(sorter)
                : ServiceGame.Factions.OrderBy(sorter);

            foreach (Model.Faction faction in sortedFactions)
            {
                var factionLine = lines[i];
                FeedFactionLine(factionLine, faction);
                i++;
            }
        }

        public void ShowBoard<T>(Func<Model.Faction, T> sorter)
        {
            foreach (Model.Faction faction in ServiceGame.Factions.OrderBy(sorter))
            {
                var factionLine = Instantiate(factionContentLine, parent.transform);
                FeedFactionLine(factionLine, faction);
                lines.Add(factionLine);
            }
        }

        private static void FeedFactionLine(Transform factionLine, Model.Faction faction)
        {
            var factionCell = factionLine.Find("FactionCell");
            factionCell.Find("Text").GetComponent<TextMeshProUGUI>().text = faction.longName;
            factionCell.Find("Flag").GetComponent<RawImage>().texture =
                FactionsManager.Instance.GetFactionManager(faction).Flag;

            factionCell.Find("Text").GetComponent<FactionLink>().faction = faction;

            Model.Npc factionBoss = ServiceGame.FactionBoss(faction.id);
            var bossCell = factionLine.Find("BossNameCell");
            bossCell.Find("Text").GetComponent<TextMeshProUGUI>().text = factionBoss.fullName;
            bossCell.Find("Text").GetComponent<NpcLink>().npc = factionBoss;

            int dodris = factionBoss.money;
            factionLine.Find("BossDodrisCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(dodris);

            int population = ServiceGame.FactionPopulation(faction.id);
            factionLine.Find("FactionPopulationCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(population);

            int islandsCount = ServiceGame.FactionOwnedIslands(faction.id).Count();
            factionLine.Find("FactionOwnedIslandsCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(islandsCount);

            int shipsCount = ServiceGame.FactionOwnedShips(faction.id).Count();
            factionLine.Find("FactionOwnedShipsCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(shipsCount);
        }
    }
}
