using ColanderSource;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Colfront.GamePlay
{
    public class GamepediaFactionsManager : MonoBehaviour
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
