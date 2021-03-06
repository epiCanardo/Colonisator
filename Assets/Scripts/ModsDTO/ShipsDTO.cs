using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class ShipsDTO : ConfigDTO<ShipsDTO>
    {
        public List<string> shipNames { get; set; }
        public List<ShipClass> shipClasses { get; set; }

        public class ShipClass
        {
            public string shipTypeEnum { get; set; }
            public string className { get; set; }
        }
    }
}