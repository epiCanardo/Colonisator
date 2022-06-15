using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.ScriptableObjects.Faction
{
    public class FactionDetailBoard : UIBoard
    {
        public Model.Faction faction;

        [Header("Titre")]
        public TextMeshProUGUI title;

        void Start()
        {
            title.text = $"Détail de la faction {faction.longName}";
        }

        public override string key => "factionDetailBoard";
    }
}
