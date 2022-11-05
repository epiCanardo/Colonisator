using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    public class Move
    {
        public List<MoveDetails> moveDetails { get; set; }
        public int cost { get; set; }
    }
}