using System.Collections.Generic;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// la carte
    /// </summary>
    public class Card : ColanderSourceModel
    {
        public List<string> conditions { get; set; } // liste des clés de conditions
        public string title { get; set; } // texte d'affichage haut (résumé)
        public string description { get; set; } // description complète
        public int priority { get; set; } // priorité de la carte : 0, 1 ou 2
        public List<CardChoice> choices { get; set; }
    }

    public class CardChoice
    {
        public string label { get; set; } // descrpition du choix
        public string shipId { get; set; } // pour déterminer le navire
        public ShipBoard shipBoardDelta { get; set; } // les conséquences sur le shipboard
        public int shipCrewDelta { get; set; } // les conséquences sur le nombre de matelots
        public string islandId { get; set; } // pour déterminer l'île
        public IslandBoard islandBoardDelta { get; set; } // les conséquences sur le islandboard
        public int islandCrewDelta { get; set; } // les conséquences sur le nombre de matelots
    }
}