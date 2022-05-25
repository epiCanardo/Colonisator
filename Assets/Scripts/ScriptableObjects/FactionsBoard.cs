using ColanderSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public class FactionsBoard : MonoBehaviour
    {
        public GameObject parent;
        public Transform factionContentLine;
        public Transform factionNameSorter;
        public Transform bossNameSorter;
        public Transform bossFortuneSorter;
        public Transform peopleSorter;

        private List<Transform> lines = new List<Transform>();
        private bool isDescSort;

        void Start()
        {
            factionNameSorter.gameObject.AddComponent<Button>().onClick.AddListener(FactionNameSort);
            bossNameSorter.gameObject.AddComponent<Button>().onClick.AddListener(BossNameSort);
            bossFortuneSorter.gameObject.AddComponent<Button>().onClick.AddListener(BossFortuneSort);
            peopleSorter.gameObject.AddComponent<Button>().onClick.AddListener(PeopleSort);
        }

        private void FactionNameSort()
        {
            isDescSort = !isDescSort;
            SortBoard(x => x.longName);
        }
        private void BossNameSort()
        {
            isDescSort = !isDescSort;
            SortBoard(x => ServiceGame.FactionBoss(x.id).fullName);
        }
        private void BossFortuneSort()
        {
            isDescSort = !isDescSort;
            SortBoard(x => ServiceGame.FactionBoss(x.id).money);
        }
        private void PeopleSort()
        {
            isDescSort = !isDescSort;
            SortBoard(x => ServiceGame.FactionPopulation(x.id));
        }

        public void SortBoard<T>(Func<Faction, T> sorter)
        {
            int i = 0;
            IEnumerable<Faction> sortedFactions = (isDescSort)
                ? ServiceGame.Factions.OrderByDescending(sorter)
                : ServiceGame.Factions.OrderBy(sorter);

            foreach (Faction faction in sortedFactions)
            {
                var factionLine = lines[i];
                FeedFactionLine(factionLine, faction);
                i++;
            }
        }

        public void ShowBoard<T>(Func<Faction, T> sorter)
        {
            foreach (Faction faction in ServiceGame.Factions.OrderBy(sorter))
            {
                var factionLine = Instantiate(factionContentLine, parent.transform);
                FeedFactionLine(factionLine, faction);
                lines.Add(factionLine);
            }
        }

        private static void FeedFactionLine(Transform factionLine, Faction faction)
        {
            factionLine.Find("FactionCell").Find("Text").GetComponent<TextMeshProUGUI>().text = faction.longName;
            factionLine.Find("FactionCell").Find("Flag").GetComponent<RawImage>().texture =
                FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).Flag;

            Npc factionBoss = ServiceGame.FactionBoss(faction.id);
            factionLine.Find("BossNameCell").Find("Text").GetComponent<TextMeshProUGUI>().text = factionBoss.fullName;

            int dodris = factionBoss.money;
            factionLine.Find("BossDodrisCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(dodris);

            int population = ServiceGame.FactionPopulation(faction.id);
            factionLine.Find("FactionPopulationCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(population);
        }
    }
}
