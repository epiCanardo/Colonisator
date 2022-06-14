using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Faction
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