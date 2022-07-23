using Assets.Scripts.Model;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionName", menuName = "ScriptableObjects/FactionSO", order = 1)]
public class FactionScriptableObject : ScriptableObject
{
    public PlayerTypeEnum playerType;
    
    public Faction faction;

    public bool isPlaying;

    public Texture2D flag;

    public Color mainColor;

    public string mainColorName;
}
