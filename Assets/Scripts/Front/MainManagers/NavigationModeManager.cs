using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Front.Squares;
using Assets.Scripts.Model;
using StylizedWater2;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{
    /// <summary>
    /// Le mode navigation se compose de deux parties :
    ///  - décision du mouvement (intervention utilisateur)
    ///  - mouvement case à case
    /// </summary>
    public class NavigationModeManager : UnityEngine.MonoBehaviour
    {
        public GameObject dialog;
        public TextMeshProUGUI text;
        public Button valider;

        // gestion du mode navigation
        public int squaresRemaning;
        public int windDirection;
        private List<Square> recordedMovement;
        private bool isNavModeFinished;
        private ShipManager shipM;

        // gestion du mouvement
        private MovementStep activeMoveStep = MovementStep.Nope;
        //private SquareManagement shipSquare;
        private SquareManagement nextSquare;
        private int pendingRiggingSpent;

        [SerializeField]
        private float distanceToDestination;

        public static NavigationModeManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        // Start is called before the first frame update
        void Start()
        {         
            valider.onClick.AddListener(ValidateNavigation);            
        }

        void Update()
        {
            // mouvement du navire en fonction des cases enregistrées, dans l'ordre
            switch (activeMoveStep)
            {
                // début de la séquence
                case MovementStep.Start:
                    nextSquare = GameManager.Instance.GetPhysicalSquareFromSquare(recordedMovement.First());
                    activeMoveStep = MovementStep.HasToFaceNextSquare;
                    break;
                // le navire s'oriente pour faire face à la case suivante
                case MovementStep.HasToFaceNextSquare:
                    GameManager.Instance.GetActualPlayinghipObject.transform.Rotate(0, AngleToNextSquare(), 0);
                    //gm.GetPlayerShip().transform.LookAt(nextSquare.gameObject.transform);
                    activeMoveStep = MovementStep.HasToMove;
                    // TODO : réaliser une animation
                    break;
                // le navire va bouger
                case MovementStep.HasToMove:
                    shipM.Move(Vector3.forward);
                    activeMoveStep = MovementStep.MovingStarted;
                    break;
                // on vérifie la position du navire
                case MovementStep.MovingStarted:
                    distanceToDestination = Vector3.Distance(GameManager.Instance.GetActualPlayinghipObject.transform.position, (nextSquare.transform.position + (Vector3.down * 10)));
                    if (distanceToDestination > 1f)
                        activeMoveStep = MovementStep.HasToMove;
                    else
                        activeMoveStep = MovementStep.HasReachedNextSquare;
                    break;
                // on retire la position actuelle de la liste et on rafraîchit les coordonnées du navire
                case MovementStep.HasReachedNextSquare:
                    nextSquare.RAZNavigationMode();
                    shipM.ship.coordinates = nextSquare.coordinates;
                    recordedMovement.RemoveAt(0);
                    if (recordedMovement.Any())
                        activeMoveStep = MovementStep.Start;
                    else
                        activeMoveStep = MovementStep.End;
                    break;
                // fin de la séquence de mouvement
                case MovementStep.End:
                    dialog.SetActive(false);
                    activeMoveStep = MovementStep.Nope;
                    shipM.ship.shipBoard.rigging -= pendingRiggingSpent;
                    pendingRiggingSpent = 0;
                    TurnManager.Instance.MainState = TurnState.AI;
                    break;
                default:
                    break;
            }
        }

        public void StartNavigationMode(Ship ship)
        {
            shipM = GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>();
            recordedMovement = new List<Square> { ship.coordinates };
            isNavModeFinished = false;
            activeMoveStep = MovementStep.Nope;
            GameManager.Instance.ToggleSquares(true);
        }

        public IEnumerable<Square> NextPossibleSquare()
        {
            var currentSquare = recordedMovement.Last();
            return new List<Square>
            {
                { new Square(currentSquare.x - 1, currentSquare.y) },
                { new Square(currentSquare.x + 1, currentSquare.y) },
                { new Square(currentSquare.x, currentSquare.y-1) },
                { new Square(currentSquare.x, currentSquare.y+1) }
            }.Except(recordedMovement);
        }

        public void SquareActivation(bool active, Square wantedSquare)
        {
            if (active)
            {
                recordedMovement.Add(wantedSquare);
                squaresRemaning -= 1;
                pendingRiggingSpent += 1;
            }
            else
            {
                recordedMovement.Remove(wantedSquare);
                squaresRemaning += 1;
                pendingRiggingSpent -= 1;
            }

            UpdateMoveText();
        }

        public void UpdateMoveText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Mode Navigation");
            sb.AppendLine($"Gréément restant : {shipM.ship.shipBoard.rigging}");
            sb.AppendLine($"Gréément consommé : {pendingRiggingSpent}");
            sb.AppendLine($"Direction du vent : {GetWindDirection()}");
            sb.AppendLine(string.Empty);

            if (squaresRemaning > 0)
                sb.AppendLine($"Vous pouvez avancer de : {squaresRemaning} cases");
            else
                sb.AppendLine($"Vous ne pouvez plus avancer");

            sb.AppendLine(string.Empty);

            sb.AppendLine("Clic gauche  : sélectionner case suivante");
            sb.AppendLine("Clic central : retirer la case choisie");

            text.text = sb.ToString();
        }

        private string GetWindDirection()
        {
            switch (windDirection)
            {
                case 1:
                    return "Nord";
                case 2:
                    return "Est";
                case 3:
                    return "Sud";
                case 4:
                    return "Ouest";
                default:
                    return null;
            }
        }

        private void ValidateNavigation()
        {
            if (!isNavModeFinished)
            {
                // retour de la cam à la normale
                GameManager.Instance.camOffSet = new Vector3(0, 300, -160);
                GameManager.Instance.camEulerAngles = new Vector3(60, 0, 0);
                GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);

                // retrait de la position de base de la liste
                recordedMovement.RemoveAt(0);
                GameManager.Instance.ToggleNavigationMode(false);
                isNavModeFinished = true;
                GameManager.Instance.ToggleSquares(false);

                // démarrage du movement
                activeMoveStep = MovementStep.Start;
            }
        }

        private float AngleToNextSquare()
        {
            if (nextSquare.coordinates.x > shipM.ship.coordinates.x)
                return 90f - GameManager.Instance.GetActualPlayinghipObject.transform.rotation.eulerAngles.y;
            else if (nextSquare.coordinates.x < shipM.ship.coordinates.x)
                return -90f - GameManager.Instance.GetActualPlayinghipObject.transform.rotation.eulerAngles.y;
            else if (nextSquare.coordinates.y > shipM.ship.coordinates.y)
                return 0f - GameManager.Instance.GetActualPlayinghipObject.transform.rotation.eulerAngles.y;
            else
                return 180f - GameManager.Instance.GetActualPlayinghipObject.transform.rotation.eulerAngles.y;
        }
    }

    enum MovementStep
    {
        Nope,
        Start,
        HasToFaceNextSquare,
        HasToMove,
        MovingStarted,
        HasReachedNextSquare,
        End
    }
}