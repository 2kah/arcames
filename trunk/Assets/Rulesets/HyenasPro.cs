using System;

namespace AssemblyCSharp
{
    public class HyenasPro : Rules
    {
        public HyenasPro ()
        {
            Name = "HyenasPro";
            Description = "Run away from the reds! If they catch you you're dead. Reds die if they hit each other or a green. Kill all reds to win";
            NumRed = 8;
            NumGreen = 2;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Player;
            GreenMovement = MovementType.RandomShort;
            ScoreLimit = 8;
            PlayerRed = CollisionEffect.Death;
            RedRed = CollisionEffect.Death;
            RedGreen = CollisionEffect.Death;
            ScoreRedRed = 2;
            ScoreRedGreen = 1;
        }
    }
}

