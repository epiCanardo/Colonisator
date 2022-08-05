using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Faction
{
    public class FactionLink : LinkObject
    {
        public Model.Faction faction;
        
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            targetInstance.GetComponent<FactionDetailBoard>().faction = faction;
        }
    }
}