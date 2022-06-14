using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class FlagsManager : UnityEngine.MonoBehaviour
    {
        [Header("Drapeaux")]
        public List<Texture2D> Flags;

        public static FlagsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public Texture2D GetRandomFlag()
        {
            return Flags[Random.Range(0, Flags.Count)];
        }

        public Color32 AverageColorFromTexture(Texture2D tex)
        {

            Color32[] texColors = tex.GetPixels32();

            int total = texColors.Length;

            float r = 0;
            float g = 0;
            float b = 0;

            for (int i = 0; i < total; i++)
            {

                r += texColors[i].r;
                g += texColors[i].g;
                b += texColors[i].b;
            }

            return new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);
        }

        public List<Color32> GetMainColorsFromTexture(Texture2D tex)
        {
            Color32[] texColors = tex.GetPixels32();
            Color32 actualColor;
            Dictionary<Color32, int> standings = new Dictionary<Color32, int>();

            int total = texColors.Length;

            for (int i = 0; i < total; i++)
            {
                actualColor = new Color32((byte)texColors[i].r, (byte)texColors[i].g, (byte)texColors[i].b, 0);
                if (!standings.ContainsKey(actualColor))
                    standings.Add(actualColor, 1);
                else
                    standings[actualColor] += 1;
            }

            return new List<Color32>(standings.OrderByDescending(x => x.Value).Take(3).Select(x=>x.Key));
        }
    }
}
