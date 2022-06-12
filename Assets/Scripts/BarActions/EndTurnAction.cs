using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Colfront.GamePlay
{
    public class EndTurnAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
