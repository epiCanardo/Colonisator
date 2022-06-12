using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace ColanderSource
{
    public class EventDTO : ColanderSourceModel
    {
        [JsonProperty(Order = -1)]
        public string eventType { get; set; }

        public EventDTO()
        {
            id = Guid.NewGuid().ToString();
        }
    }

    //[JsonConverter(typeof(StringEnumConverter))]
    //public enum EventType
    //{
    //    COLONISATION,
    //    MOVE,
    //    PUNCTURE,
    //    NPC
    //}
}
