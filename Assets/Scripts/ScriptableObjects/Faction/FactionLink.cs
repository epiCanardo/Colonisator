using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public class FactionLink : LinkObject
    {
        public Faction faction;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetInstance.GetComponent<FactionDetailBoard>().faction = faction;
        }
    }
}