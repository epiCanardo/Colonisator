using Newtonsoft.Json;
using System.Collections.Generic;

namespace Assets.Scripts.DTO
{
    /// <summary>
    /// pour la création d'une partie
    /// </summary>
    public class NewGameDTO
    {
        public int nbNpc { get; set; }
        public List<NewGameFaction> factions { get; set; }

        public class NewGameFaction
        {
            public string playerTypeEnum { get; set; }
            public string name { get; set; }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}