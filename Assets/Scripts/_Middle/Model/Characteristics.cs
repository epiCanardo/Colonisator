namespace ColanderSource
{
    /// <summary>
    /// enumère les différentes caractéristiques du npc
    /// </summary>
    public class Characteristics
    {
        // cractéristiques basiques
        public int CONSTITUTION { get; set; }
        public int MENTAL { get; set; }

        // traits de personnalité
        public int AMBITION { get; set; }
        public int VENALITE { get; set; }
        public int TEMERITE { get; set; }
        public int LEADERSHIP { get; set; }

        // compétences
        public int ARTILLERIE { get; set; }
        public int COMBAT { get; set; }
        public int NAVIGATION { get; set; }
        public int NEGOCIATION { get; set; }
        public int BATISSEUR { get; set; }
        public int MEDECINE { get; set; }
        public int CUISINE { get; set; }
        //public int ARTISANAT { get; set; }
        //public int INTENDANCE { get; set; }

        // tares
        public int ALCOOLISME { get; set; }
    }
}
