using ColanderSource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Colfront.GamePlay
{
    public class FactionsManager : MonoBehaviour
    {
        public static FactionsManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null) { Instance = this; Factions = new List<FactionManager>(); };
        }

        public List<FactionManager> Factions { get; set; }

        //public Dictionary<Faction, Texture2D> DicoFlags { get; private set; }
    }

    public class FactionManager
    {
        public Faction Faction { get; set; }

        public Texture2D Flag { get; set; }

        public List<Color32> Colors { get; set; }

        public void SetFactionFlag(List<Color32> colors)
        {
            Flag = FlagsManager.Instance.GetRandomFlag();
        }
    }
}
