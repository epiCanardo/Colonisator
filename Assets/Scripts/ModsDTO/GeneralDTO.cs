using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class GeneralDTO : ConfigDTO<GeneralDTO>
    {
        public Values gameplayValues { get; set; }
        public List<ShipDefinition> shipDefinitions { get; set; }

        public class CrewBounds
        {
            public int min { get; set; }
            public int max { get; set; }
        }
        
        public class ShipboardBounds
        {
            public int food { get; set; }
            public int rigging { get; set; }
            public int hull { get; set; }
            public int cannonsCount { get; set; }
            public int powderBarrels { get; set; }
            public int ammoCount { get; set; }
            public int pinnaces { get; set; }
        }

        public class ShipDefinition
        {
            public string shipTypeEnum { get; set; }
            public ShipboardBounds shipboardBounds { get; set; }
            public CrewBounds crewBounds { get; set; }
        }

        public class Values
        {
            public int RIGGING_COST_WIND_DIRECTION { get; set; }
            public int RIGGING_COST_AGAINST_WIND_DIRECTION { get; set; }
            public int RIGGING_COST_CROSS_CROSS_WIND_DIRECTION { get; set; }
            public int COLONIZATION_MINIMUM_SAILORS { get; set; }
            public int COLONIZATION_MINIMUM_FOOD { get; set; }
            public int SHIP_FOOD_CONSUMPTION_BASE_RATE { get; set; }
            public int SHIP_FOOD_CONSUMPTION_CREW_STEP { get; set; }
        }
    }
}