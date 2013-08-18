using System;

namespace AssemblyCSharp
{
    public class WhackAMole : Rules
    {
        public WhackAMole ()
        {
            Util util = new Util();
            
            Name = "Whack a mole";
            Description = "Catch greens to get points, get 3 points to win";
            NumEntities[(int)EntityType.Green] = 3;
            NumEntities[(int)EntityType.Blue] = 5;
            GreenMovement = MovementType.Flee;
            BlueMovement = MovementType.Chase;
            GreenTarget = EntityType.Player;
            BlueTarget = EntityType.Green;
            ScoreLimit = 3;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Green, EntityType.Player)] = CollisionEffect.Teleport;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Green, EntityType.Blue)] = CollisionEffect.Teleport;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Blue, EntityType.Green)] = CollisionEffect.Teleport;
            ScorePlayerGreen = 1;
        }
    }
}

