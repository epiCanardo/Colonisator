using System.Collections.Generic;

namespace Assets.Scripts.ModsDTO
{
    public class FactionsDefinitionDTO : ConfigDTO<FactionsDefinitionDTO>
    {       
        public List<FactionsDefinition> factionsDefinition { get; set; }

        public class Ship
        {
            public string shipTypeEnum { get; set; }
            public string harbor { get; set; }
        }

        public class FactionsDefinition
        {
            public string id { get; set; }
            public string playerTypeEnum { get; set; }
            public string factionPostureEnum { get; set; }
            public List<string> allies { get; set; }
            public List<string> neutral { get; set; }
            public List<string> enemies { get; set; }
            public List<Ship> ships { get; set; }
        }
    }
}