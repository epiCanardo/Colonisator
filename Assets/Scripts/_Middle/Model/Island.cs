using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// classe Island
    /// </summary>
    public class Island : ColanderSourceModel
    {
        public string name { get; set; }
        public Square harbourCoordinates { get; set; }
        public string owner { get; set; }
        public List<string> npcs { get; set; }
        public IslandBoard islandBoard { get; set; }

        //public string OwnerText => (owner != null) ? $"Faction propriétaire : {owner.name}" : $"Faction propriétaire : aucune";
        public string PopulationText => $"{npcs.Count}";

    }
}