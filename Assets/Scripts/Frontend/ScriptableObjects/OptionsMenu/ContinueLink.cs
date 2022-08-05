using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.OptionsMenu
{
    public class ContinueLink : LinkObject
    {
        public GameObject optionsmenu;

        public override void OnPointerDown(PointerEventData eventData)
        {
            MenusManager.Instance.TryDestroyMenu("systemOptions");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                MenusManager.Instance.TryDestroyMenu("systemOptions");
        }
    }
}