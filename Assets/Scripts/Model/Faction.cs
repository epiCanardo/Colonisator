using System.Collections.Generic;
using Assets.Scripts.ModsDTO;
using Newtonsoft.Json;

namespace Assets.Scripts.Model
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

        [JsonIgnore]
        public string longName
        {
            get
            {
                if (playerTypeEnum.Equals(FactionsDTO.COMPETITOR) || playerTypeEnum.Equals(FactionsDTO.HUMAN))
                    return name;

                return ModManager.Instance.GetFactionLabel(playerTypeEnum, x => x.longLabel);

                //switch (playerTypeEnum)
                //{
                //    case FactionsDTO.COMPETITOR:
                //        return name;
                //        break;
                //    case FactionsDTO.NEUTRAL:
                //        return ModManager.Instance.GetFactionLabel(FactionsDTO.NEUTRAL, x => x.longLabel);
                //        break;
                //    case FactionsDTO.PIRATE:
                //        return "Communauté des Pirates Libres";
                //        break;
                //    case FactionsDTO.REBEL_SAILORS:
                //        return "Confrérie des Matelots Révoltés";
                //        break;
                //    case FactionsDTO.TOWN:
                //        return "Sundercity";
                //        break;
                //    case FactionsDTO.PENITENTIARY:
                //        return "Centre de détention de Piofo";
                //        break;
                //    case FactionsDTO.HUMAN:
                //        return name;
                //        break;
                //    case FactionsDTO.PRISON:
                //        return "Colonie pénitencière de Missytown";
                //        break;
                //    case FactionsDTO.GHOST:
                //        return "Fantôme";
                //        break;
                //    default:
                //        return "Pas de nom";
                //        break;
                //}
            }
        }

        public bool Equals(Faction faction)
        {
            return this.id.Equals(faction.id);
        }

    }

    public enum FacturePosture
    {
        DEFAULT,
        DEFENSIVE,
        MERCHANT,
        AGGRESSIVE
    }
}