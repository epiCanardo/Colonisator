using System.Collections.Generic;
using Assets.Scripts.Model;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
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
