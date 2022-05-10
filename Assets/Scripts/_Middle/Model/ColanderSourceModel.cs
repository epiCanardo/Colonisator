using Newtonsoft.Json;

namespace ColanderSource
{
    public abstract class ColanderSourceModel
    {
        [JsonProperty(Order = -2)]
        public string id { get; set; }
    }
}
