using Assets.Scripts.Front.MainManagers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Front.QuickActions
{
    public class CombatScene : UnityEngine.MonoBehaviour
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