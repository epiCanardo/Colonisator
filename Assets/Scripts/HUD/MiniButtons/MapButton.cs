using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

namespace Colfront.GamePlay
{
    public class MapButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public void OnPointerEnter(PointerEventData eventData)
        {
            //transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetRelative(true).SetEase(Ease.Linear);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //transform.DOKill(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            GameManager.Instance.ToggleMap(true);
        }
    }
}
