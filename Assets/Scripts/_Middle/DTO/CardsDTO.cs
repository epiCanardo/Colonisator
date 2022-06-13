using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace ColanderSource
{
    /// <summary>
    /// classe de stockage des cartes (envoyées en une fois en début de partie)
    /// </summary>
    public class CardsDTO
    {
        public List<string> cards { get; set; }

        public CardsDTO(List<string> sourceCards)
        {
            cards = sourceCards;
        }

        /// <summary>
        /// conversion en format json interpétable par le back
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("[");
            sb.AppendLine(string.Join(",", cards));
            sb.AppendLine("]");

            return sb.ToString();
        }
    }
}
