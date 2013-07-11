using System;

namespace AssemblyCSharp
{
    public class Rescue : Rules
    {
        public Rescue ()
        {
            Name = "Rescue";
            Description = "Catch a green to win. Reds will kill both you and greens";
            NumGreen = 2;
            NumRed = 4;
            GreenMovement = MovementType.Flee;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Green;
            GreenTarget = EntityType.Red;
            ScoreLimit = 1;
            PlayerRed = CollisionEffect.Death;
            RedGreen = CollisionEffect.Teleport;
            GreenRed = CollisionEffect.Death;
            ScorePlayerGreen = 1;
        }
    }
}

