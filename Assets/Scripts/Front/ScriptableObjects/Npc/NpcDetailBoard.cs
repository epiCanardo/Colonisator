using System.Linq;
using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.Front.ScriptableObjects.Faction;
using Assets.Scripts.Service;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.ScriptableObjects.Npc
{
    public class NpcDetailBoard : UIBoard
    {
        public override string key => "npcDetailBoard";
        
        public Model.Npc npc;

        [Header("Titre")]
        public TextMeshProUGUI title;

        [Header("Data")]
        public TextMeshProUGUI fullNameText;
        public TextMeshProUGUI ageText;
        public TextMeshProUGUI sexText;
        public TextMeshProUGUI rankText;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI heightText;
        public TextMeshProUGUI weightText;
        public TextMeshProUGUI fortuneText;
        public TextMeshProUGUI factionText;
        public TextMeshProUGUI localisationText;
        public TextMeshProUGUI aspirationText;
        public TextMeshProUGUI factionLoyaltyText;

        // caractéristiques
        public TextMeshProUGUI consitutionText;
        public TextMeshProUGUI mentalText;

        public TextMeshProUGUI ambitionText;
        public TextMeshProUGUI venalityText;
        public TextMeshProUGUI temerityText;
        public TextMeshProUGUI leadershipText;

        public TextMeshProUGUI artileryText;
        public TextMeshProUGUI combatText;
        public TextMeshProUGUI navigationText;
        public TextMeshProUGUI negociationText;
        public TextMeshProUGUI builderText;
        public TextMeshProUGUI medecineText;
        public TextMeshProUGUI cookingText;
        public TextMeshProUGUI craftmanshipText;
        public TextMeshProUGUI intendanceText;

        public TextMeshProUGUI alcoolismeText;

        void Start()
        {
            Model.Faction faction = ServiceGame.GetFactionFromId(npc.faction);


            title.text = $"Détail de {npc.fullName}, {npc.description}";

            MeshRenderer rend = GameManager.Instance.PortraitFlagTextile.GetComponent<MeshRenderer>();
            rend.material.SetTexture("flagTexture", FactionsManager.Instance.Factions.First(x => x.Faction.id.Equals(npc.faction)).Flag);
            rend.material.SetFloat("amplitude", UnityEngine.Random.Range(3, 8));

            factionText.GetComponent<FactionLink>().faction = faction;

            // infos de base
            fullNameText.text = npc.fullName;
            ageText.text = $"{npc.age} ans";
            sexText.text = npc.Sexe;
            rankText.text = npc.Rang;
            healthText.text = npc.EtatSante;
            heightText.text = $"{npc.size} cm";
            weightText.text = $"{npc.weight} kg";
            fortuneText.text = npc.money.ToString();
            factionText.text = faction.longName;
            localisationText.text = npc.Localisation;
            aspirationText.text = npc.Aspiration;
            factionLoyaltyText.text = npc.FactionLotalty;

            // caracs
            consitutionText.text = npc.characteristics.CONSTITUTION.ToString();
            mentalText.text = npc.characteristics.MENTAL.ToString();
            ambitionText.text = npc.characteristics.AMBITION.ToString();
            venalityText.text = npc.characteristics.VENALITE.ToString();
            temerityText.text = npc.characteristics.TEMERITE.ToString();
            leadershipText.text = npc.characteristics.LEADERSHIP.ToString();

            artileryText.text = npc.characteristics.ARTILLERIE.ToString();
            combatText.text = npc.characteristics.COMBAT.ToString();
            navigationText.text = npc.characteristics.NAVIGATION.ToString();
            negociationText.text = npc.characteristics.NEGOCIATION.ToString();
            builderText.text = npc.characteristics.BATISSEUR.ToString();
            medecineText.text = npc.characteristics.MEDECINE.ToString();
            cookingText.text = npc.characteristics.CUISINE.ToString();
            craftmanshipText.text = npc.characteristics.ARTISANAT.ToString();
            intendanceText.text = npc.characteristics.INTENDANCE.ToString();

            alcoolismeText.text = npc.characteristics.ALCOOLISME.ToString();
        }
    }
}