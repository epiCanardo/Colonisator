using System.Collections.Generic;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
{
    public class PunctureEventDTO : EventDTO
    {
        [JsonProperty(Order = 0)]
        public List<string> npcIds { get; set; }
        [JsonProperty(Order = 0)]
        public string sourceShipId { get; set; }
        [JsonProperty(Order = 0)]
        public string targetShipId { get; set; }

        public PunctureEventDTO()
        {
            eventType = "PUNCTURE";
        }
    }
}
