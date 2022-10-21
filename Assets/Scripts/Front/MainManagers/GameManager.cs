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
using DG.Tweening;

namespace Assets.Scripts.Front.MainManagers
{
    public class GameManager : UnityEngine.MonoBehaviour
    {
        public Canvas canvas;

        [Header("Token Borders")]
        public Image CUIITokenBorder;

        [Header("Gestion des cases")]
        public GameObject squarePrefab;
        public GameObject nonNavigableSquarePrefab;
        public GameObject harborSquarePrefab;
        public GameObject ile3HarborSqurePrefab;
        public GameObject sundercityHarborSqurePrefab;
        public GameObject ileNeutre1HarborSqurePrefab;
        public GameObject squaresParent;

        [SerializeField]
        private List<GameObject> islands;

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
        [SerializeField] private GameObject mapViewPlane;
        [SerializeField] private Material islandBordersMaterial;

        public TextMeshProUGUI coordinatesTextObject;

        // l'instance du navire du joueur
        private List<GameObject> instanciedShipObjects = new List<GameObject>();

        private float zSquaresStart = 817.1f;
        private float xSquaresStart = -2000f;
        private float zSize = 5000f;
        private float xSize = 5000f;
        private float zSquares = 100f;
        private float xSquares = 100f;
        private bool squaresShowed;
        private List<SquareManagement> squares = new List<SquareManagement>();

        // mode navigation
        private bool navigationMode;

        public Ship CurrentShipToPlay { get; set; }

        public Npc GetPlayerCharacter
        {
            get
            {
                Ship humanShip = ServiceGame.GetHumanShip;
                if (humanShip != null)
                    return ServiceGame.ShipCaptain(humanShip);
                return null;
            }
        }
       
        // récupère l'objet instancié correspondant au navire en cours
        public GameObject GetActualPlayinghipObject => instanciedShipObjects.First(x => x.GetComponent<ShipManager>().ship.Equals(CurrentShipToPlay));

        public GameObject GetPlayingHumanShipObject => instanciedShipObjects.First(x => x.GetComponent<ShipManager>().ship.Equals(ServiceGame.GetHumanShip));

        public void FocusCamOnShip(GameObject ship)
        {
            // postionnement de la caméra par rapport au navire actuel
            ToggleCamMovement(false);
            Camera.main.transform.position = ship.transform.position + Vector3.back * 100;
            Camera.main.GetComponent<CamMovement>().SetCamToActionLevel();

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
            // définition de la qualité globale
            QualitySettings.SetQualityLevel(ModManager.Instance.GetGlobalQuality());

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

            //miniMap.texture = ModManager.Instance.GenerateMinimapColors();
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
        }

        public void CreateSquares()
        {
            CreateSquaresTask();           
        }

        void CreateSquaresTask()
        {
            // création des cases
            List<Square> harbours = ServiceGame.Islands.Select(x => x.harbourCoordinates).ToList();

            Square basicSquare;
            Vector3 physicalSquare;

            var zstep = (zSize / zSquares);
            var xstep = (xSize / xSquares);

            for (int z = 0; z < 100; z++)
            {
                for (int x = 0; x < 100; x++)
                {
                    physicalSquare = new Vector3(xSquaresStart + (xstep / 2) + x * xstep, 10, zSquaresStart + (zstep / 2) + z * zstep);
                    basicSquare = new Square(x + 1, z + 1);
                    GameObject objectCreated = null;
                    SquareManagement squareManager = null;

                    if (harbours.Contains(basicSquare))
                    {
                        // on y va en dur comme des sales, c'est ça qu'on veut voir !
                        if (x + 1 == 47 && z + 1 == 90) CreateIsland(ile3HarborSqurePrefab, physicalSquare, basicSquare, "ile3");
                        else if (x + 1 == 5 && z + 1 == 96) CreateIsland(sundercityHarborSqurePrefab, physicalSquare, basicSquare, "sundercity");
                        else if (x + 1 == 27 && z + 1 == 69) CreateIsland(ileNeutre1HarborSqurePrefab, physicalSquare, basicSquare, "ileNeutre1");
                        else                        
                            CreateIsland(harborSquarePrefab, physicalSquare, basicSquare, "balek");
                        
                    }
                    else
                    {
                        if (ModManager.Instance.IsSquareNonNavigable(basicSquare))
                        {
                            objectCreated = Instantiate(nonNavigableSquarePrefab, physicalSquare, nonNavigableSquarePrefab.transform.rotation, squaresParent.transform);
                            squareManager = objectCreated.GetComponent<NonNavigableSquareManagement>();
                        }
                        else
                        {
                            objectCreated = Instantiate(squarePrefab, physicalSquare, squarePrefab.transform.rotation, squaresParent.transform);
                            squareManager = objectCreated.GetComponent<NavigableSquareManagement>();
                        }

                        objectCreated.name = $"Square{basicSquare.x}_{basicSquare.y}";
                        squareManager.coordinates = basicSquare;
                        squareManager.SetDebugText(""/*basicSquare.ToString()*/);
                        squares.Add(squareManager);
                        objectCreated.SetActive(false);
                    }                    
                }
            }            
        }

        private void DrawIslandsBorders()
        {
            foreach (var harbor in squares.OfType<HarborSquareManagement>())
            {
                List<SquareManagement> costalSquares = GetIslandCostalSquares(harbor);
                if (costalSquares.Any())
                {
                    // dessin des contours des îles (pour la minimap)
                    var gameObjectDummy = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    var instanceDummy = Instantiate(gameObjectDummy, mapViewPlane.transform);
                    instanceDummy.layer = mapViewPlane.layer;

                    LineRenderer line = instanceDummy.AddComponent<LineRenderer>();
                    line.material = new Material(islandBordersMaterial);                    
                    line.material.color = harbor.GetOwnerColor();
                    line.material.SetColor("_EmissionColor", harbor.GetOwnerColor());
                    line.name = "islandBorders";
                    line.widthMultiplier = 2f;
                    line.positionCount = costalSquares.Count;

                    var points = new Vector3[costalSquares.Count];
                    for (int i = 0; i < costalSquares.Count; i++)
                    {
                        points[i] = costalSquares[i].transform.position;
                    }
                    line.SetPositions(points);
                }
            }
        }

        private void CreateIsland(GameObject harborPrefab, Vector3 square, Square basicSquare, string name)
        {
            var objectCreated = Instantiate(harborPrefab, square, harborPrefab.transform.rotation, squaresParent.transform);
            var harbor = objectCreated.GetComponent<HarborSquareManagement>();
            harbor.coordinates = basicSquare;
            harbor.SetDebugText(""/*basicSquare.ToString()*/);
            harbor.SetIsland();
            squares.Add(harbor);

            objectCreated.SetActive(true);
        }

        private List<SquareManagement> GetIslandCostalSquares(HarborSquareManagement harbor)
        {
            List<SquareManagement> costalSquares = new List<SquareManagement>();

            var nonNavagableSquaresCoordinates = ModManager.Instance.GetIslandCostalSquares(harbor.Island.harbourCoordinates);
            foreach (var square in nonNavagableSquaresCoordinates)
            {
                SquareManagement squareM = GetPhysicalSquareFromSquare(square);
                costalSquares.Add(squareM);
            }

            return costalSquares;
        }

        private List<Square> GetFourNextNonNavigableSquares(Square square)
        {
            List<Square> squares = new List<Square>();

            NonNavigableSquareDetection(squares, square + Square.East);
            NonNavigableSquareDetection(squares, square + Square.West);
            NonNavigableSquareDetection(squares, square + Square.North);
            NonNavigableSquareDetection(squares, square + Square.South);

            return squares;

            static void NonNavigableSquareDetection(List<Square> squares, Square candidateSquare)
            {
                if (ModManager.Instance.IsSquareNonNavigable(candidateSquare))
                    squares.Add(candidateSquare);
            }
        }

        private bool IsCostalSquare(Square square)
        {
            return GetFourNextNonNavigableSquares(square).Count < 4;
        }

        /// <summary>
        /// Donne l'objet de case plateau � partir des coordon�es
        /// </summary>
        /// <param name="square"></param>
        /// <returns></returns>
        public SquareManagement GetPhysicalSquareFromSquare(Square square)
        {
            return squares.FirstOrDefault(x => x.coordinates.Equals(square));
        }

        public void PlayersSpawn()
        {
            // Joueur
            Faction human = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "HUMAN");
            if (human != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/defaultHuman");
                SetFactionToManager(human, "blue", new List<Color32> { Color.blue, Color.white, Color.red }, flag.texture);
                ShipsInstanciation(human, new GameObject[1] { mainShipPrefab });
            }

            // Competitor
            int count = 0;
            var competitors = ServiceGame.Factions.Where(x => x.playerTypeEnum == "COMPETITOR");
            List<string> colors = new List<string> { "yellow", "green", "red" };
            foreach (Faction competitor in competitors)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/competitor_{colors[count]}");
                SetFactionToManager(competitor, colors[count], new List<Color32> { ColorTools.NameToColor(colors[count]), Color.green, Color.gray }, flag.texture);
                ShipsInstanciation(competitor, competitorShipPrefabs);
                count++;
            }

            // CUII
            Faction cuii = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "NEUTRAL");
            if (cuii != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/cuii");
                SetFactionToManager(cuii, "gold", new List<Color32> { ColorTools.NameToColor("gold"), Color.black, Color.white }, flag.texture);
                ShipsInstanciation(cuii, cuiiShipPrefabs);
            }

            // Sundercity
            Faction sundercity = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "TOWN");
            if (sundercity != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/sundercity");
                SetFactionToManager(sundercity, "teal", new List<Color32> { ColorTools.NameToColor("teal"), Color.black, Color.white }, flag.texture);
                ShipsInstanciation(sundercity, new GameObject[1] { sundercityShipPrefab });
            }

            // Piofo
            Faction piofo = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "PENITENTIARY");
            if (piofo != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/piofo");
                SetFactionToManager(piofo, "white", new List<Color32> { Color.white, Color.white, Color.black }, flag.texture);
                ShipsInstanciation(piofo, new GameObject[1] { piofoShipPrefab });
            }

            // Missytown
            Faction missytown = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "PRISON");
            if (missytown != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/missytown");
                SetFactionToManager(missytown, "purple", new List<Color32> { ColorTools.NameToColor("purple"), Color.black, Color.gray }, flag.texture);
                ShipsInstanciation(missytown, new GameObject[1] { missytownShipPrefab });
            }

            // CPL
            Faction cpl = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "PIRATE");
            if (cpl != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/cpl");
                SetFactionToManager(cpl, "black", new List<Color32> { Color.black, Color.red, Color.white }, flag.texture);
                ShipsInstanciation(cpl, cplShipPrefabs);
            }

            // CMR
            Faction cmr = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "REBEL_SAILORS");
            if (cmr != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/cmr");
                SetFactionToManager(cmr, "maroon", new List<Color32> { ColorTools.NameToColor("maroon"), Color.black, Color.yellow }, flag.texture);
                ShipsInstanciation(cmr, cmrShipPrefabs);
            }

            // Ghost
            Faction ghost = ServiceGame.Factions.FirstOrDefault(x => x.playerTypeEnum == "GHOST");
            if (ghost != null)
            {
                Sprite flag = Resources.Load<Sprite>($"Textures/Icons/Flags/ghost");
                SetFactionToManager(ghost, "cyan", new List<Color32> { Color.cyan, Color.red, Color.white }, flag.texture);
                ShipsInstanciation(ghost, new GameObject[1] { ghostShipPrefab });
            }

            foreach (HarborSquareManagement harborSquare in squares.OfType<HarborSquareManagement>())
            {
                harborSquare.SetHarborVisualColors();
            }

            DrawIslandsBorders();
        }

        private void SetFactionToManager(Faction faction, string colorName, List<Color32> colors, Texture2D flag, bool isPlaying = true)
        {
            var fM = new FactionManager
            {
                Faction = faction,
                Colors = colors,
                MainColor = colorName,
                IsPlaying = isPlaying,
                Flag = flag
            };
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

            sM = currentShipObject.transform.GetComponent<ShipManager>();
            // création du navire
            sM.ship = ship;
            // positionnement des coordonn�es du navire
            sM.ship.coordinates = square.coordinates;

            // TODO : fix sur le navire fantôme à mettre en place
            //if (ServiceGame.GetFactionFromId(ship.owner).playerTypeEnum != "GHOST")
            //{
                // apparition de l'équipage
                sM.AssignCrew();
                // apparition des drapeaux
                sM.AssignColors();
            //}

            instanciedShipObjects.Add(currentShipObject);
        }

        public void ToggleSquares(bool active)
        {
            squaresShowed = active;
            //squaresParent.SetActive(squaresShowed);

            foreach (var square in squaresParent.GetComponentsInChildren<NavigableSquareManagement>())
            {
                square.gameObject.SetActive(active);
            }
        }

        private List<SquareManagement> previousSquares = new List<SquareManagement>();

        public void ShowSquaresWhereMovementIsPossible(Square shipPosition, int squaresRemaning, int windDirection)
        {
            // recherche de la case phyisque correpondant à la case du navire
            SquareManagement startSquare = GetPhysicalSquareFromSquare(shipPosition);

            for (int i = 1; i <= squaresRemaning; i++)
            {
                var square = GetPhysicalSquareFromSquare(shipPosition + Square.East * i);
                ActivateSquare(square);
                square = GetPhysicalSquareFromSquare(shipPosition + Square.West * i);
                ActivateSquare(square);
                square = GetPhysicalSquareFromSquare(shipPosition + Square.North * i);
                ActivateSquare(square);
                square = GetPhysicalSquareFromSquare(shipPosition + Square.South * i);
                ActivateSquare(square);
            }

            void ActivateSquare(SquareManagement square)
            {
                if (square != null)
                {
                    square.gameObject.SetActive(true);
                    previousSquares.Add(square);
                }
            }
        }  
        
        public void HidePreviousNavMode()
        {
            foreach (SquareManagement squareM in previousSquares)
            {
                if (squareM is NavigableSquareManagement)
                    squareM.gameObject.SetActive(false);
            }
            previousSquares.Clear();
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

        public void BlinkTokenBorder()
        {
            CUIITokenBorder.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopBlinkTokenBorder()
        {
            CUIITokenBorder.DOKill(true);
            CUIITokenBorder.color = new Color(CUIITokenBorder.color.r, CUIITokenBorder.color.g, CUIITokenBorder.color.b, 1f);
        }

        #region Mode Navigation

        public void ToggleNavigationMode(bool active)
        {
            navigationMode = active;
        }

        public bool IsNavigationModeActive => navigationMode;

        #endregion
    }
}