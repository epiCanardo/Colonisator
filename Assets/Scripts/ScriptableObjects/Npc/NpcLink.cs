using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public class NpcLink : LinkObject
    {
        public Npc npc;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetInstance.GetComponent<NpcDetailBoard>().npc = npc;
        }
    }
}