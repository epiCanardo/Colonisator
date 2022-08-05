using Assets.Scripts.Front.MainManagers;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Front.QuickActions
{
    public class EndCombatScene : UnityEngine.MonoBehaviour
    {
        private Button button;
        private DevOptionsManager dom;

        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(EndCombat);

            dom = FindObjectOfType<DevOptionsManager>();
        }

        public void EndCombat()
        {
            dom.BackToLastState();
            dom.RefreshGameState();

            SceneManager.UnloadSceneAsync("CombatScene", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);            
        }
    }
}