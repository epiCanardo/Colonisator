using System;

namespace ColanderSource
{
    /// <summary>
    /// la table de bord du navire
    /// </summary>
    public class IslandBoard : Board
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        public int defenceLevel { get; set; }
        
        /// <summary>
        /// constructeur par défaut (essentiellement pour les test)
        /// </summary>
        public IslandBoard()
        {
            dodris = rnd.Next(0, 10000);
            food = rnd.Next(0, 100);
            rigging = rnd.Next(0, 100);
            order = 100;
            defenceLevel = 1;            
        }
    }
}
