using System;
using Assets.Scripts.Model;
using Newtonsoft.Json;

namespace Assets.Scripts.EventsDTO
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
