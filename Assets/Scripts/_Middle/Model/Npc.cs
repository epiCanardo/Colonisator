using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// classe Island
    /// </summary>
    public class Npc : ColanderSourceModel
    {
        public string faction { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string rankEnum { get; set; }
        public int age { get; set; }
        public int healthState { get; set; }
        public int money { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public string sex { get; set; }
        public string fullName => $"{name} {surname}";
        public Dictionary<string, int> loyalties { get; set; }
        public string description { get; set; }
        public string aspirationEnum { get; set; }
        public Characteristics characteristics { get; set; }
        public string currentIsland { get; set; }
        public string currentShip { get; set; }

        public override string ToString()
        {
            return id;
        }
    }

    public enum Rank
    {
        NONE,
        PENDING,
        SAILOR,
        OFFICER,
        GOVERNOR,
        CAPTAIN,
        BOSS
    }

    public enum Aspiration
    {
        NONE,
        MONEY,
        COLONIZATION,
        FACTION,
        ECOLO,
        ALTERMONDIALIST,
        PEACE
    }
}