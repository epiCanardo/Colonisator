using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.HUD.MiniButtons
{
    public class HistoricPanelToggle : UnityEngine.MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public GameObject historicPanel;
        public GameObject historicPanelButtons;

        private Vector3 historicPanelSourcePosition;
        private Vector3 historicPanelButtonsSourcePosition;

        private bool collapsed;

        private void Start()
        {
            historicPanelSourcePosition = historicPanel.transform.position;
            historicPanelButtonsSourcePosition = historicPanelButtons.transform.position;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
           // historicPanel.transform.DOScaleX(0, 1);
           // historicPanelButtons.transform.DOMoveX(0, 1);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
           // historicPanel.transform.DOScaleX(1, 1);
           //historicPanelButtons.transform.DOMove(historicPanelButtonsSourcePosition, 1);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (collapsed)
            {
                historicPanel.transform.localScale = new Vector3(1, 1, 1);
                historicPanelButtons.transform.position = historicPanelButtonsSourcePosition;

               // historicPanel.transform.DOScaleX(1, 1);
               // historicPanelButtons.transform.loca DOLocalMoveX(historicPanelButtonsSourcePosition, 1);
                collapsed = false;
            }
            else
            {
                historicPanel.transform.localScale = Vector3.zero;
                historicPanelButtons.transform.localPosition += Vector3.right * 300;

                //historicPanel.transform.DOScaleX(0, 1);
                //historicPanelButtons.transform.DOLocalMoveX(0, 1);
                collapsed = true;
            }
        }
    }
}
