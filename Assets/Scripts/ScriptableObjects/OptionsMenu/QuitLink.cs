using ColanderSource;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Colfront.GamePlay
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