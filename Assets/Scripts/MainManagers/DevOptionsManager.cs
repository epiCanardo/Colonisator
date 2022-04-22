using UnityEngine;
using UnityEngine.UI;

namespace Colfront.GamePlay
{
    public class DevOptionsManager : MonoBehaviour
    {
        [Header("Les options de dev")]
        public Button launchTurnButton;
        public Button populateWorldButton;
        public Button moveShipButton;
        public Button toCombatSceneButton;
        public Button endCombatSceneButton;
        public Button colonisationButton;

        public GameState currentState;
        private GameState lastState;

        public static DevOptionsManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        // Start is called before the first frame update
        void Start()
        {
            launchTurnButton.interactable = false;

            currentState = GameState.WorldNotPopulated;
            RefreshGameState();
        }

        public void SaveState()
        {
            lastState = currentState;
        }

        public void BackToLastState()
        {
            currentState = lastState;
        }

        public void RefreshGameState()
        {
            switch (currentState)
            {
                case GameState.WorldNotPopulated:
                    populateWorldButton.interactable = true;
                    moveShipButton.interactable = false;
                    toCombatSceneButton.interactable = false;
                    endCombatSceneButton.interactable = false;
                    colonisationButton.interactable = true;
                    break;
                case GameState.GameStarted:
                    populateWorldButton.interactable = false;
                    moveShipButton.interactable = true;
                    toCombatSceneButton.interactable = false;
                    endCombatSceneButton.interactable = false;
                    colonisationButton.interactable = true;
                    break;
                case GameState.TurnStarted:
                    populateWorldButton.interactable = false;
                    moveShipButton.interactable = false;
                    toCombatSceneButton.interactable = false;
                    endCombatSceneButton.interactable = false;
                    colonisationButton.interactable = true;
                    break;
                case GameState.TurnFinished:
                    populateWorldButton.interactable = false;
                    moveShipButton.interactable = true;
                    toCombatSceneButton.interactable = false;
                    endCombatSceneButton.interactable = false;
                    colonisationButton.interactable = true;
                    break;
                case GameState.CombatStarted:
                    populateWorldButton.interactable = false;
                    moveShipButton.interactable = false;
                    toCombatSceneButton.interactable = false;
                    endCombatSceneButton.interactable = true;
                    colonisationButton.interactable = false;
                    break;
                default:
                    break;
            }
        }

        public enum GameState
        {
            WorldNotPopulated,
            GameStarted,
            TurnStarted,
            TurnFinished,
            CombatStarted
        }
    }
}
