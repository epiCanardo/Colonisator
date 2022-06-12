using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Colfront.GamePlay
{
    public class MoveShipAction : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject dialog;
        public TextMeshProUGUI text;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (TurnManager.Instance.MainState == TurnState.Human)
            {
                // si le mode navigation n'est pas actif, on le lance
                if (!GameManager.Instance.IsNavigationModeActive())
                {
                    var ship = GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().ship;
                    GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().PauseSwing();

                    NavigationModeManager navMode = NavigationModeManager.Instance;
                    navMode.StartNavigationMode(ship);
                    navMode.squaresRemaning = Random.Range(2, 13);
                    navMode.windDirection = Random.Range(1, 5);
                    navMode.UpdateMoveText();

                    // affichage de la dialog de suivi du mouvement
                    dialog.SetActive(true);

                    // activation des cases dans la scene principale
                    GameManager.Instance.ToggleNavigationMode(true);
                }
            }
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
