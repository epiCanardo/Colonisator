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

        public void ShowBoard()
        {           
            foreach (Faction faction in ServiceGame.Factions.OrderBy(x=>x.name))
            {
                var factionLine = Instantiate(factionContentLine, parent.transform);

                factionLine.Find("FactionCell").Find("Text").GetComponent<TextMeshProUGUI>().text = faction.longName;
                factionLine.Find("FactionCell").Find("Flag").GetComponent<RawImage>().texture = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).Flag;

                Npc factionBoss = ServiceGame.FactionBoss(faction.id);
                factionLine.Find("BossNameCell").Find("Text").GetComponent<TextMeshProUGUI>().text = factionBoss?.fullName;

                int dodris = factionBoss.money;
                factionLine.Find("BossDodrisCell").Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(dodris);
            }
        }
    }
}
