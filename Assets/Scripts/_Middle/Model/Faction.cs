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

        public string longName
        {
            get
            {
                switch (playerTypeEnum)
                {
                    case "COMPETITOR":
                        return name;
                        break;
                    case "NEUTRAL":
                        return "Comité d'Union des Îles Indépendantes";
                        break;
                    case "PIRATE":
                        return "Communauté des Pirates Libres";
                        break;
                    case "REBEL_SAILORS":
                        return "Confrérie des Matelots Révoltés";
                        break;
                    case "TOWN":
                        return "Sundercity";
                        break;
                    case "PENITENTIARY":
                        return "Centre de détention de Piofo";
                        break;
                    case "HUMAN":
                        return name;
                        break;
                    case "PRISON":
                        return "Colonie pénitencière de Missytown";
                        break;
                    case "GHOST":
                        return "Fantôme";
                        break;
                    default:
                        return "Pas de nom";
                        break;
                }
            }
        }

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