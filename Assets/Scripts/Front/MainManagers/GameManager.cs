using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Front.Cams;
using Assets.Scripts.Front.ScriptableObjects.Faction;
using Assets.Scripts.Front.ScriptableObjects.Npc;
using Assets.Scripts.Front.ShipScreen;
using Assets.Scripts.Front.Squares;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using Assets.Scripts.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{
    public class GameManager : UnityEngine.MonoBehaviour
    {
        public Canvas canvas;
        
        [Header("Gestion des cases")]
        public GameObject squarePrefab;
        public GameObject harborSquarePrefab;
        public GameObject ile3HarborSqurePrefab;
        public GameObject sundercityHarborSqurePrefab;
        public GameObject ileNeutre1HarborSqurePrefab;
        public GameObject squaresParent;

        [Header("Gestion du panneau d'affichage du navire")]
        public RectTransform shipScreen;

        [Header("Gestion du tooltip de port")]
        //public Canvas harborToolTip;
        public RectTransform harborToolTip;

        [Header("Gestion des navires")]
        public GameObject mainShipPrefab;
        public GameObject[] cuiiShipPrefabs;
        public GameObject sundercityShipPrefab;
        public GameObject piofoShipPrefab;
        public GameObject[] cplShipPrefabs;
        public GameObject missytownShipPrefab;
        public GameObject ghostShipPrefab;
        public GameObject[] cmrShipPrefabs;
        public GameObject[] competitorShipPrefabs;

        [Header("Panneau info sélection")]
        public TextMeshProUGUI PanelActionSelectionShipName;
        public TextMeshProUGUI PanelActionSelectionFactionName;
        public TextMeshProUGUI PanelActionSelectionCaptainName;
        public TextMeshProUGUI PanelActionSelectionCrewCount;
        public TextMeshProUGUI PanelActionSelectionOfficerCount;
        public TextMeshProUGUI PanelActionSelectionFoodCount;

        public GameObject PanelActionSelectionFactionFlag;
        public Image PanelActionSelectionBorder;

        [Header("Map")]
        public GameObject PortraitFlagTextile;
        public RawImage miniMap;

        public TextMeshProUGUI coordinatesTextObject;

        public Vector3 camOffSet { get; set; }
        public Vector3 camEulerAngles { get; set; }

        // l'instance du navire du joueur
        private List<GameObject> instanciedShipObjects = new List<GameObject>();
        //private GameObject playerShip;

        private float zSquaresStart = 817.1f;
        private float xSquaresStart = -2000f;
        private float zSize = 5000f;
        private float xSize = 5000f;
        private float zSquares = 100f;
        private float xSquares = 100f;
        private bool squaresShowed;
        private List<SquareManagement> squares = new List<SquareManagement>();
        private SquareManagement squareM;
        private SquareManagement harborSquareM;

        // mode navigation
        private bool navigationMode;
        private int currentQualityLevel = 0;

        public Ship CurrentShipToPlay { get; set; }        

        // récupère l'objet instancié correspondant au navire en cours
        public GameObject GetActualPlayinghipObject => instanciedShipObjects.First(x => x.GetComponent<ShipManager>().ship.Equals(CurrentShipToPlay));

        public GameObject GetPlayingHumanShipObject => instanciedShipObjects.First(x => x.GetComponent<ShipManager>().ship.Equals(ServiceGame.GetHumanShip));

        public void FocusCamOnShip(GameObject ship)
        {
            // postionnement de la caméra par rapport au navire actuel
            ToggleCamMovement(false);
            Camera.main.transform.position = ship.transform.position + Vector3.back * 100;
            Camera.main.GetComponent<CamMovement>().SetCamToActionLevel();

            //Camera.main.transform.eulerAngles = camEulerAngles;

            // on rend la camera libre si ce n'est pas le tour du joueur humain
            if (TurnManager.Instance.MainState != TurnState.AI)
                ToggleCamMovement(true);

            // sélection du navire
            ship.GetComponent<ShipManager>().SelectShip();
        }

        public void ToggleCamMovement(bool active)
        {
            //Camera.main.GetComponent<SimpleCameraController>().enabled = active;
        }

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        // Start is called before the first frame update
        void Start()
        {
            // définition des vecteurs de position et d'angle pour la camera principale
            //camOffSet = new Vector3(0, 300, -160);
            //camEulerAngles = new Vector3(60, 0, 0);

            camOffSet = new Vector3(0, 100, -100);
            //camEulerAngles = new Vector3(50, 0, 0);

            // la map doit être carrée (le plateau de jeu est un carré)
            // le carré est de 0.8 * le bord le plus petit de côté
            float shorterBounds = 0.8f * Mathf.Min(Screen.width, Screen.height);
            float xAnchor = 0.5f * (Screen.width - shorterBounds);
            float yAnchor = 0.5f * (Screen.height - shorterBounds);
        }

        // On lance la game !
        public void StartGame()
        {
            // affichage du tour courant
            StartCoroutine(TurnManager.Instance.StartTurn());

            miniMap.texture = ModManager.Instance.GenerateMinimapColors();

            //// affection du navire courant
            //CurrentShipToPlay = ServiceGame.GetShipsTurnOrder().First();
            //currentShipIndex = 0;

            //// positionnement de la cam
            //FocusCamOnCurrentPlayingShip();
        }

        public void SetInfoPanelBorderColor(Color color)
        {
            PanelActionSelectionBorder.color = color;
        }

        public void SetInfoPanelFlag(Texture2D texture)
        {
            //Fetch the RawImage component from the GameObject
            var raw = PanelActionSelectionFactionFlag.GetComponent<RawImage>();
            //Change the Texture to be the one you define in the Inspector
            raw.texture = texture;
        }

        /// <summary>
        /// affichage des informations sur la selection du navive en cours
        /// TODO : est ce le bon emplacement ? (plutôt shipmanager)
        /// </summary>
        /// <param name="ship"></param>
        public void SetInfoPanelTitle(Ship ship)
        {
            PanelActionSelectionShipName.text = ship.name;

            PanelActionSelectionFactionName.text = ServiceGame.GetFaction(ship).longName;
            PanelActionSelectionFactionName.GetComponent<FactionLink>().faction = ServiceGame.GetFaction(ship);

            PanelActionSelectionCaptainName.text = ServiceGame.ShipCaptain(ship).fullName;
            PanelActionSelectionCaptainName.GetComponent<NpcLink>().npc = ServiceGame.ShipCaptain(ship);

            PanelActionSelectionCrewCount.text = ServiceGame.ShipSailors(ship).Count().ToString();
            PanelActionSelectionOfficerCount.text = ServiceGame.ShipOfficiers(ship).Count().ToString();
            PanelActionSelectionFoodCount.text = CurrentShipToPlay.shipBoard.food.ToString();
        }

        // Update is called once per frame
        void Update()
        {
            // pas de contrôle en cas de tour IA
            if (TurnManager.Instance.MainState == TurnState.AI)
                return;

            // fin du tour
            if (TurnManager.Instance.MainState == TurnState.WaitForEndTurn && Input.GetKeyDown(KeyCode.Space))
                TurnManager.Instance.MainState = TurnState.ActionsFinished;

            // faire apparaitre / disparaitre les cases
            if (Input.GetKeyDown(KeyCode.C))            
                ToggleSquares(squaresShowed);            

            // positionnement sur le navire du joueur humain
            if (Input.GetKeyDown(KeyCode.S))
                FocusCamOnShip(GetPlayingHumanShipObject);

            // modification de la qualité
            if (Input.GetKeyDown(KeyCode.Q))
            {
                currentQualityLevel += 1;
                if (currentQualityLevel == 3)
                    currentQualityLevel = 0;

                QualitySettings.SetQualityLevel(currentQualityLevel);
            }
        }

        public void CreateSquares()
        {
            //StartCoroutine("CreateSquaresTask");
            CreateSquaresTask();
        }

        void CreateSquaresTask()
        {
            // création des cases
            List<Square> harbours = ServiceGame.Islands.Select(x => x.harbourCoordinates).ToList();

            Square basicSquare;
            Vector3 physicalSquare;
            squareM = squarePrefab.GetComponent<SquareManagement>();
            harborSquareM = harborSquarePrefab.GetComponent<SquareManagement>();

            var zstep = (zSize / zSquares);
            var xstep = (xSize / xSquares);

            for (int z = 0; z < 100; z++)
            {
                for (int x = 0; x < 100; x++)
                {
                    physicalSquare = new Vector3(xSquaresStart + (xstep / 2) + x * xstep, 10, zSquaresStart + (zstep / 2) + z * zstep);
                    basicSquare = new Square(x + 1, z + 1);
                    GameObject objectCreated;

                    if (harbours.Contains(basicSquare))
                    {
                        // on y va en dur comme des sales, c'est ça qu'on veut voir !
                        if (x + 1 == 47 && z + 1 == 90) CreateIsland(ile3HarborSqurePrefab, physicalSquare, basicSquare, "ile3");
                        else if (x + 1 == 5 && z + 1 == 96) CreateIsland(sundercityHarborSqurePrefab, physicalSquare, basicSquare, "sundercity");
                        else if (x + 1 == 27 && z + 1 == 69) CreateIsland(ileNeutre1HarborSqurePrefab, physicalSquare, basicSquare, "ileNeutre1");
                        else
                        {
                            objectCreated = Instantiate(harborSquarePrefab, physicalSquare, harborSquarePrefab.transform.rotation, squaresParent.transform);
                            objectCreated.name = $"HarborSquare{basicSquare.x}_{basicSquare.y}";
                            var squareManager = objectCreated.GetComponent<HarborSquareManagement>();
                            squareManager.coordinates = basicSquare;
                            squareManager.SetIsland();
                            squares.Add(squareManager);
                        }
                    }
                    else
                    {
                        objectCreated = Instantiate(squarePrefab, physicalSquare, squarePrefab.transform.rotation, squaresParent.transform);
                        objectCreated.name = $"Square{basicSquare.x}_{basicSquare.y}";
                        //objectCreated.GetComponent<SquareManagement>().coordinates = new Square(x + 1, z + 1);
                        //objectCreated.SetActive(false);
                        var squareManager = objectCreated.GetComponent<NavigableSquareManagement>();
                        squareManager.coordinates = basicSquare;
                        squares.Add(squareManager);

                        // si la case est non navigable
                        if (ModManager.Instance.IsSquareNonNavigable(squareManager.coordinates))
                            squareManager.SetOutlineColor(Color.red);
                    }

                    //objectCreated.GetComponent<SquareManagement>().coordinates = new Square(x + 1, z + 1);
                    //if (z < 80)
                    //    objectCreated.SetActive(false);
                }
                //yield return null;
            }

            squaresShowed = true;
        }

        private void CreateIsland(GameObject harborPrefab, Vector3 square, Square basicSquare, string name)
        {
            //harborPrefab.name = $"{name}_{basicSquare.x}_{basicSquare.y}";
            var objectCreated = Instantiate(harborPrefab, square, harborPrefab.transform.rotation, squaresParent.transform);
            var squareManager = objectCreated.GetComponent<HarborSquareManagement>();
            squareManager.coordinates = basicSquare;
            squareManager.SetIsland();
            squares.Add(squareManager);
        }

        /// <summary>
        /// Donne l'objet de case plateau � partir des coordon�es
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public SquareManagement GetPhysicalSquareFromSquare(Square square)
        {
            return squares.First(x => x.coordinates.Equals(square));
        }

        public void PlayersSpawn()
        {
            // Joueur
            Faction human = ServiceGame.Factions.First(x => x.playerTypeEnum == "HUMAN");
            SetFactionToManager(human, "blue", new List<Color32> { Color.blue, Color.white, Color.red });
            ShipsInstanciation(human, new GameObject[1] { mainShipPrefab });

            // CUII
            Faction cuii = ServiceGame.Factions.First(x => x.playerTypeEnum == "NEUTRAL");
            SetFactionToManager(cuii, "yellow", new List<Color32> { Color.yellow, Color.black, Color.white });
            ShipsInstanciation(cuii, cuiiShipPrefabs);

            // Sundercity
            Faction sundercity = ServiceGame.Factions.First(x => x.playerTypeEnum == "TOWN");
            SetFactionToManager(sundercity, "cyan", new List<Color32> { Color.cyan, Color.black, Color.white });
            ShipsInstanciation(sundercity, new GameObject[1] { sundercityShipPrefab });

            // Piofo
            Faction piofo = ServiceGame.Factions.First(x => x.playerTypeEnum == "PENITENTIARY");
            SetFactionToManager(piofo, "red", new List<Color32> { Color.red, Color.white, Color.black });
            ShipsInstanciation(piofo, new GameObject[1] { piofoShipPrefab });

            // Missytown
            Faction missytown = ServiceGame.Factions.First(x => x.playerTypeEnum == "PRISON");
            SetFactionToManager(missytown, "green", new List<Color32> { Color.green, Color.black, Color.gray });
            ShipsInstanciation(missytown, new GameObject[1] { missytownShipPrefab });

            // CPL
            Faction cpl = ServiceGame.Factions.First(x => x.playerTypeEnum == "PIRATE");
            SetFactionToManager(cpl, "black", new List<Color32> { Color.black, Color.red, Color.white });
            ShipsInstanciation(cpl, cplShipPrefabs);

            // CMR
            Faction cmr = ServiceGame.Factions.First(x => x.playerTypeEnum == "REBEL_SAILORS");
            SetFactionToManager(cmr, "magenta", new List<Color32> { Color.magenta, Color.black, Color.yellow });
            ShipsInstanciation(cmr, cmrShipPrefabs);

            // Competitor
            int count = 0;
            var competitors = ServiceGame.Factions.Where(x => x.playerTypeEnum == "COMPETITOR");
            List<string> colors = new List<string> { "lime", "navy", "olive" };
            foreach (Faction competitor in competitors)
            {
                SetFactionToManager(competitor, colors[count], new List<Color32> { ColorTools.NameToColor(colors[count]), Color.green, Color.gray });
                ShipsInstanciation(competitor, competitorShipPrefabs);
                count++;
            }

            // Ghost
            Faction ghost = ServiceGame.Factions.First(x => x.playerTypeEnum == "GHOST");
            SetFactionToManager(ghost, "gray", new List<Color32> { Color.gray, Color.red, Color.white });
            ShipsInstanciation(ghost, new GameObject[1] { ghostShipPrefab });
        }

        private void SetFactionToManager(Faction faction, string colorName, List<Color32> colors, bool isPlaying = true)
        {
            var fM = new FactionManager
            {
                Faction = faction,
                Colors = colors,
                MainColor = colorName,
                IsPlaying = isPlaying
            };
            fM.SetFactionFlag(fM.Colors);
            FactionsManager.Instance.Factions.Add(fM);
        }

        private void ShipsInstanciation(Faction faction, GameObject[] shipPrefabs)
        {
            var ships = ServiceGame.GetShipsFromFaction(faction);                  

            foreach (var ship in ships)
            {
                ShipInstanciation(shipPrefabs[0], ship);   
            }
        }

        private void ShipInstanciation(GameObject shipPrefab, Ship ship)
        {
            GameObject startSquare;
            SquareManagement square;
            ShipManager sM;
            GameObject currentShipObject;

            // TODO : retirer le fix des coordonnées nulles
            if (ship.coordinates == null)
                ship.coordinates = new Square(20, 20);

            square = GetPhysicalSquareFromSquare(ship.coordinates);
            //square = squares.First(x => x.coordinates.x == ship.coordinates.x && x.coordinates.y == ship.coordinates.y);
            startSquare = square.gameObject;
            currentShipObject = Instantiate(shipPrefab, startSquare.transform.position + new Vector3(0, -10f, 0), startSquare.transform.rotation);

            sM = currentShipObject.GetComponent<ShipManager>();
            // création du navire
            sM.ship = ship;
            // positionnement des coordonn�es du navire
            sM.ship.coordinates = square.coordinates;

            // TODO : fix sur le navire fantôme à mettre en place
            if (ServiceGame.GetFactionFromId(ship.owner).playerTypeEnum != "GHOST")
            {
                // apparition de l'équipage
                sM.AssignCrew();
                // apparition des drapeaux
                sM.AssignColors();
            }

            instanciedShipObjects.Add(currentShipObject);
        }

        public void ToggleSquares(bool active)
        {
            squaresShowed = active;
            squaresParent.SetActive(squaresShowed);
        }

        /// <summary>
        /// Commutateur d'�cran de navire
        /// </summary>
        /// <param name="active">doit on afficher ou cacher ?</param>
        /// <param name="ship">optionnel. si renseign�, il s'agit de la classe Ship (Colander)</param>
        public void ToggleShipScreen(bool active, Ship ship = null)
        {
            shipScreen.gameObject.SetActive(active);
            if (active)
            {
                var shipM = shipScreen.GetComponent<ShipScreenManager>();
                shipM.ship = ship;
                shipM.Show();
            }
        }

        public void ToggleHarborTooltip(bool active, Island island = null)
        {
            if (active)
            {
                var tooltipManager = harborToolTip.GetComponent<HarborTooltipManager>();
                tooltipManager.title.text = island.name;
                tooltipManager.owner.text = (string.IsNullOrEmpty(island.owner)) ? "Île non colonisée" : ServiceGame.GetFactionFromId(island.owner).name;
                tooltipManager.population.text = island.PopulationText;
            }

            //harborToolTip.transform.position = Input.mousePosition;
            harborToolTip.gameObject.SetActive(active);
        }

        #region Mode Navigation

        public void ToggleNavigationMode(bool active)
        {
            navigationMode = active;
        }

        public bool IsNavigationModeActive()
        {
            return navigationMode;
        }

        #endregion
    }
}
