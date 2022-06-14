using Newtonsoft.Json;

namespace Assets.Scripts.Model
{
    public abstract class ColanderSourceModel
    {
        [JsonProperty(Order = -2)]
        public string id { get; set; }
    }
}
