using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class MainConfigDTO : ConfigDTO<MainConfigDTO>
    {       
        public string language { get; set; }
        public string version { get; set; }
        public List<string> activeMods { get; set; }
        
    }
}