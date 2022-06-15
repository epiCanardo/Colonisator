﻿using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// la table de bord du navire
    /// </summary>
    public class ShipBoard : Board
    {
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

        /// <summary>
        /// utilisé pour la table de bord, hors canons et ammo
        /// </summary>
        /// <param name="delta"></param>
        public void UpdateBoardWithDelta(ShipBoard delta)
        {
            dodris += delta.dodris;
            food += delta.food;
            order += delta.order;
            rigging += delta.rigging;
            hull += delta.hull;
            powderBarrels += delta.powderBarrels;
        }
    }
}