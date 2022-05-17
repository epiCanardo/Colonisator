using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// correspond aux actions à effectuer pour un navire en début de tour
    /// id correspond à l'id du navire
    /// </summary>
    public class ShipTurnAction : ColanderSourceModel
    {
        /// <summary>
        /// l'indicateur de vent pour ce tour
        /// </summary>
        public Wind wind { get; set; }

        /// <summary>
        /// la description de l'objectif de tour du navire
        /// </summary>
        public ObjectiveRuleResult objectiveRuleResult { get; set; }

        /// <summary>
        /// la description de la solution à appliquer pour atteindre l'objectif
        /// </summary>
        public SolutionRuleResult solutionRuleResult { get; set; }

        /// <summary>
        /// la description de la réalisation de la solution à appliquer pour atteindre l'objectif
        /// </summary>
        public RealisationRuleResult realisationRuleResult { get; set; }
    }
}