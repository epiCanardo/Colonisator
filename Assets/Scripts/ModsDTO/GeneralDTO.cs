using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class GeneralDTO : ConfigDTO<GeneralDTO>
    {
        public int RIGGING_COST_WIND_DIRECTION { get; set; }
        public int RIGGING_COST_AGAINST_WIND_DIRECTION { get; set; }
        public int RIGGING_COST_CROSS_CROSS_WIND_DIRECTION { get; set; }
        public int COLONIZATION_MINIMUM_SAILORS { get; set; }
        public int COLONIZATION_MINIMUM_FOOD { get; set; }
        public int SHIP_MINIMUM_SAILORS { get; set; }
        public int SHIP_FOOD_CONSUMPTION_BASE_RATE { get; set; }
        public int SHIP_FOOD_CONSUMPTION_CREW_STEP { get; set; }
    }
}