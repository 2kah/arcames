using System;

namespace AssemblyCSharp
{
    public class HyenasPro : Rules
    {
        public HyenasPro ()
        {
            Util util = new Util();
            
            Name = "HyenasPro";
            Description = "Run away from the reds! If they catch you you're dead. Reds die if they hit each other or a green. Kill all reds to win";
            NumEntities[(int)EntityType.Red] = 8;
            NumEntities[(int)EntityType.Green] = 2;
            RedMovement = MovementType.Chase;
            RedTarget = EntityType.Player;
            GreenMovement = MovementType.RandomShort;
            ScoreLimit = 8;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Player, EntityType.Red)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Red, EntityType.Red)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Red, EntityType.Green)] = CollisionEffect.Death;
            ScoreRedRed = 2;
            ScoreRedGreen = 1;
        }
    }
}

