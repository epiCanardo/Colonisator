using ColanderSource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Affichage du tour de jeu")]
        public TextMeshProUGUI CurrentTurnText;

        [Header("Bouton de fin de tour")]
        public GameObject BackgroundColor;
        public GameObject Button;

        public static TurnManager Instance { get; private set; }

        private bool nonHumanAutoTestActive = true;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public TurnState MainState { get; set; }

        public void BounceButton()
        {
            Button.GetComponent<RawImage>().transform.DOScale(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopBouncing()
        {
            Button.GetComponent<RawImage>().transform.DOKill();
            Button.GetComponent<RawImage>().transform.localScale = Vector3.one;
        }

        public IEnumerator StartTurn()
        {
            while (ServiceGame.GetCurrentTurn.number < 1000)
            {
                // démarrage du tour
                ServiceGame.StartNewTurn();

                MainState = TurnState.ActionsStarted;
                GameManager.Instance.ToggleSquares(false);
                CurrentTurnText.text = $"Tour {ServiceGame.GetCurrentTurn.number} : Début du tour";

                // on commence par le joueur humain
                // la fonction reprendra son exécution lorsque le statut de tour sera placé à AI
                //GameManager.Instance.CurrentShipToPlay = ServiceGame.GetHumanShip("Joueur Humain 1");
                //MainState = TurnState.Human;
                //yield return new WaitUntil(() => MainState == TurnState.AI);

                // pour chaque tour de faction, on déroule les actions
                foreach (var factionTurn in ServiceGame.GetCurrentTurn.factionsAndShips)
                {
                    Faction faction = ServiceGame.GetFactionFromId(factionTurn.Key);

                    // si c'est au joueur humain de jouer, on laisse la main. La fonction reprendra lorsque MainState sera AI
                    if (faction.playerTypeEnum == "HUMAN")
                    {
                        // TODO : le mouvement du joueur est igoré à des fins de tests
                        if (nonHumanAutoTestActive)
                            continue;

                        GameManager.Instance.ToggleCamMovement(true);
                        MainState = TurnState.Human;
                        // positionnement de la caméra derrière le navire en cours
                        GameManager.Instance.CurrentShipToPlay = ServiceGame.GetHumanShip("Joueur Humain 1");
                        GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);
                        CurrentTurnText.text =
                            $"Tour {ServiceGame.GetCurrentTurn.number} : Tour de la faction : {faction.name} - Navire : {GameManager.Instance.CurrentShipToPlay.name}";

                        yield return new WaitUntil(() => MainState == TurnState.AI);
                    }
                    else
                    {
                        // on laisse 1 seconde entre les navires
                        yield return new WaitForSeconds(1f);

                        // on est sur les navires IA, la camra libre est désactivée
                        MainState = TurnState.AI;
                        GameManager.Instance.ToggleCamMovement(false);

                        // application des actions prévues pour chaque navire de la faction
                        foreach (var action in factionTurn.Value)
                        {
                            // positionnement de la caméra derrière le navire en cours
                            GameManager.Instance.CurrentShipToPlay = ServiceGame.GetShip(action.id);
                            GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);
                            CurrentTurnText.text =
                                $"Tour {ServiceGame.GetCurrentTurn.number} : Tour de la faction : {faction.name} - Navire : {GameManager.Instance.CurrentShipToPlay.name}";

                            yield return new WaitForSeconds(0.1f);

                            // si c'est un navire IA, il effectue les actions prévues

                            // gestion du déplacement
                            if (action.move != null)
                            {
                                var movement = ServiceGame.PrepareShipMovement(action);
                                foreach (Square square in movement)
                                {
                                    // case physique d'arrivée et mouvement
                                    var physicalSquare = GameManager.Instance.GetPhysicalSquareFromSquare(square);
                                    // orientation par rapport à la cible
                                    yield return GameManager.Instance.GetActualPlayinghipObject.transform
                                        .DOLookAt(physicalSquare.transform.position, 1f).WaitForCompletion();
                                    // déplacement
                                    GameManager.Instance.GetActualPlayinghipObject.transform.DOMove(
                                        physicalSquare.transform.position + (Vector3.down * 10), 1);
                                    // déplacement de la caméra à la même vitesse
                                    yield return Camera.main.transform
                                        .DOMove(physicalSquare.transform.position + GameManager.Instance.camOffSet, 1)
                                        .WaitForCompletion();

                                    // application des effets sur le gréément
                                    GameManager.Instance.CurrentShipToPlay.shipBoard.rigging -= action.move.cost;
                                }

                                // s'il y a eu mouvement, on enregistre le mouvement
                                if (movement.Any())
                                {
                                    ServiceGame.ApplyShipMovement(GameManager.Instance.CurrentShipToPlay,
                                        movement.Last());
                                    ServiceGame.RegisterMovement(new MoveDTO
                                    {
                                        move = (action.move != null)
                                            ? new Move {cost = action.move.cost, moveDetails = action.move.moveDetails}
                                            : null,
                                        ship = GameManager.Instance.CurrentShipToPlay
                                    });
                                }
                            }

                            // gestion de la colonisation
                            if (action.realisation == "COLONIZE")
                            {
                                // TODO : colonisation type avec 15 vivres, 15 matelots et 15 d'ordre en plus
                                ServiceGame.ColonizeIsland(
                                    new ColonisationDTO
                                    {
                                        ship = GameManager.Instance.CurrentShipToPlay,
                                        island = ServiceGame.GetIsland(GameManager.Instance.CurrentShipToPlay
                                            .coordinates),
                                        food = -15,
                                        // on prend 15 matelots au hasard
                                        npcs = ServiceGame.ShipSailors(GameManager.Instance.CurrentShipToPlay).Take(15)
                                            .ToList(),
                                        order = 15
                                    });
                            }

                            // TODO : activer le navire fantômes
                            if (action.solution == "PUNCTURE_CREW" && action.realisation == "GET_SAILORS")
                            {
                                //continue;

                                if (action.puncture != null)
                                    ServiceGame.Puncture(new PunctureDTO
                                    {
                                        npcIds = action.puncture.npcs,
                                        sourceShipId = ServiceGame.GetNpc(action.puncture.npcs[0]).currentShip,
                                        targetShipId = action.id
                                    });
                            }

                            if (faction.playerTypeEnum == "REBEL_SAILORS")
                            {
                                var test = action.id;
                            }
                        }
                    }
                }

                // à la fin du tour, on revient par défaut sur le navire du joueur
                var color = BackgroundColor.GetComponent<Image>().color;
                var targetColor = new Color(color.r, color.g, color.b, 0);
                yield return BackgroundColor.GetComponent<Image>().DOColor(targetColor, 0.5f).WaitForCompletion();

                // gestion de la fin du tour
                if (nonHumanAutoTestActive)
                    MainState = TurnState.ActionsFinished;
                else
                {
                    // on se place en attente de fin de tour
                    MainState = TurnState.WaitForEndTurn;
                    BackgroundColor.GetComponent<Image>().DOColor(Color.red, 1).SetEase(Ease.InBounce);
                    yield return new WaitUntil(() => MainState == TurnState.ActionsFinished);
                }

                CurrentTurnText.text = $"Tour {ServiceGame.GetCurrentTurn.number} : Fin du tour";
                targetColor = new Color(color.r, color.g, color.b, 0.8f);
                BackgroundColor.GetComponent<Image>().DOColor(targetColor, 0.5f);

                CurrentTurnText.text = $"Nouveau tour dans 1 seconde...";
                yield return new WaitForSeconds(1f);

                GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetPlayingHumanShipObject);
                GameManager.Instance.ToggleCamMovement(true);

                // on génère un rapport de fin de tour
                var dto = ServiceGame.GetReport();

                // fin du tour : envoi du rapport au back
                ServiceGame.EndTurn();
            }
        }
    }

    public enum TurnState
    {
        ActionsStarted,
        Human,
        AI,
        WaitForEndTurn,
        ActionsFinished,
    }
}