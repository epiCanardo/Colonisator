using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public class FactionsLink : LinkObject
    {        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetInstance.GetComponent<FactionsBoard>().ShowBoard(x => x.longName);
        }
    }
}