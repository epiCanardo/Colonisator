using System.Collections.Generic;

namespace Assets.Scripts.ModsDTO
{
    public class MapDefinitionDTO : ConfigDTO<MapDefinitionDTO>
    {
        public List<Harbor> harbors { get; set; }
        public List<int[]> nonNavigableSquares { get; set; }

        public class Harbor
        {
            public string islandName { get; set; }
            public List<int> coordinates { get; set; }
            public List<List<int>> costalSquares { get; set; }
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