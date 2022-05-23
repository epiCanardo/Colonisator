using ColanderSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Colfront.GamePlay
{
    public class FactionsBoard : MonoBehaviour
    {
        public GameObject grid;
        public Transform cell;

        public void ShowBoard()
        {
            foreach (Faction faction in ServiceGame.Factions)
            {
                var factionNameObject = Instantiate(cell, grid.transform);
                factionNameObject.Find("Text").GetComponent<TextMeshProUGUI>().text = faction.name;

                var bossNameObject = Instantiate(cell, grid.transform);
                Npc factionBoss = ServiceGame.FactionBoss(faction.id);
                bossNameObject.Find("Text").GetComponent<TextMeshProUGUI>().text = factionBoss?.fullName;

                int dodris = (factionBoss != null) ? factionBoss.money : 0; // c'est ballot hein !?
                var dodrisObject = Instantiate(cell, grid.transform);
                dodrisObject.Find("Text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(dodris);
            }
        }
    }
}
