using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// Classe Game
    /// </summary>
    internal class Game : ColanderSourceModel
    {        
        public CurrentTurn currentTurn { get; set; }
        public IEnumerable<Island> islands { get; set; }
        public IEnumerable<Faction> factions { get; set; }
        public IEnumerable<Npc> nonPlayerCharacters { get; set; }
        public IEnumerable<Ship> ships { get; set; }
    }
}