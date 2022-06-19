using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class IslandsDTO : ConfigDTO<IslandsDTO>
    {
        public List<string> islandNames { get; set; }
        
    }
}