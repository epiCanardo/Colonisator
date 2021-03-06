using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ColorTools
    {
        public static Color NameToColor(string colorName)
        {
            Color color = new Color();

            switch (colorName)
            {
                case "lime":
                    ColorUtility.TryParseHtmlString("00FF00", out color);
                    break;
                case "navy":
                    ColorUtility.TryParseHtmlString("#000080", out color);
                    break;
                case "olive":
                    ColorUtility.TryParseHtmlString("#808000", out color);
                    break;
                case "silver":
                    ColorUtility.TryParseHtmlString("#C0C0C0", out color);
                    break;
                case "teal":
                    ColorUtility.TryParseHtmlString("#008080", out color);
                    break;
                default:
                    break;
            }
            
            return color;
        }
    }
}
