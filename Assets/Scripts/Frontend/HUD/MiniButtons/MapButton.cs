using Assets.Scripts.Front.Cams;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD.MiniButtons
{
    public class MapButton : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
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
            Camera.main.GetComponent<CamMovement>().SetCamToMapLevel();
        }
    }
}
