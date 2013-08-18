using System;

namespace AssemblyCSharp
{
    public class Pacu : Rules
    {
        public Pacu ()
        {
            Util util = new Util();
            
            Name = "Pacu";
            Description = "Collect all blues to win. Reds will chase and kill you, greens teleport you away";
            PlayerSpeed = 4;
            NumEntities[(int)EntityType.Red] = 4;
            NumEntities[(int)EntityType.Green] = 4;
            NumEntities[(int)EntityType.Blue] = 20;
            RedMovement = MovementType.Chase;
            GreenMovement = MovementType.Still;
            BlueMovement = MovementType.Still;
            RedTarget = EntityType.Player;
            RedSpeed = 3.5f;
            ScoreLimit = 20;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Player, EntityType.Red)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Green, EntityType.Player)] = CollisionEffect.Death;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Player, EntityType.Green)] = CollisionEffect.Teleport;
            CollisionEffects[util.CollisionEffectsIndex(EntityType.Blue, EntityType.Player)] = CollisionEffect.Death;
            ScorePlayerBlue = 1;
            Map pac = new Pac();
            MapWidth = pac.Width;
            MapHeight = pac.Height;
            MapData = pac.MapData;
        }
    }
}

