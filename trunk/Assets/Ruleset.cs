using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace AssemblyCSharp
{
    public enum MovementType {Still, RandomShort, RandomLong, Clockwise, AntiClockwise, Chase, Flee};
    public enum CollisionEffect {None, Death, Teleport, Push};
    public enum EntityType {Red, Green, Blue, Player};
}

public class Ruleset : MonoBehaviour {

    
    public int NumRed, NumGreen, NumBlue;
    public MovementType RedMovement, GreenMovement, BlueMovement;
    public EntityType RedTarget, GreenTarget, BlueTarget;
    public int ScoreLimit;
    public CollisionEffect PlayerRed, PlayerGreen, PlayerBlue, RedPlayer, RedRed, RedGreen, RedBlue, GreenPlayer, GreenRed, GreenGreen, GreenBlue, BluePlayer, BlueRed, BlueGreen, BlueBlue;
    public int ScorePlayerRed, ScorePlayerGreen, ScorePlayerBlue, ScoreRedRed, ScoreRedGreen, ScoreRedBlue, ScoreGreenGreen, ScoreGreenBlue, ScoreBlueBlue;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
