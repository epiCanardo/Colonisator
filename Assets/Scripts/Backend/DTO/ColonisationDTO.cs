using System.Collections.Generic;
using Assets.Scripts.Model;

namespace Assets.Scripts.DTO
{
    public class ColonisationDTO
    {
        /// <summary>
        /// l'île à coloniser
        /// </summary>
        public Island island { get; set; }

        /// <summary>
        /// le navrie qui colonise
        /// </summary>
        public Ship ship { get; set; }

        /// <summary>
        /// les npcs qui bougent sur l'île
        /// </summary>
        public List<Npc> npcs { get; set; }

        /// <summary>
        /// la quantité de barils de vivres qui bougent sur l'île
        /// </summary>
        public int food { get; set; }

        /// <summary>
        /// la quantité d'ordre que la colonisation génère à bord
        /// </summary>
        public int order { get; set; }
    }
}