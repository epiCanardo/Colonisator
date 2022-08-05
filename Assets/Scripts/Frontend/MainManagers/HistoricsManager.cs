using System;
using System.Text;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.Front.MainManagers
{
    public class HistoricsManager : UnityEngine.MonoBehaviour
    {
        [Header("Text pour affichage")]
        public TextMeshProUGUI textArea;

        private StringBuilder messages = new StringBuilder();

        public static HistoricsManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null) { Instance = this; }
        }

        public void NewMessage(string message)
        {
            messages.Insert(0, Environment.NewLine); 
            messages.Insert(0, Environment.NewLine);
            messages.Insert(0, message);
            textArea.text = messages.ToString();

        }
    }
}
