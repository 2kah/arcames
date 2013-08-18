using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using AssemblyCSharp;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AssemblyCSharp
{
    public enum MovementType {Still, RandomShort, RandomLong, Clockwise, AntiClockwise, Chase, Flee};
    public enum CollisionEffect {None, Death, Teleport, Push};
    public enum EntityType {Red, Green, Blue, Player};
}

//TODO: this could be a singleton
public class Ruleset : MonoBehaviour {

    [System.NonSerialized]
    public string Name, Description;
    [System.NonSerialized]
    public float PlayerSpeed, RedSpeed, GreenSpeed, BlueSpeed;
    [System.NonSerialized]
    public int[] NumEntities;
    //public int NumRed, NumGreen, NumBlue;
    [System.NonSerialized]
    public MovementType RedMovement, GreenMovement, BlueMovement;
    [System.NonSerialized]
    public EntityType RedTarget, GreenTarget, BlueTarget;
    [System.NonSerialized]
    public int ScoreLimit = 1;
    [System.NonSerialized]
    public CollisionEffect[] CollisionEffects;
    //public CollisionEffect PlayerRed, PlayerGreen, PlayerBlue, RedPlayer, RedRed, RedGreen, RedBlue, GreenPlayer, GreenRed, GreenGreen, GreenBlue, BluePlayer, BlueRed, BlueGreen, BlueBlue;
    [System.NonSerialized]
    public int ScorePlayerRed, ScorePlayerGreen, ScorePlayerBlue, ScoreRedRed, ScoreRedGreen, ScoreRedBlue, ScoreGreenGreen, ScoreGreenBlue, ScoreBlueBlue;
    [System.NonSerialized]
    public int MapWidth, MapHeight;
    [System.NonSerialized]
    public bool[] MapData;
    //public Map MapObject;
    
    private Util util;
    private static bool firstAwake = true;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        util = new Util();
        if(firstAwake)
        {
            Type rulesType = util.InbuiltRules[UnityEngine.Random.Range(0, util.InbuiltRules.Count)];
            util.CopyFromRules((Rules)Activator.CreateInstance(rulesType));
            firstAwake = false;
        }
    }
    
    void Start()
    {
    }
    
    void Update()
    {
    }
    
}
