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

public class Ruleset : MonoBehaviour {

    [System.NonSerialized]
    public Rules rules;
    [System.NonSerialized]
    public string Name, Description;
    [System.NonSerialized]
    public int NumRed, NumGreen, NumBlue;
    [System.NonSerialized]
    public MovementType RedMovement, GreenMovement, BlueMovement;
    [System.NonSerialized]
    public EntityType RedTarget, GreenTarget, BlueTarget;
    [System.NonSerialized]
    public int ScoreLimit;
    [System.NonSerialized]
    public CollisionEffect PlayerRed, PlayerGreen, PlayerBlue, RedPlayer, RedRed, RedGreen, RedBlue, GreenPlayer, GreenRed, GreenGreen, GreenBlue, BluePlayer, BlueRed, BlueGreen, BlueBlue;
    [System.NonSerialized]
    public int ScorePlayerRed, ScorePlayerGreen, ScorePlayerBlue, ScoreRedRed, ScoreRedGreen, ScoreRedBlue, ScoreGreenGreen, ScoreGreenBlue, ScoreBlueBlue;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    
    void Start()
    {
        rules = new Rules();
    }
    
    void Update()
    {
    }
    
}
