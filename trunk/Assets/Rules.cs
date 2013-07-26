using System;
using System.Xml;
using System.Xml.Serialization;

namespace AssemblyCSharp
{
    public class Rules
    {
        public string Name = "", Description = "";
        public float PlayerSpeed = 2.5f, RedSpeed = 2, GreenSpeed = 2, BlueSpeed = 2;
        public int NumRed, NumGreen, NumBlue;
        public MovementType RedMovement, GreenMovement, BlueMovement;
        public EntityType RedTarget, GreenTarget, BlueTarget;
        public int ScoreLimit;
        public CollisionEffect PlayerRed, PlayerGreen, PlayerBlue, RedPlayer, RedRed, RedGreen, RedBlue, GreenPlayer, GreenRed, GreenGreen, GreenBlue, BluePlayer, BlueRed, BlueGreen, BlueBlue;
        public int ScorePlayerRed, ScorePlayerGreen, ScorePlayerBlue, ScoreRedRed, ScoreRedGreen, ScoreRedBlue, ScoreGreenGreen, ScoreGreenBlue, ScoreBlueBlue;
        public Map MapObject;
        
        public Rules ()
        {
        }
    }
}

