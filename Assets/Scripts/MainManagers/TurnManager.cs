using ColanderSource;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;
using System.Text;

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
            for (int i = 0; i < 1000; i++)
            {
                //while (ServiceGame.GetCurrentTurn.number < 1000)
            //{
                // démarrage du tour
                yield return StartCoroutine("NewTurn");

                MainState = TurnState.ActionsStarted;
                GameManager.Instance.ToggleSquares(false);
                CurrentTurnText.text = $"Tour {ServiceGame.GetCurrentTurn.number} : Début du tour";

                // pour chaque tour de faction, on déroule les actions
                foreach (var factionTurn in ServiceGame.GetCurrentTurn.factionsAndShips)
                {
                    Faction faction = ServiceGame.GetFactionFromId(factionTurn.Key);

                    // si la faction ne joue pas, ça dégage
                    var play = FactionsManager.Instance.Factions.First(x => x.Faction.Equals(faction)).IsPlaying;
                    if (!play)
                        continue;

                    // si c'est au joueur humain de jouer, on laisse la main. La fonction reprendra lorsque MainState sera AI
                    if (faction.playerTypeEnum == "HUMAN")
                    {
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
                            GameManager.Instance.CurrentShipToPlay = ServiceGame.GetShip(action.id);
                            ShipManager shipManager =
                                GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>();

                            // mise à jour du navire
                            shipManager.ship = GameManager.Instance.CurrentShipToPlay;
                            // positionnement de la caméra derrière le navire en cours
                            GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);

                            // mise à jour du texte du tour en cours
                            CurrentTurnText.text =
                                $"Tour {ServiceGame.GetCurrentTurn.number} : Tour de la faction : {faction.name} - Navire : {shipManager.ship}";

                            yield return new WaitForSeconds(0.1f);

                            // si c'est un navire IA, il effectue les actions prévues

                            // affichage de l'action prévue
                            StringBuilder sb = new StringBuilder();

                            // travail sur les objectifs
                            switch (action.objectiveRuleResult?.objectiveEnum)
                            {
                                case "COLONIZE_ISLAND":
                                    sb.AppendLine("Je souhaite coloniser une île.");
                                    break;
                                case "PUNCTURE_CREW":
                                    sb.AppendLine("Je vais faucher des âmes..");
                                    break;
                                case "REFOURGUER_CREW":
                                    sb.AppendLine("Je souhaite me débarasser de certains matelots !!");
                                    break;
                                case "GET_RIGGING":
                                    sb.AppendLine("Je souhaite acheter du gréément");
                                    break;
                                default:
                                    sb.AppendLine("Je n'ai plus aucun objectif !");
                                    break;
                            }

                            // travail sur les solutions
                            switch (action.solutionRuleResult?.solutionEnum)
                            {
                                case "REFOURGUER_CREW":
                                    List<Npc> landingNpcs = ServiceGame.GetNpcs(action.realisationRuleResult?.npcs)
                                        .ToList();
                                    TradeDTO trade = new TradeDTO
                                    {
                                        ship = shipManager.ship,
                                        island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                        landingNpcs = landingNpcs,
                                    };
                                    ServiceGame.Trade(trade);
                                    sb.AppendLine($"{landingNpcs.Count} ont été débarqués.");
                                    break;
                                case "GO_TO_ISLAND":
                                    if (action.solutionRuleResult?.solutionEnum == "GO_TO_ISLAND")
                                        sb.AppendLine(
                                            $"Je me dirige vers : {ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId).name}");
                                    break;
                                case "BUY":
                                    //  -> 1000 dodris forfaitaires quel que soit la quantité
                                    //  -> quantité : pour atteindre les 100
                                    TradeDTO buyTrade = new TradeDTO
                                    {
                                        ship = GameManager.Instance.CurrentShipToPlay,
                                        island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                        buys = new List<TradeLine>
                                        {
                                            {
                                                new TradeLine
                                                {
                                                    ressource = "rigging",
                                                    quantity = 100 - shipManager.ship.shipBoard.rigging, cost = 1000
                                                }
                                            }
                                        },
                                        deltaStuff = new ShipBoard
                                        {
                                            rigging = 100 - shipManager.ship.shipBoard.rigging,
                                            dodris = -1000
                                        }
                                    };
                                    ServiceGame.Trade(buyTrade);
                                    sb.AppendLine(
                                        $"Je viens d'acheter {buyTrade.deltaStuff.rigging} de gréément, contre " +
                                        $"{-buyTrade.deltaStuff.dodris} dodris.");
                                    break;
                                case "COLONIZE":
                                    Island island = ServiceGame.GetIsland(GameManager.Instance.CurrentShipToPlay
                                        .coordinates);
                                    // TODO : colonisation type avec 15 vivres, 15 matelots et gain de 15 d'ordre
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

                                    ServiceGame.ColonizeIsland(dtoColonisation);

                                    HistoricsManager.Instance.NewMessage(
                                        $"[Tour {ServiceGame.GetCurrentTurn.number}] - [Faction : {faction.name}] - " +
                                        $"Le navire '{GameManager.Instance.CurrentShipToPlay.name}' " +
                                        $"a installé une nouvelle colonie sur {island.name} ! " +
                                        $"{dtoColonisation.npcs.Count} matelots ont été débarqués." +
                                        $"Reste : {GameManager.Instance.CurrentShipToPlay.crew.Count}");
                                    sb.AppendLine($"Je viens d'établir une colonie sur {island.name} !");
                                    break;
                                case "PUNCTURE_CREW":
                                    sb.AppendLine(
                                        $"{action.realisationRuleResult.npcs.Count}, ça fera l'affaire pour cette fois !");
                                    var sailors = ServiceGame.ShipSailors(GameManager.Instance.CurrentShipToPlay);

                                    List<string> npcsToPunct = action.realisationRuleResult.npcs;
                                    if (npcsToPunct != null && npcsToPunct.Any())
                                    {
                                        PunctureDTO punctureDto = new PunctureDTO
                                        {
                                            npcIds = npcsToPunct.ToList(),
                                            // on prend le navire du premier npc car ils sont tous du même navire
                                            sourceShipId = ServiceGame.GetNpc(npcsToPunct[0]).currentShip,
                                            targetShipId = action.id
                                        };
                                        ServiceGame.Puncture(punctureDto);

                                        HistoricsManager.Instance.NewMessage(
                                            $"[Tour {ServiceGame.GetCurrentTurn.number}] - [Faction : {faction.name}] - " +
                                            $"Le navire fantôme s'est renforcé avec {punctureDto.npcIds.Count} prélevés !' " +
                                            $"Il paraît qu'ils n'ont pas souffert, mais les cris entendus à bord indiquent le contraire :|");
                                    }

                                    break;
                                default:
                                    break;
                            }

                            // travail sur les réalisations
                            switch (action.realisationRuleResult?.realisationEnum)
                            {
                                case "MOVE":
                                    // gestion du déplacement
                                    Move actualMovement = action.realisationRuleResult?.move;

                                    List<Square> movement = ServiceGame.PrepareShipMovement(action);
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
                                            .DOMove(physicalSquare.transform.position + GameManager.Instance.camOffSet,
                                                1)
                                            .WaitForCompletion();
                                    }

                                    // application des effets sur le gréément
                                    GameManager.Instance.CurrentShipToPlay.shipBoard.rigging -= actualMovement.cost;

                                    // s'il y a eu mouvement, on enregistre le mouvement
                                    if (movement.Any())
                                    {
                                        ServiceGame.ApplyShipMovement(GameManager.Instance.CurrentShipToPlay,
                                            movement.Last());
                                        ServiceGame.RegisterMovement(new MoveDTO
                                        {
                                            move = (action.realisationRuleResult.move != null)
                                                ? new Move
                                                {
                                                    cost = actualMovement.cost, moveDetails = actualMovement.moveDetails
                                                }
                                                : null,
                                            ship = GameManager.Instance.CurrentShipToPlay
                                        });

                                        if (actualMovement.cost > 0)
                                            HistoricsManager.Instance.NewMessage(
                                                $"[Tour {ServiceGame.GetCurrentTurn.number}] - [Faction : {faction.name}] - Le navire '{GameManager.Instance.CurrentShipToPlay.name}' " +
                                                $"a bougé de {actualMovement.moveDetails.Sum(x => x.Value)} cases et a perdu {actualMovement.cost} de gréément suite à son mouvement ! " +
                                                $"Reste : {GameManager.Instance.CurrentShipToPlay.shipBoard.rigging}");

                                        sb.AppendLine(
                                            $"J'ai bougé de {actualMovement.moveDetails.Sum(x => x.Value)} cases et perdu {actualMovement.cost} de gréément.");
                                    }

                                    break;
                                default:
                                    break;
                            }

                            shipManager.PrintActionText(sb.ToString());
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
                //yield return new WaitForSeconds(1f);

                GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetPlayingHumanShipObject);
                GameManager.Instance.ToggleCamMovement(true);

                // on génère un rapport de fin de tour
                yield return StartCoroutine("GenerateReport");

                // fin du tour : envoi du rapport au back
                yield return StartCoroutine("EndTurn");

                //StartTurn();
            }
        }

        IEnumerator NewTurn()
        {
            ServiceGame.StartNewTurn();
            yield return null;
        }

        IEnumerable GenerateReport()
        {
            ServiceGame.GetReport();
            yield return null;
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