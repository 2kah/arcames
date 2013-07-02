using UnityEngine;

namespace AssemblyCSharp
{
	
	public enum Direction {None, Up, Right, Down, Left};
    public enum Axis {None, Horizontal, Vertical};
	
	public class Util
	{
		public Util ()
		{
		}
		
		public Direction Opposite(Direction dir)
		{
			switch(dir)
			{
            case Direction.Up:
                return Direction.Down;
            case Direction.Right:
                return Direction.Left;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            default:
                return Direction.None;
			}
		}
        
        public Axis GetAxis(Direction dir)
        {
            if(dir == Direction.Left || dir == Direction.Right)
                return Axis.Horizontal;
            else if(dir == Direction.Up || dir == Direction.Down)
                return Axis.Vertical;
            return Axis.None;
        }
        
        public Vector2 DirectionToVector(Direction dir)
        {
            switch(dir)
            {
            case Direction.Up:
                return new Vector2(0,1);
            case Direction.Right:
                return new Vector2(1,0);
            case Direction.Down:
                return new Vector2(0,-1);
            case Direction.Left:
                return new Vector2(-1,0);
            default:
                return new Vector2(0,0);
            }
        }
        
        public Vector3 EmptyPosition()
        {
            while(true)
            {
                Vector3 target = new Vector3(Random.Range(-6f,6f),0.5f,Random.Range(-6f,6f));
                var checkResult = Physics.OverlapSphere(target, 1);
                if(checkResult.Length == 0)
                {
                    return target;
                }
            }
        }
	}
}

