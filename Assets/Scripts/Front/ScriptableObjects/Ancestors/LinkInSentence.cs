﻿using TMPro;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.ScriptableObjects.Ancestors
{
    public class LinkInSentence : UnityEngine.MonoBehaviour, IPointerDownHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            TextMeshProUGUI pTextMeshPro = GetComponent<TextMeshProUGUI>();
            int linkIndex =
                TMP_TextUtilities.FindIntersectingLink(pTextMeshPro, eventData.position,
                    null); // If you are not in a Canvas using Screen Overlay, put your camera instead of null
            if (linkIndex != -1)
            {
                // was a link clicked?
                TMP_LinkInfo linkInfo = pTextMeshPro.textInfo.linkInfo[linkIndex];
                pTextMeshPro.text = pTextMeshPro.text.Replace(linkInfo.GetLinkText(), $"<color=\"blue\">{linkInfo.GetLinkText()}</color>");
            }
        }
    }
}