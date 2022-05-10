using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// correspond aux actions à effectuer pour un navire en début de tour
    /// l'id correspond à l'id du navire
    /// </summary>
    public class ShipTurnAction : ColanderSourceModel
    {
        public Wind wind { get; set; }
        public Move move { get; set; }

        /// <summary>
        /// dédié pour le navire fantôme, null pour les autres
        /// </summary>
        public Puncture puncture { get; set; }

        /// <summary>
        /// l'objectif à réaliser
        /// </summary>
        public string objective { get; set; }

        /// <summary>
        /// la solution choisie pour réaliser cet objectif
        /// </summary>
        public string solution { get; set; }

        /// <summary>
        /// l'action concrète pour réaliaser l'objectif
        /// </summary>
        public string realisation { get; set; }
    }

    public enum Objective
    {
        // Objectifs liés à l'aspiration
        COLONIZE_ISLAND,
        // Vampiriser un navire (uniquement pour le navire fantôme)
        PUNCTURE_CREW,
        // Refourguer des matelots
        REFOURGUER_CREW,
        // Objectifs liés au besoin
        GET_RIGGING
    }

    public enum Solution
    {
        GO_TO_ISLAND,
        BUY,
        PUNCTURE_CREW,
        REFOURGUER_CREW,
        COLONIZE
    }

    public enum Realisation
    {
        MOVE,
        GET_SAILORS,
        REFOURGUER_MATELOTS,
        COLONIZE
    }
}
