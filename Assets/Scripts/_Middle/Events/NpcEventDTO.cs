using System.Collections.Generic;
using Newtonsoft.Json;

namespace ColanderSource
{
    public class NpcEventDTO : EventDTO
    {
        [JsonProperty(Order = 0)]
        public Npc npc { get; set; }

        public NpcEventDTO()
        {
            eventType = "NPC";
        }
    }
}
