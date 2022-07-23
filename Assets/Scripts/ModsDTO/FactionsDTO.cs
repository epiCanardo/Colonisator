using System.Collections.Generic;

namespace Assets.Scripts.ModsDTO
{
    public class FactionsDTO : ConfigDTO<FactionsDTO>
    {
        public const string COMPETITOR = "COMPETITOR";
        public const string NEUTRAL = "NEUTRAL";
        public const string PIRATE = "PIRATE";
        public const string REBEL_SAILORS = "REBEL_SAILORS";
        public const string TOWN = "TOWN";
        public const string PENITENTIARY = "PENITENTIARY";
        public const string HUMAN = "HUMAN";
        public const string PRISON = "PRISON";
        public const string GHOST = "GHOST";

        public List<FactionDTOObject> factions { get; set; }
        public List<string> customNames { get; set; }        
    }

    public class FactionDTOObject
    {
        public string key { get; set; }
        public string shortLabel { get; set; }
        public string longLabel { get; set; }
    }
}