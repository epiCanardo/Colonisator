using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public GameObject playerShip;
    public GameObject ennemyShip;

    public List<AudioClip> fireSounds;
    public List<AudioClip> flybySounds;
    public List<AudioClip> hittingSounds;
    public List<AudioClip> waterHitSounds;

    public static CombatManager Instance { get; private set; }

    private readonly System.Random rnd = new System.Random();

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public double NextDouble => rnd.NextDouble();    

    public int NextInt(int start, int end)
    {
        return rnd.Next(start, end);
    }
}
