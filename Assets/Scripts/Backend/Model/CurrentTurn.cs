using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// correpond à la liste des factions et des navires pour le tour courant (sauf l'humain)
    /// avec les actions attendues
    /// </summary>
    public class CurrentTurn
    {
        public int number { get; set; }

        /// <summary>
        /// factionsAndShips
        /// clé : l'id de la faction
        /// valeur : une liste correspondant à chaque navire présent dans la faction
        /// </summary>
        public Dictionary<string, List<ShipTurnAction>> factionsAndShips { get; set; }
    }
}
