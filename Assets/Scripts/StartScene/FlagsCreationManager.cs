using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagsCreationManager : MonoBehaviour
{
    public static FlagsCreationManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    public List<Sprite> Bases;
    public Image BaseFlagImage;
    public TextMeshProUGUI PrevisousBase;
    public TextMeshProUGUI NextBase;

    public List<GameObject> DisplayFlags;

    public void ShowBaseFlag()
    {
        BaseFlagImage.sprite = Bases.First();
    }
}
