using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// classe Ship
    /// </summary>
    public sealed class Ship : ColanderSourceModel, IEquatable<Ship>
    {
        public string shipTypeEnum { get; set; }
        public string name { get; set; }
        public string owner { get; set; }
        public List<string> crew { get; set; }
        public Square coordinates { get; set; }
        public ShipBoard shipBoard { get; set; }

        public Ship()
        {
            shipBoard = new ShipBoard();
        }

        public bool Equals(Ship other)
        {
            return id.Equals(other.id);
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShipType
    {
        PINNACE,
        FLOATING_TRASH_CAN,
        SLOOP,
        FRIGATE,
        GALLEON
    }
}