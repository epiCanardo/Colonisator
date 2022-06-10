using Newtonsoft.Json;
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

        [JsonIgnore]
        public string EtatSante
        {
            get
            {
                if (healthState > 75)
                    return $"Excellent ({healthState})";
                else if (healthState > 50)
                    return $"Correct ({healthState})";
                else if (healthState > 25)
                    return $"Mauvais ({healthState})";
                else if (healthState > 0)
                    return $"Exécrable ({healthState})";
                return "Mort";
            }
        }

        public int money { get; set; }
        public int size { get; set; }
        public int weight { get; set; }
        public string sexEnum { get; set; }

        [JsonIgnore]
        public string Rang
        {
            get
            {
                switch (rankEnum)
                {
                    case "NONE":
                        return "Aucun";
                        break;
                    case "PENDING":
                        return "En attente de libération";
                        break;
                    case "SAILOR":
                        return "Marin";
                        break;
                    case "OFFICER":
                        return "Officier de marine";
                        break;
                    case "GOVERNOR":
                        return "Gouverneur d'île";
                        break;
                    case "CAPTAIN":
                        return "Capitaine de navire";
                        break;
                    case "BOSS":
                        return "Boss de faction";
                        break;
                    default:
                        return "Pas de rang";
                        break;
                }
            }
        }

        [JsonIgnore]
        public string Sexe
        {
            get
            {
                return sexEnum.Equals("M") ? "Homme" : "Femme";
            }
        }
        public string fullName => $"{name} {surname}";
        public Dictionary<string, int> loyalties { get; set; }

        [JsonIgnore]
        public string FactionLotalty
        {
            get
            {
                if (loyalties.ContainsKey(faction))
                {
                    int loyalty = loyalties[faction];

                    if (loyalties[faction] > 75)
                        return $"Excellente ({loyalty})";
                    if (loyalties[faction] > 50)
                        return $"Correcte ({loyalty})";
                    if (loyalties[faction] > 25)
                        return $"Mauvaise ({loyalty})";

                    return $"Execrable ({loyalty})";
                }
                return "N'a pas de loyauté envers cette faction.";

            }
        }
        public string description { get; set; }

        [JsonIgnore]
        public string aspirationEnum { get; set; }

        public string Aspiration
        {
            get
            {
                switch (aspirationEnum)
                {
                    case "NONE":
                        return "Aucune";
                        break;
                    case "MONEY":
                        return "Être riche !";
                        break;
                    case "COLONIZATION":
                        return "Coloniser toutes les îles !";
                        break;
                    case "FACTION":
                        return "Être le boss de ma faction !";
                        break;
                    case "ECOLO":
                        return "Être écolo";
                        break;
                    case "ALTERMONDIALIST":
                        return "Combattre le système !";
                        break;
                    case "PEACE":
                        return "Oeuvrer pour la paix !";
                        break;
                    default:
                        return "BALEK";
                        break;
                }
            }
        }

        public Characteristics characteristics { get; set; }
        public string currentIsland { get; set; }
        public string currentShip { get; set; }

        [JsonIgnore]
        public string Localisation
        {
            get
            {
                if (currentIsland != null)
                    return $"Sur l'île : {ServiceGame.GetIslandFromId(currentIsland).name}";
                if (currentShip != null)
                    return $"Sur le navire : {ServiceGame.GetShip(currentShip).name}";
                return "Porté disparu";
            }
        }


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