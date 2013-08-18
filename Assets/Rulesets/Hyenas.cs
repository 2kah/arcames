using System;

namespace AssemblyCSharp
{
    public class Hyenas : Rules
    {
        public Hyenas ()
        {
            Util util = new Util();
            
            Name = "Hyenas";
            Description = "Run away from the reds! If they catch you you're dead. Reds die if they hit each other. Kill all reds to win";
            NumEntities[(int)EntityType.Red] = 6;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Player;
            ScoreLimit = 6;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Player, EntityType.Red)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Red, EntityType.Red)] = CollisionEffect.Death;
            ScoreRedRed = 2;
        }
    }
}

