using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColanderSource;
using UnityEditor.PackageManager.Requests;

namespace Colfront.GamePlay
{
    public class ModManager
    {
        private static string activeMod = "Core";
        private Dictionary<Objective, string> objectives;
        private static List<string> activeCards = new List<string>();

        public static ModManager Instance { get; private set; }

        public ModManager()
        {
            if (Instance == null)
                Instance = this;
        }

        public static void GetCards()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string txtName in Directory.GetFiles($"Mods/{activeMod}/Cards/", " *.json"))
            {
                using (StreamReader sr = new StreamReader(txtName))
                {
                    activeCards.Add(sr.ReadToEnd());
                }
            }
        }
    }

    public enum Objective
    {
        OBJECTIVE_WANNA_COLONIZE,
        OBJECTIVE_WANNA_PUNCTURE,
        OBJECTIVE_WANNA_FIRECREW,
        OBJECTIVE_WANNA_BUY_RIGGING,
        OBJECTIVE_WANNA_BUY_FOOD,
        OBJECTIVE_WANNA_BUY_CREW,
        OBJECTIVE_NOTHING
    }
}