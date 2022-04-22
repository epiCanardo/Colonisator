using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Colfront.GamePlay
{

    public class LaunchColonisation : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {
        public TextMeshProUGUI text;
        private Colonisation colonisation;

        private void Start()
        {
            colonisation = FindObjectOfType<Colonisation>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            text.DOFaceColor(Color.red, 0.5f);
            text.DOScale(2, 0.5f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            text.DOFaceColor(Color.white, 0.5f);
            text.DOScale(1, 0.5f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            colonisation.DoColonize();
        }
    }
}