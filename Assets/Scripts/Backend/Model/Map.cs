using System.Collections.Generic;
using System.Numerics;

namespace Assets.Scripts.Model
{
    public class Map
    {
        public List<Square> Squares { get; set; }

        /// <summary>
        /// liste des ports
        /// </summary>
        public List<Vector2> Harbors { get; set; }

        public Map()
        {
            Squares = new List<Square>();
            Harbors = new List<Vector2>
            {
                new Vector2(5,96),
                new Vector2(97,3),
                new Vector2(94,37),
                new Vector2(27,69),
                new Vector2(53,54),
                new Vector2(77,22),
                new Vector2(96,97),
                new Vector2(2,2),
                new Vector2(47,90),
                new Vector2(87,77),
                new Vector2(7,44),
                new Vector2(13,18),
                new Vector2(33,28),
                new Vector2(77,52),
                new Vector2(70,96)
            };
        }
    }
}
