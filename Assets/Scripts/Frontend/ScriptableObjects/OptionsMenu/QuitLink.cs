using Assets.Scripts.Front.ScriptableObjects.Ancestors;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.OptionsMenu
{
    public class QuitLink : LinkObject, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
           // Destroy(optionsmenu);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Application.Quit();
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }
}