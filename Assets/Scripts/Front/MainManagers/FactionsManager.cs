using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class FactionsManager : UnityEngine.MonoBehaviour
    {
        public static FactionsManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null) { Instance = this; Factions = new List<FactionManager>();};
        }

        public List<FactionManager> Factions { get; set; }

        public FactionManager GetFactionManager(Faction faction)
        {
            return Factions.First(x => x.Faction.Equals(faction));
        }

        public FactionManager GetFactionManager(string playerTypeEnumKey)
        {
            return Factions.First(x => x.Faction.playerTypeEnum.Equals(playerTypeEnumKey));
        }
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
