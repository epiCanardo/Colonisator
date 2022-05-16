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
                // d�marrage du tour
                ServiceGame.StartNewTurn();

                MainState = TurnState.ActionsStarted;
                GameManager.Instance.ToggleSquares(false);
                CurrentTurnText.text = $"Tour {ServiceGame.GetCurrentTurn.number} : D�but du tour";

                // pour chaque tour de faction, on d�roule les actions
                foreach (var factionTurn in ServiceGame.GetCurrentTurn.factionsAndShips)
                {
                    Faction faction = ServiceGame.GetFactionFromId(factionTurn.Key);

                    // si la faction ne joue pas, �a d�gage
                    var play = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).IsPlaying;
                    if (!play)
                        continue;

                    // si c'est au joueur humain de jouer, on laisse la main. La fonction reprendra lorsque MainState sera AI
                    if (faction.playerTypeEnum == "HUMAN")
                    {
                        //if (nonHumanAutoTestActive)
                          //  continue;

                        GameManager.Instance.ToggleCamMovement(true);
                        MainState = TurnState.Human;
                        // positionnement de la cam�ra derri�re le navire en cours
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

                        // on est sur les navires IA, la camra libre est d�sactiv�e
                        MainState = TurnState.AI;
                        GameManager.Instance.ToggleCamMovement(false);

                        // application des actions pr�vues pour chaque navire de la faction
                        foreach (var action in factionTurn.Value)
                        {
                            // positionnement de la cam�ra derri�re le navire en cours
                            GameManager.Instance.CurrentShipToPlay = ServiceGame.GetShip(action.id);

                            // mise � jour du navire
                            GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().ship =
                                GameManager.Instance.CurrentShipToPlay;

                            GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);
                            CurrentTurnText.text =
                                $"Tour {ServiceGame.GetCurrentTurn.number} : Tour de la faction : {faction.name} - Navire : {GameManager.Instance.CurrentShipToPlay.name}";

                            yield return new WaitForSeconds(0.1f);

                            // si c'est un navire IA, il effectue les actions pr�vues

                            // gestion du d�placement
                            if (action.move != null)
                            {
                                var movement = ServiceGame.PrepareShipMovement(action);
                                foreach (Square square in movement)
                                {
                                    // case physique d'arriv�e et mouvement
                                    var physicalSquare = GameManager.Instance.GetPhysicalSquareFromSquare(square);
                                    // orientation par rapport � la cible
                                    yield return GameManager.Instance.GetActualPlayinghipObject.transform
                                        .DOLookAt(physicalSquare.transform.position, 1f).WaitForCompletion();
                                    // d�placement
                                    GameManager.Instance.GetActualPlayinghipObject.transform.DOMove(
                                        physicalSquare.transform.position + (Vector3.down * 10), 1);
                                    // d�placement de la cam�ra � la m�me vitesse
                                    yield return Camera.main.transform
                                        .DOMove(physicalSquare.transform.position + GameManager.Instance.camOffSet, 1)
                                        .WaitForCompletion();
                                }

                                // application des effets sur le gr��ment
                                GameManager.Instance.CurrentShipToPlay.shipBoard.rigging -= action.move.cost;

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

                                    if (action.move.cost > 0)
                                        HistoricsManager.Instance.NewMessage($"[Tour {ServiceGame.GetCurrentTurn.number }] - [Faction : {faction.name}] - Le navire '{GameManager.Instance.CurrentShipToPlay.name}' " +
                                                                             $"a boug� de {action.move.moveDetails.Sum(x=>x.Value)} cases et a perdu {action.move.cost} de gr��ment suite � son mouvement ! " +
                                                                             $"Reste : {GameManager.Instance.CurrentShipToPlay.shipBoard.rigging}");
                                }
                            }

                            // gestion de la colonisation
                            if (action.realisation == "COLONIZE")
                            {
                                Island island = ServiceGame.GetIsland(GameManager.Instance.CurrentShipToPlay
                                    .coordinates);
                                ColonisationDTO dtoColonisation = new ColonisationDTO
                                {
                                    ship = GameManager.Instance.CurrentShipToPlay,
                                    island = island,
                                    food = -15,
                                    // on prend 15 matelots au hasard
                                    npcs = ServiceGame.ShipSailors(GameManager.Instance.CurrentShipToPlay).Take(15)
                                        .ToList(),
                                    order = 15
                                };

                                // TODO : colonisation type avec 15 vivres, 15 matelots et 15 d'ordre en plus
                                ServiceGame.ColonizeIsland(dtoColonisation);

                                HistoricsManager.Instance.NewMessage($"[Tour {ServiceGame.GetCurrentTurn.number }] - [Faction : {faction.name}] - " +
                                                                     $"Le navire '{GameManager.Instance.CurrentShipToPlay.name}' " +
                                                                     $"a install� une nouvelle colonie sur {island.name} ! " +
                                                                     $"{dtoColonisation.npcs.Count} matelots ont �t� d�barqu�s." +
                                                                     $"Reste : {GameManager.Instance.CurrentShipToPlay.crew.Count}");
                            }

                            if (action.solution == "PUNCTURE_CREW" && action.realisation == "GET_SAILORS")
                            {
                                var sailors = ServiceGame.ShipSailors(GameManager.Instance.CurrentShipToPlay);

                                if (action.puncture != null && action.puncture.npcs.Any())
                                {
                                    PunctureDTO punctureDto = new PunctureDTO
                                    {
                                        npcIds = action.puncture.npcs,
                                        // on prends le navire du premier npc car ils sont tous du m�me navire
                                        sourceShipId = ServiceGame.GetNpc(action.puncture.npcs[0]).currentShip,
                                        targetShipId = action.id
                                    };
                                    ServiceGame.Puncture(punctureDto);

                                    HistoricsManager.Instance.NewMessage(
                                        $"[Tour {ServiceGame.GetCurrentTurn.number}] - [Faction : {faction.name}] - " +
                                        $"Le navire fant�me s'est renforc� avec {punctureDto.npcIds.Count} pr�lev�s !' " +
                                        $"Il para�t qu'ils n'ont pas souffert, mais les cris entendus � bord indiquent le contraire :|");
                                }
                            }

                            if (faction.playerTypeEnum == "REBEL_SAILORS")
                            {
                                var test = action.id;
                            }
                        }
                    }
                }

                // � la fin du tour, on revient par d�faut sur le navire du joueur
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

                // on g�n�re un rapport de fin de tour
                var dto = ServiceGame.GetReport();

                // fin du tour : envoi du rapport au back
                yield return StartCoroutine("EndTurn");
            }
        }

        IEnumerator EndTurn()
        {
            ServiceGame.EndTurn();
            yield return null;

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