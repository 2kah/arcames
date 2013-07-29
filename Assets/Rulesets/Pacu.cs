using System;

namespace AssemblyCSharp
{
    public class Pacu : Rules
    {
        public Pacu ()
        {
            Name = "Pacu";
            Description = "Collect all blues to win. Reds will chase and kill you, greens teleport you away";
            PlayerSpeed = 3;
            NumRed = 4;
            NumGreen = 4;
            NumBlue = 20;
            RedMovement = MovementType.Chase;
            GreenMovement = MovementType.Still;
            BlueMovement = MovementType.Still;
            RedTarget = EntityType.Player;
            ScoreLimit = 20;
            PlayerRed = CollisionEffect.Death;
            GreenPlayer = CollisionEffect.Death;
            PlayerGreen = CollisionEffect.Teleport;
            BluePlayer = CollisionEffect.Death;
            ScorePlayerBlue = 1;
            Map pac = new Pac();
            MapWidth = pac.Width;
            MapHeight = pac.Height;
            MapData = pac.MapData;
        }
    }
}

