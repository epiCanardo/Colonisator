using Assets.Scripts.Model;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
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
