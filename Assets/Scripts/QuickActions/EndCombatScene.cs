using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Colfront.GamePlay
{
    public class EndCombatScene : MonoBehaviour
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