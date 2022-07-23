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
                case "maroon":
                    ColorUtility.TryParseHtmlString("#800000", out color);
                    break;
                case "purple":
                    ColorUtility.TryParseHtmlString("#800080", out color);
                    break;
                case "yellow":
                    return Color.yellow;
                case "green":
                    return Color.green;
                case "red":
                    return Color.red;
                default:
                    break;
            }
            
            return color;
        }
    }
}
