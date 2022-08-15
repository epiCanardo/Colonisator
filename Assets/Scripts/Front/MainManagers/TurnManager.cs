using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.DTO;
using Assets.Scripts.EventsDTO;
using Assets.Scripts.Front.Cams;
using Assets.Scripts.Front.ScriptableObjects.Faction;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using Assets.Scripts.Service;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Front.MainManagers
{
    public class TurnManager : UnityEngine.MonoBehaviour
    {
        [Header("Affichage du tour de jeu")]
        public TextMeshProUGUI CurrentTurnText;

        [Header("Boutons de la barre d'action")]
        public GameObject EndTurnButton;
        public GameObject MoveShipButton;

        [Header("Carte")]
        public GameObject cardObject;
        public static TurnManager Instance { get; private set; }

        private bool nonHumanAutoTestActive = false;

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public TurnState MainState { get; set; }

        public void BounceButton()
        {
            EndTurnButton.GetComponent<RawImage>().transform.DOScale(0.5f, 1).SetLoops(-1, LoopType.Yoyo);
        }

        public void StopBouncing()
        {
            EndTurnButton.GetComponent<RawImage>().transform.DOKill();
            EndTurnButton.GetComponent<RawImage>().transform.localScale = Vector3.one;
        }

        public IEnumerator StartTurn()
        {
            for (int i = 0; i < 1000; i++)
            {
                // démarrage du tour
                yield return StartCoroutine("NewTurn");

                GameManager.Instance.BlinkTokenBorder();

                MainState = TurnState.ActionsStarted;
                Camera.main.GetComponent<CamMovement>().SetCamToActionLevel();
                CurrentTurnText.text = string.Format(ModManager.Instance.GetSentence(SentenceDTO.CURRENT_TURN_START),
                    ServiceGame.GetCurrentTurn.number);

                // pour chaque tour de faction, on déroule les actions
                foreach (var factionTurn in ServiceGame.GetCurrentTurn.factionsAndShips)
                {
                    Faction faction = ServiceGame.GetFactionFromId(factionTurn.Key);

                    // si la faction ne joue pas, ça dégage
                    var play = FactionsManager.Instance.GetFactionManager(faction).IsPlaying;
                    if (!play)
                        continue;

                    // si c'est au joueur humain de jouer, on laisse la main. La fonction reprendra lorsque MainState sera AI
                    if (faction.playerTypeEnum == FactionsDTO.HUMAN)
                    {
                        if (nonHumanAutoTestActive)
                            continue;

                        GameManager.Instance.ToggleCamMovement(true);
                        MainState = TurnState.Human;
                        // positionnement de la caméra derrière le navire en cours
                        GameManager.Instance.CurrentShipToPlay = ServiceGame.GetHumanShip;
                        GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);
                        CurrentTurnText.text =
                            string.Format(ModManager.Instance.GetSentence(SentenceDTO.CURRENT_TURN_DETAIL),
                                ServiceGame.GetCurrentTurn.number, ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).longName,
                                GameManager.Instance.CurrentShipToPlay.name);

                        // carte piochée ou mouvement ?
                        Card card = factionTurn.Value.First().card;
                        if (card != null)
                        {
                            var cardMenu = MenusManager.Instance.TryOpenMenu("card", cardObject);
                            CardBoard cardBoard = cardMenu.GetComponent<CardBoard>();
                            cardBoard.callbackFunc += CardChoiceCallBack;
                            cardBoard.SetCard(card);

                            yield return new WaitUntil(() => MainState == TurnState.CardChoiceDone);
                            MenusManager.Instance.TryDestroyMenu("card");
                            MainState = TurnState.Human;
                        }
                        // si pas de carte piochée, on peut avancer
                        //else
                        //{
                        MoveShipButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);

                        // dès que le mode navigation est démarré, le bouton n'est plus actif
                        yield return new WaitUntil(() => MainState == TurnState.NavigationMode);
                        MoveShipButton.GetComponent<Image>().color = new Color(0, 0, 0, 0.196f);
                        //}

                        // TODO : mettre un bouton pour mettre fin ou tour du joueur
                        yield return new WaitUntil(() => MainState == TurnState.AI || MainState == TurnState.CardChoiceDone);
                        MoveShipButton.GetComponent<Image>().color = new Color(0, 0, 0, 0.196f);
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
                                string.Format(ModManager.Instance.GetSentence(SentenceDTO.CURRENT_TURN_DETAIL),
                                    ServiceGame.GetCurrentTurn.number, ModManager.Instance.GetFactionLabel(faction.playerTypeEnum, x => x.shortLabel),
                                    GameManager.Instance.CurrentShipToPlay.name);

                            yield return new WaitForSeconds(0.1f);

                            // si c'est un navire IA, il effectue les actions prévues

                            // affichage de l'action prévue
                            StringBuilder sb = new StringBuilder();

                            // travail sur les objectifs
                            switch (action.objectiveRuleResult?.objectiveEnum)
                            {
                                case ObjectiveRuleResult.COLONIZE_ISLAND:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_COLONIZE));
                                    break;
                                case ObjectiveRuleResult.PUNCTURE_CREW:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_PUNCTURE));
                                    break;
                                case ObjectiveRuleResult.REFOURGUER_CREW:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_FIRECREW));
                                    break;
                                case ObjectiveRuleResult.GET_RIGGING:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_BUY_RIGGING));
                                    break;
                                case ObjectiveRuleResult.GET_FOOD:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_BUY_FOOD));
                                    break;
                                case ObjectiveRuleResult.GET_CREW:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_WANNA_BUY_CREW));
                                    break;
                                default:
                                    sb.AppendLine(ModManager.Instance.GetSentence(SentenceDTO.OBJECTIVE_NOTHING));
                                    break;
                            }

                            // travail sur les solutions
                            switch (action.solutionRuleResult?.solutionEnum)
                            {
                                case SolutionRuleResult.REFOURGUER_CREW:
                                    List<Npc> landingNpcs = ServiceGame.GetNpcs(action.realisationRuleResult?.npcs)
                                        .ToList();
                                    TradeDTO trade = new TradeDTO
                                    {
                                        ship = shipManager.ship,
                                        island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                        landingNpcs = landingNpcs,
                                    };
                                    ServiceGame.Trade(trade);
                                    sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_FIRECREW_DONE), landingNpcs.Count));

                                    HistoricsManager.Instance.NewMessage(string.Format(ModManager.Instance.GetSentence(
                                        SentenceDTO.HISTORIC_FIRECREW_DONE),
                                        ServiceGame.GetCurrentTurn.number, ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).longName,
                                        GameManager.Instance.CurrentShipToPlay.name,
                                        trade.landingNpcs.Count, GameManager.Instance.CurrentShipToPlay.crew.Count));

                                    break;
                                case SolutionRuleResult.GO_TO_ISLAND:
                                    sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_GO_TO_ISLAND),
                                        ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId).name));
                                    break;
                                case SolutionRuleResult.BUY:
                                    TradeDTO buyTrade = new TradeDTO();

                                    switch (action.solutionRuleResult?.ressource)
                                    {
                                        case "RIGGING":
                                            //  -> 1000 dodris forfaitaires quel que soit la quantité
                                            //  -> quantité : pour atteindre les 100
                                            buyTrade = new TradeDTO
                                            {
                                                ship = GameManager.Instance.CurrentShipToPlay,
                                                island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                                buys = new List<TradeLine>
                                                {
                                                    new TradeLine
                                                        {
                                                            ressource = "rigging",
                                                            quantity = 100 - shipManager.ship.shipBoard.rigging, cost = 1000
                                                        }
                                                },
                                                deltaStuff = new ShipBoard
                                                {
                                                    rigging = 100 - shipManager.ship.shipBoard.rigging,
                                                    dodris = -1000
                                                }
                                            };
                                            sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_BOUGHT_RIGGING),
                                                buyTrade.deltaStuff.rigging, -buyTrade.deltaStuff.dodris));
                                            break;
                                        case "FOOD":
                                            //  -> 1000 dodris forfaitaires quel que soit la quantité
                                            //  -> quantité : pour atteindre les 100
                                            buyTrade = new TradeDTO
                                            {
                                                ship = GameManager.Instance.CurrentShipToPlay,
                                                island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                                buys = new List<TradeLine>
                                                {
                                                    {
                                                        new TradeLine
                                                        {
                                                            ressource = "food",
                                                            quantity = 100 - shipManager.ship.shipBoard.food, cost = 1000
                                                        }
                                                    }
                                                },
                                                deltaStuff = new ShipBoard
                                                {
                                                    food = 100 - shipManager.ship.shipBoard.food,
                                                    dodris = -1000
                                                }
                                            };
                                            sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_BOUGHT_FOOD),
                                                buyTrade.deltaStuff.food, -buyTrade.deltaStuff.food));
                                            break;
                                        case "CREW":
                                            Island islandCrew = ServiceGame.GetIsland(GameManager.Instance.CurrentShipToPlay
                                                .coordinates);
                                            //  -> 1000 dodris forfaitaires quel que soit la quantité
                                            //  -> quantité : pour atteindre les 100
                                            buyTrade = new TradeDTO
                                            {
                                                ship = GameManager.Instance.CurrentShipToPlay,
                                                island = ServiceGame.GetIslandFromId(action.solutionRuleResult.islandId),
                                                boardingNpcs = ServiceGame.GetNpcs(islandCrew.npcs).Where(x => x.Rang == "SAILOR")
                                                    .Take(Mathf.Min(action.solutionRuleResult.quantity, islandCrew.npcs.Count)).ToList()
                                            };
                                            sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_HIRED_CREW),
                                                buyTrade.boardingNpcs.Count, -buyTrade.deltaStuff.dodris));
                                            break;
                                    }

                                    // réalisation du trade
                                    ServiceGame.Trade(buyTrade);

                                    break;
                                case SolutionRuleResult.COLONIZE:
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

                                    HistoricsManager.Instance.NewMessage(string.Format(ModManager.Instance.GetSentence(
                                        SentenceDTO.HISTORIC_COLONIZATION_DONE),
                                        ServiceGame.GetCurrentTurn.number, ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).longName,
                                        GameManager.Instance.CurrentShipToPlay.name,
                                        island.name, dtoColonisation.npcs.Count,
                                        GameManager.Instance.CurrentShipToPlay.crew.Count));

                                    sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_COLONIZATION_DONE), island.name));
                                    break;
                                case SolutionRuleResult.PUNCTURE_CREW:
                                    sb.AppendLine(string.Format(ModManager.Instance.GetSentence(SentenceDTO.SOLUTION_PUNCTURE_DONE),
                                        action.realisationRuleResult.npcs.Count));

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

                                        HistoricsManager.Instance.NewMessage(string.Format(ModManager.Instance.GetSentence(SentenceDTO.HISTORIC_PUNCTURE_DONE),
                                                ServiceGame.GetCurrentTurn.number, ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).longName,
                                                punctureDto.npcIds.Count));
                                    }

                                    break;
                                default:
                                    break;
                            }

                            // travail sur les réalisations
                            switch (action.realisationRuleResult?.realisationEnum)
                            {
                                case RealisationRuleResult.MOVE:
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
                                        yield return Camera.main.transform.DOMove(
                                            new Vector3(physicalSquare.transform.position.x, Camera.main.transform.position.y, physicalSquare.transform.position.z - 100)  /*+ GameManager.Instance.camOffSet*/,
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
                                                    cost = actualMovement.cost,
                                                    moveDetails = actualMovement.moveDetails
                                                }
                                                : null,
                                            ship = GameManager.Instance.CurrentShipToPlay
                                        });

                                        if (actualMovement.cost > 0)
                                            HistoricsManager.Instance.NewMessage(string.Format(
                                                ModManager.Instance.GetSentence(SentenceDTO.HISTORIC_MOVE_DONE),
                                                ServiceGame.GetCurrentTurn.number,
                                                ServiceGame.GetFaction(GameManager.Instance.CurrentShipToPlay).longName,
                                                GameManager.Instance.CurrentShipToPlay.name,
                                                actualMovement.moveDetails.Sum(x => x.Value), actualMovement.cost,
                                                GameManager.Instance.CurrentShipToPlay.shipBoard.rigging));

                                        sb.AppendLine(string.Format(
                                            ModManager.Instance.GetSentence(SentenceDTO.REALISATION_MOVE_DONE),
                                            actualMovement.moveDetails.Sum(x => x.Value), actualMovement.cost));
                                    }

                                    break;
                            }

                            // consommation de nourriture à bord pour tous les navires sauf le ghost
                            if (faction.playerTypeEnum != FactionsDTO.GHOST)
                                ServiceGame.ConsumeFood(GameManager.Instance.CurrentShipToPlay);

                            // affichage des actions
                            shipManager.PrintActionText(sb.ToString());
                        }
                    }
                }

                // à la fin du tour, on revient par défaut sur le navire du joueur
                // var color = BackgroundColor.GetComponent<Image>().color;
                // var targetColor = new Color(color.r, color.g, color.b, 0);
                // yield return BackgroundColor.GetComponent<Image>().DOColor(targetColor, 0.5f).WaitForCompletion();

                EndTurnButton.GetComponent<Image>().color = new Color(0, 0, 0, 1);

                GameManager.Instance.StopBlinkTokenBorder();

                // gestion de la fin du tour
                if (nonHumanAutoTestActive)
                    MainState = TurnState.ActionsFinished;
                else
                {
                    // on se place en attente de fin de tour
                    MainState = TurnState.WaitForEndTurn;
                    // BackgroundColor.GetComponent<Image>().DOColor(Color.red, 1).SetEase(Ease.InBounce);
                    yield return new WaitUntil(() => MainState == TurnState.ActionsFinished);
                }

                EndTurnButton.GetComponent<Image>().color = new Color(0, 0, 0, 0.196f);
                CurrentTurnText.text = $"Tour {ServiceGame.GetCurrentTurn.number} : Fin du tour";
                //  targetColor = new Color(color.r, color.g, color.b, 0.8f);
                //  BackgroundColor.GetComponent<Image>().DOColor(targetColor, 0.5f);

                CurrentTurnText.text = $"Nouveau tour dans 1 seconde...";
                //yield return new WaitForSeconds(1f);

                //GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetPlayingHumanShipObject);
                //GameManager.Instance.ToggleCamMovement(true);

                // on génère un rapport de fin de tour
                ServiceGame.GetReport();

                // fin du tour : envoi du rapport au back
                yield return StartCoroutine("EndTurn");
            }
        }

        IEnumerator NewTurn()
        {
            ServiceGame.StartNewTurn();
            yield return null;
        }

        IEnumerator EndTurn()
        {
            ServiceGame.EndTurn();
            yield return null;
        }
        /// <summary>
        /// récupération du choix réalisé et application des conséquences
        /// </summary>
        /// <param name="choice"></param>
        private void CardChoiceCallBack(CardChoice choice)
        {
            // création d'un trade pour les conséquences
            // TODO : actuellement, seul le différentiel de table de board est géré
            ServiceGame.Trade(new TradeDTO
            {
                deltaStuff = choice.shipBoardDelta,
                ship = ServiceGame.GetShip(choice.shipId)
            });
            
            // affichage des conséquences
            StartCoroutine(GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().PrintCardConsequencesText(choice.FormatedConsequence));
            MainState = TurnState.CardChoiceDone;
        }
    }

    public enum TurnState
    {
        ActionsStarted,
        Human,
        NavigationMode,
        CardChoiceDone,
        AI,
        WaitForEndTurn,
        ActionsFinished,
    }
}