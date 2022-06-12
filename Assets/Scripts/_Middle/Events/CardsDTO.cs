﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace ColanderSource
{
    /// <summary>
    /// classe de stockage des cartes (envoyées en une fois en début de partie)
    /// il contient une liste d'évènements
    /// </summary>
    public class CardsDTO
    {
        public List<Card> cards { get; set; }

        public CardsDTO()
        {
            cards = new List<Card>();
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
