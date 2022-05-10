using Newtonsoft.Json;

namespace ColanderSource
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
