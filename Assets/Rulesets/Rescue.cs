using System;

namespace AssemblyCSharp
{
    public class Rescue : Rules
    {
        public Rescue ()
        {
            Util util = new Util();
            
            Name = "Rescue";
            Description = "Catch a green to win. Reds will kill both you and greens";
            NumEntities[(int)EntityType.Red] = 4;
            NumEntities[(int)EntityType.Green] = 2;
            GreenMovement = MovementType.Flee;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Green;
            GreenTarget = EntityType.Red;
            ScoreLimit = 1;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Player, EntityType.Red)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Red, EntityType.Green)] = CollisionEffect.Teleport;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Green, EntityType.Red)] = CollisionEffect.Death;
            ScorePlayerGreen = 1;
        }
    }
}

