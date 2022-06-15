using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Ancestors
{
    public abstract class UIBoard : UnityEngine.MonoBehaviour, IPointerDownHandler
    {        
        public abstract string key { get; }

        // sur le clic, passe en premier plan
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
        }
    }
}