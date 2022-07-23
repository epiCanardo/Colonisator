using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using Assets.Scripts.ModsDTO;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.OptionsMenu
{
    public class ReloadConfigLink : LinkObject
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            ModManager.Instance.LoadMainConfig();
            MenusManager.Instance.TryDestroyMenu("systemOptions");
        }
    }
}