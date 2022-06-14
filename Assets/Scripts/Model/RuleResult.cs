using System.Collections.Generic;

namespace Assets.Scripts.Model
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
        public int quantity { get; set; }

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

        public const string GO_TO_ISLAND = "GO_TO_ISLAND";
        public const string BUY = "BUY";
        public const string PUNCTURE_CREW = "PUNCTURE_CREW";
        public const string REFOURGUER_CREW = "REFOURGUER_CREW";
        public const string COLONIZE = "COLONIZE";
    }

    public class ObjectiveRuleResult
    {
        /// <summary>
        /// la description textuelle de l'objectif
        /// </summary>
        public string objectiveEnum { get; set; }

        public const string COLONIZE_ISLAND = "COLONIZE_ISLAND";
        public const string PUNCTURE_CREW = "PUNCTURE_CREW";
        public const string REFOURGUER_CREW = "REFOURGUER_CREW";
        public const string GET_RIGGING = "GET_RIGGING";
        public const string GET_FOOD = "GET_FOOD";
        public const string GET_CREW = "GET_CREW";
    }

    public enum RealisationsEnum
    {
        MOVE,
        GET_SAILORS,
        REFOURGUER_MATELOTS,
        COLONIZE
    }
}
