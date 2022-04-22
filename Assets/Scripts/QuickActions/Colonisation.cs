using ColanderSource;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;
using System.Linq;

namespace Colfront.GamePlay
{

    public class Colonisation : MonoBehaviour
    {
        [Header("Global")]
        public GameObject dialog;
        public TextMeshProUGUI text;
        public TextMeshProUGUI islandName;
        public Button button;

        [Header("Matelots")]
        public Slider sailorsSlider;
        public TextMeshProUGUI shipSailorsText;
        public TextMeshProUGUI islandSailorsText;
        private int sailorsValue;

        [Header("Vivres")]
        public Slider foodSlider;
        public TextMeshProUGUI shipFoodText;
        public TextMeshProUGUI islandFoodText;
        private int foodValue;

        [Header("BarellsInfo")] 
        public TextMeshProUGUI barellsInfo;

        private GameManager gm;
        private DevOptionsManager dom;
        private Ship humanShip;
        private Island island;

        // Start is called before the first frame update
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            dom = FindObjectOfType<DevOptionsManager>();

            sailorsSlider.onValueChanged.AddListener(SailorsValueChanged);
            foodSlider.onValueChanged.AddListener(FoodValueChanged);

            button = GetComponent<Button>();
            button.onClick.AddListener(InitializeColonization);
        }

        public void InitializeColonization()
        {
            humanShip = GameManager.Instance.CurrentShipToPlay;
            island = ServiceGame.GetIsland(humanShip.coordinates);

            dialog.SetActive(true);
            UpdateColonisationText();

            // matelots
            // le nombre à laisser par défaut est de 15;
            sailorsValue = 15;
            sailorsSlider.value = sailorsValue;
            sailorsSlider.minValue = sailorsValue;
            sailorsSlider.maxValue = humanShip.crew.Count - 20;
            UpdateText(islandSailorsText, sailorsValue);
            int shipSailorsValue = humanShip.crew.Count - sailorsValue;
            UpdateText(shipSailorsText, shipSailorsValue);

            foodValue = sailorsValue;
            foodSlider.value = foodValue;
            foodSlider.minValue = foodValue;
            foodSlider.maxValue = humanShip.shipBoard.food - 20;
            UpdateText(islandFoodText, sailorsValue);
            int shipFoodValue = humanShip.shipBoard.food - foodValue;
            UpdateText(shipFoodText, shipFoodValue);
        }

        public void DoColonize()
        {
            // sélection des matelots qui vont dégager du navire
            var victims = ServiceGame.GetNpcs(humanShip).Where(x => x.rankEnum == Rank.SAILOR)
                .OrderByDescending(x => x.characteristics.BATISSEUR).Take(sailorsValue).ToList();

            ColonisationDTO dto = new ColonisationDTO
            {
                food = foodValue,
                island = island,
                npcs = victims,
                ship = humanShip,
                order = 15
            };
            ServiceGame.ColonizeIsland(dto);

            dialog.SetActive(false);
        }

        private void UpdateColonisationText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Colonisation de {island.name}");
            text.text = sb.ToString();

            islandName.text = island.name;
        }

        private void SailorsValueChanged(float value)
        {
            sailorsValue = Convert.ToInt32(Math.Floor(value));
            UpdateText(islandSailorsText, sailorsValue);
            
            // calcul pour le navire
            int shipSailorsValue = humanShip.crew.Count - sailorsValue;
            UpdateText(shipSailorsText, shipSailorsValue);

            barellsInfo.gameObject.SetActive(foodValue < sailorsValue);
        }

        private void FoodValueChanged(float value)
        {
            foodValue = Convert.ToInt32(Math.Floor(value));
            UpdateText(islandFoodText, foodValue);

            // calcul pour le navire
            int shipFoodValue = humanShip.shipBoard.food - foodValue;
            UpdateText(shipFoodText, shipFoodValue);

            barellsInfo.gameObject.SetActive(foodValue < sailorsValue);
        }

        private void UpdateText(TextMeshProUGUI text, int value)
        {
            text.text = value.ToString();
        }
    }
}