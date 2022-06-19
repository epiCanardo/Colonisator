using Assets.Scripts.ModsDTO;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// la table de bord du navire
    /// </summary>
    public class ShipBoard : Board
    {
        /// <summary>
        /// la coque
        /// </summary>
        public int hull { get; set; }

        /// <summary>
        /// les canons
        /// </summary>
        public List<Cannon> cannons { get; set; }
        
        /// <summary>
        /// les munitions (type et quantité)
        /// </summary>
        public Dictionary<CannonBall, int> ammo { get; set; }

        /// <summary>
        /// les barils de poudre 
        /// </summary>
        public int powderBarrels { get; set; }

        /// <summary>
        /// utilisé pour la table de bord, hors canons et ammo
        /// </summary>
        /// <param name="delta"></param>
        public void UpdateBoardWithDelta(ShipBoard delta)
        {
            dodris += delta.dodris;
            food += delta.food;
            order += delta.order;
            rigging += delta.rigging;
            hull += delta.hull;
            powderBarrels += delta.powderBarrels;
        }

        public string TextFromShipBoardDelta(ShipBoard delta)
        {
            List<string> items = new List<string>();
            if (delta!= null)
            {
                if (delta.dodris != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("dodris"), dodris, delta.dodris, 0, 100000));
                if (delta.food != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("food"), food, delta.food, 0, 100));
                if (delta.order != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("order"), order, delta.order, 0, 100));
                if (delta.rigging != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("rigging"), rigging, delta.rigging, 0, 100));
                if (delta.hull != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("hull"), hull, delta.hull, 0, 100));
                if (delta.cannons != null)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("cannons"), cannons.Count, delta.cannons.Count, 0, 20));
                if (delta.powderBarrels != 0)
                    items.Add(TextFromValue(ModManager.Instance.GetPropertyLabel("powderBarrels"), powderBarrels, delta.powderBarrels, 0, 100));

            }
            return string.Join(",", items);
        }

        private string TextFromValue(string name, int currentValue, int deltaValue, int minValue, int maxValue)
        {
            string colorText = "<color=blue>";
            if (deltaValue < 0)
                colorText = "<color=red>";

            return $"{name} : {colorText}{currentValue} -> {Mathf.Clamp(currentValue + deltaValue, minValue, maxValue)}</color>";
        }
    }
}