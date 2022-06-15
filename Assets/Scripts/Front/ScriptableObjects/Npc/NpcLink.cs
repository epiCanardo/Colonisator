using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.Service;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Npc
{
    public class NpcLink : LinkObject
    {
        public Model.Npc npc;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetInstance.GetComponent<NpcDetailBoard>().npc = npc;
        }
    }
}