using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Colfront.GamePlay
{
    public class CombatScene : MonoBehaviour
    {
        private Button button;
        private DevOptionsManager dom;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(LaunchCombatScene);

            dom = FindObjectOfType<DevOptionsManager>();
        }

        public void LaunchCombatScene()
        {
            dom.SaveState();
            dom.currentState = DevOptionsManager.GameState.CombatStarted;
            dom.RefreshGameState();

            SceneManager.LoadScene("CombatScene", LoadSceneMode.Additive);
            
        }
    }
}