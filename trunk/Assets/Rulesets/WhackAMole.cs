using System;

namespace AssemblyCSharp
{
    public class WhackAMole : Rules
    {
        public WhackAMole ()
        {
            Name = "Whack a mole";
            Description = "Catch greens to get points, get 3 points to win";
            NumGreen = 3;
            NumBlue = 5;
            GreenMovement = MovementType.Flee;
            BlueMovement = MovementType.Chase;
            GreenTarget = EntityType.Player;
            BlueTarget = EntityType.Green;
            ScoreLimit = 3;
            GreenPlayer = CollisionEffect.Teleport;
            GreenBlue = CollisionEffect.Teleport;
            BlueGreen = CollisionEffect.Teleport;
            ScorePlayerGreen = 1;
        }
    }
}

