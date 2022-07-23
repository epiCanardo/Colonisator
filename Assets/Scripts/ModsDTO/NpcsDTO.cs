using System.Collections.Generic;

namespace Assets.Scripts.ModsDTO
{
    public class NpcsDTO : ConfigDTO<NpcsDTO>
    {
        public List<string> firstNames_f { get; set; }
        public List<string> firstNames_m { get; set; }
        public List<string> lastNames { get; set; }       
    }
}