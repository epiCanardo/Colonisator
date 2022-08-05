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
            }
        }

        public bool Equals(Faction faction)
        {
            return this.id.Equals(faction.id);
        }

        public static string PlayerTypeFromEnum(PlayerTypeEnum playerTypeEnum)
        {
            switch (playerTypeEnum)
            {
                case PlayerTypeEnum.COMPETITOR:
                    return "COMPETITOR";
                case PlayerTypeEnum.NEUTRAL:
                    return "NEUTRAL";
                case PlayerTypeEnum.PIRATE:
                    return "PIRATE";
                case PlayerTypeEnum.REBEL_SAILORS:
                    return "REBEL_SAILORS";
                case PlayerTypeEnum.TOWN:
                    return "TOWN";
                case PlayerTypeEnum.PENITENTIARY:
                    return "PENITENTIARY";
                case PlayerTypeEnum.HUMAN:
                    return "HUMAN";
                case PlayerTypeEnum.PRISON:
                    return "PRISON";
                case PlayerTypeEnum.GHOST:
                    return "GHOST";
                default:
                    return "";
            }
        }
    }

    public enum FacturePosture
    {
        DEFAULT,
        DEFENSIVE,
        MERCHANT,
        AGGRESSIVE
    }

    public enum PlayerTypeEnum
    {
        COMPETITOR,
        NEUTRAL,
        PIRATE,
        REBEL_SAILORS,
        TOWN,
        PENITENTIARY,
        HUMAN,
        PRISON,
        GHOST
    }
}