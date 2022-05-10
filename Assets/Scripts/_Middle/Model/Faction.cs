using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// classe Island
    /// </summary>
    public class Faction : ColanderSourceModel
    {
        public string name { get; set; }
        public string playerTypeEnum { get; set; }
        public string factionPostureEnum { get; set; }
        public IEnumerable<string> allies { get; set; }
        public IEnumerable<string> neutral { get; set; }
        public IEnumerable<string> enemies { get; set; }

        public bool Equals(Faction faction)
        {
            return this.id.Equals(faction.id);
        }

    }

    public enum PlayerType
    {
        COMPETITOR, // les concurrents
        NEUTRAL, // les îles neutres - CUII
        PIRATE, // les îles de la CPL
        REBEL_SAILORS, // la CMR
        TOWN, // Sundercity
        PENITENTIARY, // Piofo
        OTHER, //
        HUMAN, // les joueurs humains
        PRISON, // Missytown
        GHOST // navire fantôme
    }

    public enum FacturePosture
    {
        DEFAULT,
        DEFENSIVE,
        MERCHANT,
        AGGRESSIVE
    }
}