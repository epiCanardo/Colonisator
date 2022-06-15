using Assets.Scripts.Front.MainManagers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD.MiniButtons
{
    public class OptionsButton : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public GameObject optionsMenu;

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetRelative(true).SetEase(Ease.Linear);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOKill(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            MenuCreation();
        }

        private void MenuCreation()
        {
            //Instantiate(optionsMenu, GameManager.Instance.canvas.transform).SetAsLastSibling();
            MenusManager.Instance.TryOpenMenu("systemOptions", optionsMenu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))            
                MenuCreation();            
        }
    }
}
