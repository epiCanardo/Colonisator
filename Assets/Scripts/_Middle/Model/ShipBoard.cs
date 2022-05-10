using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// la table de bord du navire
    /// </summary>
    public class ShipBoard : Board
    {
        /// <summary>
        /// le gréément (mâts, voiles, cordages..)
        /// </summary>
        public int rigging { get; set; }

        /// <summary>
        /// la coque
        /// </summary>
        public int hull { get; set; }

        /// <summary>
        /// les canons
        /// </summary>
        public List<Cannon> cannons { get; set; }
        
        /// <summary>
        /// les munitions (type et quantité)
        /// </summary>
        public Dictionary<CannonBall, int> ammo { get; set; }

        /// <summary>
        /// les barils de poudre 
        /// </summary>
        public int powderBarrels { get; set; }
    }
}