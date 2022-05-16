using System.Collections.Generic;
using Newtonsoft.Json;

namespace ColanderSource
{
    public class ColonisationEventDTO : EventDTO
    {
        [JsonProperty(Order = 0)]
        public string owner { get; set; }
        [JsonProperty(Order = 0)]
        public Ship ship { get; set; }
        [JsonProperty(Order = 0)]
        public List<string> landingNpcs { get; set; }
        [JsonProperty(Order = 0)]
        public List<string> onBoardNpcs { get; set; }
        [JsonProperty(Order = 0)]
        public Island island { get; set; }

        public ColonisationEventDTO()
        {
            eventType = "COLONISATION";
        }
    }
}
