using System.Collections.Generic;

namespace ColanderSource
{
    public abstract class AbstractRuleResult
    {
        /// <summary>
        /// id de l'île concernée
        /// </summary>
        public string islandId { get; set; }

        /// <summary>
        /// la ressource concernée
        /// </summary>
        public string ressource { get; set; }

        /// <summary>
        /// la navire concerné (qui n'est pas le même que id en cours !)
        /// exemple : pour le navire fantôme, shipId correspond à la victime de la ponction de matelots
        /// </summary>
        public string shipId { get; set; }

        /// <summary>
        /// la quantité concernée
        /// </summary>
        public int quantiy { get; set; }

        /// <summary>
        /// la mouvement concerné
        /// </summary>
        public Move move { get; set; }

        /// <summary>
        /// la liste des npc concernés
        /// </summary>
        public List<string> npcs { get; set; }
    }

    public class RealisationRuleResult : AbstractRuleResult
    {
        /// <summary>
        /// la description textuelle de la réalisation
        /// </summary>
        public string realisationEnum { get; set; }
    }

    public class SolutionRuleResult : AbstractRuleResult
    {

        /// <summary>
        /// la description textuelle de la solution
        /// </summary>
        public string solutionEnum { get; set; }

    }

    public class ObjectiveRuleResult
    {
        /// <summary>
        /// la description textuelle de l'objectif
        /// </summary>
        public string objectiveEnum { get; set; }
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
