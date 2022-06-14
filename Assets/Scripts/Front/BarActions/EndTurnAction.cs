using Assets.Scripts.Front.MainManagers;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.BarActions
{
    public class EndTurnAction : UnityEngine.MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (TurnManager.Instance.MainState == TurnState.WaitForEndTurn)
                TurnManager.Instance.MainState = TurnState.ActionsFinished;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //if (TurnManager.Instance.MainState == TurnState.WaitForEndTurn)
           //     TurnManager.Instance.BounceButton();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
           // TurnManager.Instance.StopBouncing();
        }
    }
}
