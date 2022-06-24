using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class MapDTO : ConfigDTO<MapDTO>
    {
        public List<Harbor> harbors { get; set; }
        public List<int[]> nonNavigableSquares { get; set; }

        public class Harbor
        {
            public string islandName { get; set; }
            public List<int> coordinates { get; set; }
        }

        public bool IsNonNavigable(int[] square)
        {
            foreach (var item in nonNavigableSquares)
            {
                if (item[0] == square[0] && item[1] == square[1])
                    return true;
            }

            return false;
        }
    }
}