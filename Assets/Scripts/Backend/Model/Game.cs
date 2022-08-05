using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts.Model
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

        public string ToJson()
        {
            try
            {
                return JsonConvert.SerializeObject(this, Formatting.Indented);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}