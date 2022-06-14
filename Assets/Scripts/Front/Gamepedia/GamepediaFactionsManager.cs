using Assets.Scripts.Front.ScriptableObjects.Faction;
using UnityEngine;

namespace Assets.Scripts.Front.Gamepedia
{
    public class GamepediaFactionsManager : UnityEngine.MonoBehaviour
    {
        public GameObject panelPrefab;
        public Transform canvas;

        private GameObject panel;

        public static GamepediaFactionsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public void ShowFactions()
        {
            panel = Instantiate(panelPrefab, canvas);
            panel.GetComponent<FactionsBoard>().ShowBoard(x=>x.longName);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F)) ShowFactions();
        }
    }
}
