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
        
        public Vector3 EmptySpawnPosition()
        {
            for(int i = 0; i < 1000; i++)
            {
                //find an empty position that isn't too close to (0,0) which is where the player spawns
                float x = Random.value < 0.5 ? Random.Range(-6f,-1.5f) : Random.Range(1.5f, 6f);
                float z = Random.value < 0.5 ? Random.Range(-6f,-1.5f) : Random.Range(1.5f, 6f);
                Vector3 target = new Vector3(x, 0.5f, z);
                if(PositionEmpty(target))
                    return target;
            }
            return new Vector3(-2, 0.5f, 0);
        }
        
        private bool PositionEmpty(Vector3 pos)
        {
            var checkResult = Physics.OverlapSphere(pos, 1);
            if(checkResult.Length == 0)
                return true;
            return false;
        }
        
        public Vector3 EmptyPosition()
        {
            for(int i = 0; i < 1000; i++)
            {
                Vector3 target = new Vector3(Random.Range(-6f,6f),0.5f,Random.Range(-6f,6f));
                if(PositionEmpty(target))
                    return target;
            }
            //we have tried 1000 locations and none are empty
            return new Vector3(0, 0.5f, 0);
        }
	}
}

