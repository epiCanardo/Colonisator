using ColanderSource;
using System;
using System.Linq;
using TMPro;
using UnityEngine;


namespace Colfront.GamePlay
{
    public class NpcDetailBoard : UIBoard
    {
        public Npc npc;

        [Header("Titre")]
        public TextMeshProUGUI title;

        [Header("Data")]
        public TextMeshProUGUI fullNameText;
        public TextMeshProUGUI ageText;
        public TextMeshProUGUI sexText;
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI healthText;

        void Start()
        {
            title.text = $"Détail de {npc.fullName}, {npc.description}";

            MeshRenderer rend = GameManager.Instance.PortraitFlagTextile.GetComponent<MeshRenderer>();
            rend.material.SetTexture("flagTexture", FactionsManager.Instance.Factions.First(x => x.Faction.id.Equals(npc.faction)).Flag);
            rend.material.SetFloat("amplitude", UnityEngine.Random.Range(3, 8));

            fullNameText.text = npc.fullName;
            ageText.text = npc.age.ToString();
            sexText.text = npc.Sexe;
            rankText.text = npc.Rang;
            healthText.text = npc.EtatSante;
        }
    }
}
