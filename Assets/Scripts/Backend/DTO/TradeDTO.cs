using System.Collections.Generic;
using Assets.Scripts.EventsDTO;
using Assets.Scripts.Model;

namespace Assets.Scripts.DTO
{
    public class TradeDTO
    {
        public Ship ship { get; set; }
        public Island island { get; set; }
        public List<Npc> landingNpcs { get; set; }
        public List<Npc> boardingNpcs { get; set; }
        public List<TradeLine> sells { get; set; }
        public List<TradeLine> buys { get; set; }
        public ShipBoard deltaStuff { get; set; }
    }
}