using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class PropertiesDTO : ConfigDTO<PropertiesDTO>
    {
        public List<PropertiesDTOObject> boards { get; set; }

        public class PropertiesDTOObject
        {
            public string key { get; set; }
            public string value { get; set; }
        }
    }
}