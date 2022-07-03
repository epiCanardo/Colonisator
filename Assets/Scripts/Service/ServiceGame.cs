using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using Assets.Scripts.DTO;
using Assets.Scripts.EventsDTO;
using Assets.Scripts.Model;
using Assets.Scripts.ModsDTO;
using Newtonsoft.Json;
using RestSharp;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = System.Random;

namespace Assets.Scripts.Service
{
    public static class ServiceGame
    {
        private static Game game;
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static ReportDTO<EventDTO> report;

        private static void StartBack()
        {
            string processLocation = ModManager.Instance.GetColbackLocation();

            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = @"/k " + processLocation // cmd.exe specific implementation
            };
            p.StartInfo = startInfo;
            p.Start();

            // Process.Start("cmd", $"/k {processLocation}");

           bool isPortOpen = false;
            while(!isPortOpen)
            {
                isPortOpen = IsPortOpen(/*"localhost",*/ 8080/*, new TimeSpan(0, 0, 10)*/);
                if (isPortOpen)
                    break;

                Thread.Sleep(2000);
            }
        }

        private static bool IsPortOpen(/*string host,*/ int port/*, TimeSpan timeout*/)
        {
            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// génération d'une partie en exploitant l'api rest
        /// </summary>
        /// <param name="npcCount">nombre de pnj demandés</param>
        /// <param name="rndSeed"></param>
        /// <returns></returns>
        public static void GenerateGame(int npcCount, string rndSeed)
        {
            bool isPortOpen = IsPortOpen(8080);
            if (!isPortOpen)
                StartBack();

            RestClient client = new RestClient($"http://localhost:8080/games/init?nbNPC={npcCount}");
            RestRequest request = new RestRequest(Method.GET);
            game = client.Execute<Game>(request).Data;

            if (!string.IsNullOrEmpty(rndSeed))
                InitRandomWithSeed(rndSeed);
        }

        /// <summary>
        /// génération d'une partie selon un fichier data
        /// </summary>
        /// <param name="gameDatapath"></param>
        public static void GenerateGame(string gameDatapath)
        {
            string jsonMoq;

            using (StreamReader sR = new StreamReader(gameDatapath))
            {
                jsonMoq = sR.ReadToEnd();
                sR.Close();
            }

            //JsonSerializerOptions options = new JsonSerializerOptions
            //{
            //    Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
            // };

            //game = JsonSerializer.Deserialize<Game>(jsonMoq, options);
            game = JsonConvert.DeserializeObject<Game>(jsonMoq);
        }

        /// <summary>
        /// récupération en envoi des cartes
        /// </summary>
        public static void SendCards()
        {
            var client = new RestClient($"http://localhost:8080/games/{game.id}/cards");
            var request = new RestRequest(Method.POST);

            CardsDTO cards = new CardsDTO(ModManager.Instance.GetCards());

            request.AddJsonBody(cards.ToJson());
            client.Post<CardsDTO>(request);
        }

        /// <summary>
        /// démarrage d'un nouveau tour
        /// </summary>
        public static void StartNewTurn(bool test = false)
        {
            if (!test)
            {
                var client = new RestClient($"http://localhost:8080/games/{game.id}/nextTurn");
                var request = new RestRequest(Method.GET);

                // le game est écrasé avec les nouvelles données renvoyées
                game = client.Execute<Game>(request).Data;
            }

            // initialisation du rapport du tour à transmettre en fin de tour
            report = new ReportDTO<EventDTO>();

            using (StreamWriter sW = new StreamWriter($"Reports/game_turn_{GetCurrentTurn.number}.json"))
            {
                sW.Write(game.ToJson());
                sW.Close();
            }
        }

        /// <summary>
        /// initialise le random avec un seed passé en paramètre, lors de la création d'une partie
        /// </summary>
        /// <param name="seedName"></param>
        public static void InitRandomWithSeed(string seedName)
        {
            rnd = new Random(seedName.GetHashCode());
        }

        /// <summary>
        /// les îles
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Island> Islands => game.islands;

        /// <summary>
        /// les îles
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Faction> Factions => game.factions;

        /// <summary>
        /// donne le boss d'une faction
        /// </summary>
        /// <param name="factionId">l'id de la faction</param>
        /// <returns></returns>
        public static Npc FactionBoss(string factionId)
        {
            return Npcs.FirstOrDefault(x => x.rankEnum.Equals("BOSS") && x.faction.Equals(factionId));
        }

        /// <summary>
        /// donne la population de la faction
        /// </summary>
        /// <param name="factionId"></param>
        /// <returns></returns>
        public static int FactionPopulation(string factionId)
        {
            return Npcs.Count(x => x.faction!= null && x.faction.Equals(factionId));
        }

        /// <summary>
        /// donne les navires possédés par la faction
        /// </summary>
        /// <param name="factionId"></param>
        /// <returns></returns>
        public static IEnumerable<Ship> FactionOwnedShips(string factionId)
        {
            return Ships.Where(x => x.owner != null && x.owner.Equals(factionId));
        }

        /// <summary>
        /// donne les îles possédées par la faction
        /// </summary>
        /// <param name="factionId"></param>
        /// <returns></returns>
        public static IEnumerable<Island> FactionOwnedIslands(string factionId)
        {
            return Islands.Where(x => x.owner != null && x.owner.Equals(factionId));
        }

        /// <summary>
        /// retourne une Faction selon son nom
        /// </summary>
        /// <param name="playerType"></param>
        /// <returns></returns>
        public static Faction GetFactionFromName(string factionName)
        {
            return game.factions.First(x => x.name.Equals(factionName));
        }

        /// <summary>
        /// retourne une Faction selon son ID
        /// </summary>
        /// <param name="factionId"></param>
        /// <returns></returns>
        public static Faction GetFactionFromId(string factionId)
        {
            return game.factions.FirstOrDefault(x => x.id.Equals(factionId));
        }

        /// <summary>
        /// retourne la Faction propriétaire d'un navire
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static Faction GetFaction(Ship ship)
        {
            return game.factions.First(x => x.id == ship.owner);
        }

        /// <summary>
        /// les npc
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Npc> Npcs => game.nonPlayerCharacters;

        /// <summary>
        /// les navires
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Ship> Ships => game.ships;


        /// <summary>
        /// retourne les navires appartenant à une faction
        /// </summary>
        /// <param name="faction"></param>
        /// <returns></returns>
        public static IEnumerable<Ship> GetShipsFromFaction(Faction faction)
        {
            return game.ships.Where(x => x.owner.Equals(faction.id));
        }

        /// <summary>
        /// retourne le navire selon son id
        /// </summary>
        /// <param name="shipId"></param>
        /// <returns></returns>
        public static Ship GetShip(string shipId)
        {
            return game.ships.First(x => x.id.Equals(shipId));
        }

        /// <summary>
        /// retourne les navires selon les id
        /// </summary>
        /// <param name="shipIds"></param>
        /// <returns></returns>
        public static List<Ship> GetShips(List<string> shipIds)
        {
            return game.ships.Where(x => shipIds.Contains(x.id)).ToList();
        }


        /// <summary>
        /// retourne le navire du joueur humain
        /// </summary>
        /// <returns></returns>
        public static Ship GetHumanShip => 
            GetShipsFromFaction(game.factions.First(x => x.playerTypeEnum.Equals(FactionsDTO.HUMAN))).First();

        /// <summary>
        /// retourne le navire fantôme
        /// </summary>
        /// <returns></returns>
        public static Ship GetGhostShip => 
            GetShipsFromFaction(game.factions.First(x=>x.playerTypeEnum.Equals(FactionsDTO.GHOST))).First();


        /// <summary>
        /// retourne une Island selon les coordonnées du port
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        public static Island GetIsland(Square coordinates)
        {
            return game.islands.FirstOrDefault(x => x.harbourCoordinates.Equals(coordinates));
        }

        /// <summary>
        /// retourne une Island selon son id
        /// </summary>
        /// <param name="islandId"></param>
        /// <returns></returns>
        public static Island GetIslandFromId(string islandId)
        {
            return game.islands.FirstOrDefault(x => x.id.Equals(islandId));
        }

        /// <summary>
        /// retourne une Island selon son nom
        /// </summary>
        /// <param name="islandName"></param>
        /// <returns></returns>
        public static Island GetIsland(string islandName)
        {
            return game.islands.FirstOrDefault(x => x.name.Equals(islandName));
        }

        /// <summary>
        /// récupération de la liste des officiers
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Npc> GetOfficers => game.nonPlayerCharacters.Where(x => x.rankEnum.Equals("OFFICER"));

        /// <summary>
        /// les npcs qui ne sont pas à bord de navires
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Npc> GetGroundNpcs => game.islands.SelectMany(x => GetNpcs(x.npcs));


        /// <summary>
        /// liste de Npc en fonction de leurs id
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<Npc> GetNpcs(IEnumerable<string> list) =>
            game.nonPlayerCharacters.Where(x => list.Contains(x.id));

        // <summary>
        /// les npcs qui ne sont pas à bord de navires
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Npc> GetNpcs(Ship inShip)
        {
            return GetNpcs(inShip.crew);
        }

        /// <summary>
        /// les npcs qi sont à bord des navires
        /// </summary>
        /// <param name="inShips"></param>
        /// <returns></returns>
        public static IEnumerable<Npc> GetNpcs(IEnumerable<Ship> inShips)
        {
            return GetNpcs(inShips.SelectMany(x => x.crew));
        }

        /// <summary>
        /// Npc en fonction de son id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Npc GetNpc(string id) => game.nonPlayerCharacters.FirstOrDefault(x => x.id.Equals(id));

        /// <summary>
        /// La capitaine du navire
        /// </summary>
        /// <param name="ship">le navire</param>
        /// <returns></returns>
        public static Npc ShipCaptain(Ship ship)
            => GetNpcs(ship).First(x => (x.rankEnum.Equals("CAPTAIN") || x.rankEnum.Equals("BOSS")) && x.currentShip != null && x.currentShip.Equals(ship.id));

        /// <summary>
        /// la liste des officiers du navire
        /// </summary>
        /// <param name="ship">le navire</param>
        /// <returns></returns>
        public static IEnumerable<Npc> ShipOfficiers(Ship ship)
            => GetNpcs(ship).Where(x => x.rankEnum.Equals("OFFICER") && x.currentShip != null && x.currentShip.Equals(ship.id));

        /// <summary>
        /// la liste des matelots du navire
        /// </summary>
        /// <param name="ship">le navire</param>
        /// <returns></returns>
        public static IEnumerable<Npc> ShipSailors(Ship ship)
            => GetNpcs(ship).Where(x => x.rankEnum.Equals("SAILOR") && x.currentShip != null && x.currentShip.Equals(ship.id));

        /// <summary>
        /// donne le prochain mouvement de navire basé sur 2 D6
        /// </summary>
        /// <param name="ship"></param>
        /// <returns></returns>
        public static int GetMaxNavigation(Ship ship) => rnd.Next(2, 13);

        /// <summary>
        /// donne l'action du tour, basée sur un D6
        /// </summary>
        /// <returns></returns>
        public static int GetTurnFirstDice() => rnd.Next(1, 13);

        /// <summary>
        /// méthode de colonisation d'une île
        /// </summary>
        /// <param name="island"></param>
        /// <param name="ship"></param>
        public static void ColonizeIsland(ColonisationDTO dto)
        {
            // transfert des vivres
            dto.ship.shipBoard.food = Math.Max(0, dto.ship.shipBoard.food + dto.food);

            // augmentation de la confiance à bord
            dto.ship.shipBoard.order = Math.Min(100, dto.ship.shipBoard.order + dto.order);

            // transfert des matelots vers la nouvelle colonie
            foreach (var npc in dto.npcs)
            {
                dto.ship.crew.Remove(npc.id);
                npc.currentShip = null;

                npc.currentIsland = dto.island.id;
                dto.island.npcs.Add(npc.id);
            }

            // nouveau propriétaire
            dto.island.owner = GetFactionFromId(dto.ship.owner).id;

            // génération de l'event
            ColonisationEventDTO coloEvent = new ColonisationEventDTO
            {
                island = dto.island,
                landingNpcs = dto.npcs.Select(x => x.id).ToList(),
                owner = dto.island.owner,
                ship = dto.ship
            };
            report.events.Add(coloEvent);
        }

        /// <summary>
        /// Retourne la liste des cases du mouvement prévue
        /// </summary>
        /// <param name="shipAction">le mouvement prévu</param>
        public static List<Square> PrepareShipMovement(ShipTurnAction shipAction)
        {
            List<Square> result = new List<Square>();

            // si pas de mouvement, on retourne la liste vide
            if (shipAction.realisationRuleResult.move == null)
                return result;

            Ship ship = GetShip(shipAction.id);
            Square currentSquare = ship.coordinates;

            // on incrémente le mouvement avec la case de destination sur chaque direction
            foreach (KeyValuePair<string, int> move in shipAction.realisationRuleResult.move.moveDetails)
            {
                currentSquare = SingleDirectionShipMoving(currentSquare, move);
                result.Add(currentSquare);
            }

            return result;
        }

        /// <summary>
        /// cette méthode applique le mouvement réel du navire
        /// </summary>
        /// <param name="ship">le navire en question</param>
        /// <param name="waypoints">les points de coordonnées qui doivent incrémenter les coordonnées du navire</param>
        public static void ApplyShipMovement(Ship ship, Square finalSquare)
        {
            ship.coordinates = finalSquare;
        }

        /// <summary>
        /// Cette méthode est appelée par le front pour enregistrement du mouvement réel du navire
        /// Il peut y avoir une différence entre le mouvement prévu et celui effectué (évènements)
        /// Note : les coordonnées et les infos de table de bord sont déjà à jour (via le front)
        /// </summary>
        /// <param name="ship">le navire dont il est question</param>
        /// <param name="movement">le mouvement complet qui a été réalisé</param>
        public static void RegisterMovement(MoveDTO movement)
        {
            // génération de l'event
            MoveEventDTO moveEvent = new MoveEventDTO
            {
                ship = movement.ship,
                move = movement.move
            };
            report.events.Add(moveEvent);
        }

        /// <summary>
        /// retourne une case correspondant au mouvemment passé en paramètre
        /// </summary>
        /// <param name="startSquare">la case de départ</param>
        /// <param name="move">le nombre de cases de mouvement</param>
        /// <returns></returns>
        private static Square SingleDirectionShipMoving(Square startSquare, KeyValuePair<string, int> move)
        {
            switch (move.Key)
            {
                case "NORTH":
                    return new Square(startSquare.x, startSquare.y + move.Value);
                case "SOUTH":
                    return new Square(startSquare.x, startSquare.y - move.Value);
                case "WEST":
                    return new Square(startSquare.x - move.Value, startSquare.y);
                case "EAST":
                    return new Square(startSquare.x + move.Value, startSquare.y);
            }

            return startSquare;
        }

        /// <summary>
        /// action du navire fantôme, qui tue et prélève des matelots de leur navire
        /// </summary>
        /// <param name="npcs"></param>
        public static void Puncture(PunctureDTO puncture)
        {
            Ship sourceShip = GetShip(puncture.sourceShipId);
            Ship targetShip = GetShip(puncture.targetShipId);

            // on tue chaque pnj et on le balance dans le navire fantôme
            foreach (string npcId in puncture.npcIds)
            {
                Npc npc = GetNpc(npcId);
                sourceShip.crew.Remove(npcId);
                
                targetShip.crew.Add(npcId);
                npc.currentShip = targetShip.id;

                // il est mort !
                npc.healthState = 0;

                // changement de faction
                npc.faction = targetShip.owner;

                // report de la modification du npc
                report.events.Add(new NpcEventDTO
                {
                    npc = npc
                });
            }

            // pour le report
            report.events.Add(new PunctureEventDTO
            {
                sourceShipId = puncture.sourceShipId,
                targetShipId = puncture.targetShipId,
                npcIds = puncture.npcIds
            });
        }

        /// <summary>
        /// commerce entre un navire et une île
        /// </summary>
        /// <param name="trade"></param>
        /// <param name="ship"></param>
        /// <param name="island"></param>
        /// <param name="landingNpcs"></param>
        /// <param name="boardingNpcs"></param>
        public static void Trade(TradeDTO tradeDTO)
        {
            // gestion de l'équipage à débarquer
            if (tradeDTO.landingNpcs != null)
                foreach (Npc npc in tradeDTO.landingNpcs)
                {
                    tradeDTO.ship.crew.Remove(npc.id);
                    tradeDTO.island.npcs.Add(npc.id);
                    npc.currentShip = null;
                    npc.currentIsland = tradeDTO.island.id;
                    npc.faction = GetFactionFromId(tradeDTO.island.owner).id;
                }

            // gestion de l'équipage à embarquer
            if (tradeDTO.boardingNpcs != null)
                foreach (Npc npc in tradeDTO.boardingNpcs)
                {
                    tradeDTO.ship.crew.Add(npc.id);
                    tradeDTO.island.npcs.Remove(npc.id);
                    npc.currentShip = tradeDTO.ship.id;
                    npc.currentIsland = null;
                    npc.faction = GetFactionFromId(tradeDTO.ship.owner).id;
                }

            // gestion des échanges de ressources
            if (tradeDTO.deltaStuff != null)
                tradeDTO.ship.shipBoard.UpdateBoardWithDelta(tradeDTO.deltaStuff);

            // construction de l'event de type trade
            TradeEventDTO tradeEvent = new TradeEventDTO
            {
                shipDelta = new Ship
                {
                    id = tradeDTO.ship.id,
                    shipBoard = tradeDTO.ship.shipBoard
                },
                islandId = tradeDTO.island?.id,
                boardingNpcs = tradeDTO.boardingNpcs?.Select(x=>x.id).ToList(),
                landingNpcs = tradeDTO.landingNpcs?.Select(x => x.id).ToList(),
                buys = tradeDTO.buys,
                sells = tradeDTO.sells
            };

            report.events.Add(tradeEvent);
        }

        /// <summary>
        /// A chaque tour, un navire consomme de la nourriture en fonction de l'équipage
        /// </summary>
        /// <param name="ship"></param>
        public static void ConsumeFood(Ship ship)
        {
            int baseRate = ModManager.Instance.GetGameplayValue(x => x.gameplayValues.SHIP_FOOD_CONSUMPTION_BASE_RATE);
            float crewStep = ModManager.Instance.GetGameplayValue(x => x.gameplayValues.SHIP_FOOD_CONSUMPTION_CREW_STEP);

            int consumption = Mathf.CeilToInt(ship.crew.Count / crewStep) * baseRate;

            TradeEventDTO tradeEvent = new TradeEventDTO
            {
                shipDelta = new Ship
                {
                    id = ship.id,
                    shipBoard = new ShipBoard
                    {
                        food = Mathf.Clamp(ship.shipBoard.food - consumption, 0, ship.shipType.shipboardBounds.food),
                        dodris = ship.shipBoard.dodris,
                        hull = ship.shipBoard.hull,
                        rigging = ship.shipBoard.rigging,
                        order = ship.shipBoard.order
                    }
                }
            };

            report.events.Add(tradeEvent);
        }

        public static void LandNpcs(Island targetIsland, List<Npc> npcs)
        {
            foreach (var npc in npcs)
            {
                npc.currentIsland = targetIsland.id;
                npc.currentShip = null;
                npc.faction = targetIsland.owner;

                // report de la modification du npc
                report.events.Add(new NpcEventDTO
                {
                    npc = npc
                });
            }
        }

        public static void BoardNpcs(Ship targetShip, List<Npc> npcs)
        {
            foreach (var npc in npcs)
            {
                npc.currentIsland = null;
                npc.currentShip = targetShip.id;
                npc.faction = targetShip.owner;

                // report de la modification du npc
                report.events.Add(new NpcEventDTO
                {   
                    npc = npc
                });
            }
        }

        /// <summary>
        ///  récupère l'intégralité du tour courant
        /// </summary>
        public static CurrentTurn GetCurrentTurn => game.currentTurn;

        public static ReportDTO<EventDTO> GetReport()
        {
            using (StreamWriter sW = new StreamWriter($"Reports/report_turn_{GetCurrentTurn.number}.json"))
            {
                sW.Write(report.ToJson());
                sW.Close();
            }
            return report;
        }

        /// <summary>
        /// la fin du tour consiste à envoyer le rapport au back
        /// </summary>
        public static void EndTurn()
        {
            var client = new RestClient($"http://localhost:8080/games/{game.id}/report");
            var request = new RestRequest(Method.POST);
            request.AddJsonBody(report.ToJson());
            client.Post<ReportDTO<EventDTO>>(request);
        }
    }
}