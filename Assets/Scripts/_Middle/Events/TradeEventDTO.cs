using System.Collections.Generic;
using Newtonsoft.Json;

namespace ColanderSource
{
    public class TradeEventDTO : EventDTO
    {
        /// <summary>
        /// le navire à l'origine du trade. la classe est instanciée avec uniquement un ShipBoard à jour (optimisation)
        /// </summary>
        [JsonProperty(Order = 0)]        
        public Ship shipDelta { get; set; }

        /// <summary>
        /// l'île cible du trade
        /// </summary>
        [JsonProperty(Order = 0)]
        public string islandId { get; set; }

        /// <summary>
        /// la liste des ventes réalisées
        /// une ressource = une vente
        /// </summary>
        [JsonProperty(Order = 0)]
        public List<TradeLine> sells { get; set; }
        
        /// <summary>
        /// la liste des achats réalisés
        /// une ressource = un achat
        /// </summary>        
        [JsonProperty(Order = 0)]
        public List<TradeLine> buys { get; set; }

        /// <summary>
        /// la liste des npcs qui sont débarqués
        /// </summary>
        [JsonProperty(Order = 0)]
        public List<string> landingNpcs { get; set; }
        
        /// <summary>
        /// la liste des npcs qui embarquent
        /// </summary>
        [JsonProperty(Order = 0)]
        public List<string> boardingNpcs { get; set; }

        /// <summary>
        /// le différentiel du stuff échangé => pas envoyé au back, sert uniquement en interne
        /// </summary>
        [JsonIgnore]
        public ShipBoard deltaStuff { get; set; }

        public TradeEventDTO()
        {
            eventType = "TRADE";
        }
    }

    public class TradeLine
    {
        /// <summary>
        /// la ressource (barils de vivres, gréément, chanvre, bois, boulets etc..)
        /// </summary>
        public string ressource { get; set; }

        /// <summary>
        /// la quantité concernée en unité
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// coût du trade
        /// </summary>
        public int cost { get; set; }
    }
}
