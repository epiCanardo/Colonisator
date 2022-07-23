using System.Collections.Generic;

namespace Assets.Scripts.ModsDTO
{
    public class MainConfigDTO : ConfigDTO<MainConfigDTO>
    {       
        public string language { get; set; }
        public string version { get; set; }
        public List<string> activeMods { get; set; }
        public bool mouseScrollInverted { get; set; }
        public float mouseSensibility { get; set; }
        public int globalQuality { get; set; }
        public string colbackLocation { get; set; }        
    }
}