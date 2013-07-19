using System;

namespace AssemblyCSharp
{
    public class Hyenas : Rules
    {
        public Hyenas ()
        {
            Name = "Hyenas";
            Description = "Run away from the reds! If they catch you you're dead. Reds die if they hit each other. Kill all reds to win";
            NumRed = 6;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Player;
            ScoreLimit = 6;
            PlayerRed = CollisionEffect.Death;
            RedRed = CollisionEffect.Death;
            ScoreRedRed = 2;
        }
    }
}

