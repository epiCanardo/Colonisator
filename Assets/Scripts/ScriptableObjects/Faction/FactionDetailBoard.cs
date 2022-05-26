using ColanderSource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public class FactionDetailBoard : UIBoard
    {
        public Faction faction;

        [Header("Titre")]
        public TextMeshProUGUI title;

        void Start()
        {
            title.text = $"Détail de la faction {faction.longName}";
        }
    }
}
