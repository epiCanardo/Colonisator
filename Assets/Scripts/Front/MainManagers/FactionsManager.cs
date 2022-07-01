using System.Collections.Generic;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class FactionsManager : UnityEngine.MonoBehaviour
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

        public bool IsPlaying { get; set; }

        public Texture2D Flag { get; set; }

        public List<Color32> Colors { get; set; }

        public string MainColor { get; set; }

        public void SetFactionFlag(List<Color32> colors)
        {
            Flag = FlagsManager.Instance.GetRandomFlag();
        }
    }
}
