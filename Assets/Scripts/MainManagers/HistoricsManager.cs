using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class HistoricsManager : MonoBehaviour
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
