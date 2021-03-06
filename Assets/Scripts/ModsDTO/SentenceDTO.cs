using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Assets.Scripts.ModsDTO
{
    public class SentenceDTO : ConfigDTO<SentenceDTO>
    {
        public static string OBJECTIVE_WANNA_COLONIZE = "OBJECTIVE_WANNA_COLONIZE";
        public static string OBJECTIVE_WANNA_PUNCTURE = "OBJECTIVE_WANNA_PUNCTURE";
        public static string OBJECTIVE_WANNA_FIRECREW = "OBJECTIVE_WANNA_FIRECREW";
        public static string OBJECTIVE_WANNA_BUY_RIGGING = "OBJECTIVE_WANNA_BUY_RIGGING";
        public static string OBJECTIVE_WANNA_BUY_FOOD = "OBJECTIVE_WANNA_BUY_FOOD";
        public static string OBJECTIVE_WANNA_BUY_CREW = "OBJECTIVE_WANNA_BUY_CREW";
        public static string OBJECTIVE_NOTHING = "OBJECTIVE_NOTHING";

        public static string SOLUTION_FIRECREW_DONE = "SOLUTION_FIRECREW_DONE";
        public static string SOLUTION_GO_TO_ISLAND = "SOLUTION_GO_TO_ISLAND";
        public static string SOLUTION_BOUGHT_RIGGING = "SOLUTION_BOUGHT_RIGGING";
        public static string SOLUTION_BOUGHT_FOOD = "SOLUTION_BOUGHT_FOOD";
        public static string SOLUTION_HIRED_CREW = "SOLUTION_HIRED_CREW";
        public static string SOLUTION_COLONIZATION_DONE = "SOLUTION_COLONIZATION_DONE";
        public static string SOLUTION_PUNCTURE_DONE = "SOLUTION_PUNCTURE_DONE";

        public static string REALISATION_MOVE_DONE = "REALISATION_MOVE_DONE";

        public static string CURRENT_TURN_START = "CURRENT_TURN_START";
        public static string CURRENT_TURN_DETAIL = "CURRENT_TURN_DETAIL";

        public static string HISTORIC_FIRECREW_DONE = "HISTORIC_FIRECREW_DONE";
        public static string HISTORIC_COLONIZATION_DONE = "HISTORIC_COLONIZATION_DONE";
        public static string HISTORIC_PUNCTURE_DONE = "HISTORIC_PUNCTURE_DONE";
        public static string HISTORIC_MOVE_DONE = "HISTORIC_MOVE_DONE";

        public static string CARD_ID = "CARD_ID";
        public static string CARD_CHOICE_SHIPBOARD = "CARD_CHOICE_SHIPBOARD";

        public List<SentenceDTOObject> sentences { get; set; }
    }
   

    public class SentenceDTOObject
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}