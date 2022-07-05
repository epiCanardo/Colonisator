using Assets.Scripts.Front.MainManagers;
using Assets.Scripts.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Front.BarActions
{
    public class MoveShipAction : UnityEngine.MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject dialog;
        public TextMeshProUGUI text;
        public Tooltip tooltip;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (TurnManager.Instance.MainState == TurnState.Human)
            {
                // si le mode navigation n'est pas actif, on le lance
                if (!GameManager.Instance.IsNavigationModeActive())
                {              
                    // positionnement spécifique pour ce mode de déplacement
                   // GameManager.Instance.camOffSet = new Vector3(0, 1000, 0);
                    //GameManager.Instance.camEulerAngles = new Vector3(90, 0, 0); ;
                    GameManager.Instance.FocusCamOnShip(GameManager.Instance.GetActualPlayinghipObject);

                    Ship ship = GameManager.Instance.GetActualPlayinghipObject.GetComponent<ShipManager>().ship;     

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
            tooltip.gameObject.SetActive(true);
            tooltip.SetText("Vous avancez votre navire");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltip.gameObject.SetActive(false);
        }
    }
}
