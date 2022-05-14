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
                factionNameObject.Find("FactionText").GetComponent<TextMeshProUGUI>().text = faction.name;

                var bossNameObject = Instantiate(cell, grid.transform);
                bossNameObject.Find("FactionText").GetComponent<TextMeshProUGUI>().text = "BOUCHON";

                Instantiate(cell, grid.transform);
            }
        }
    }
}
