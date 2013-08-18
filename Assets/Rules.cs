using System;
using System.Xml;
using System.Xml.Serialization;

namespace AssemblyCSharp
{
    public class Rules
    {
        public string Name = "", Description = "";
        public float PlayerSpeed = 2.5f, RedSpeed = 2, GreenSpeed = 2, BlueSpeed = 2;
        public int[] NumEntities;
        //public int NumRed, NumGreen, NumBlue;
        public MovementType RedMovement, GreenMovement, BlueMovement;
        public EntityType RedTarget, GreenTarget, BlueTarget;
        public int ScoreLimit;
        public CollisionEffect[] CollisionEffects;
        //public CollisionEffect PlayerRed, PlayerGreen, PlayerBlue, RedPlayer, RedRed, RedGreen, RedBlue, GreenPlayer, GreenRed, GreenGreen, GreenBlue, BluePlayer, BlueRed, BlueGreen, BlueBlue;
        public int ScorePlayerRed, ScorePlayerGreen, ScorePlayerBlue, ScoreRedRed, ScoreRedGreen, ScoreRedBlue, ScoreGreenGreen, ScoreGreenBlue, ScoreBlueBlue;
        public int MapWidth, MapHeight;
        public bool[] MapData;
        
        public Rules ()
        {
            //Equals is default map
            Map defaultMap = new Equals();
            MapWidth = defaultMap.Width;
            MapHeight = defaultMap.Height;
            MapData = defaultMap.MapData;
            
            CollisionEffects = DefaultCollisionEffects();
            NumEntities = DefaultNumEntities();
        }
        
        private int[] DefaultNumEntities()
        {
            int numEntityTypes = Enum.GetNames(typeof(EntityType)).Length - 1;
            int[] defaultNumEntities = new int[numEntityTypes];
            for(int i = 0; i < defaultNumEntities.Length; i++)
            {
                defaultNumEntities[i] = 0;
            }
            return defaultNumEntities;
        }
        
        //initialise the collisionEffects array to the default effect of none
        private CollisionEffect[] DefaultCollisionEffects()
        {
            int numCollisionEffects = Enum.GetNames(typeof(CollisionEffect)).Length;
            CollisionEffect[] defaultEffects = new CollisionEffect[numCollisionEffects * numCollisionEffects];
            for(int i = 0; i < defaultEffects.Length; i++)
            {
                defaultEffects[i] = CollisionEffect.None;
            }
            return defaultEffects;
        }
    }
}

