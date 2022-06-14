using System.Collections.Generic;

namespace Assets.Scripts.DTO
{
    public class PunctureDTO
    {
        public List<string> npcIds { get; set; }
        public string sourceShipId { get; set; }
        public string targetShipId { get; set; }
    }
}
