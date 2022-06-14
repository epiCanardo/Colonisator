using Assets.Scripts.Model;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
{
    public class MoveEventDTO : EventDTO
    {
        [JsonProperty(Order = 0)]
        public Ship ship { get; set; }
        [JsonProperty(Order = 0)]
        public Move move { get; set; }

        public MoveEventDTO()
        {
            eventType = "MOVE";
        }
    }
}
