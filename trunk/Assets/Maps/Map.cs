using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
    public class Map
    {
        public int Width, Height;
        public bool[] MapData;
        
        public Map ()
        {
        }
        
        public int To1DIndex(int x, int y, int width)
        {
            return (width * y) + x;
        }
        
        public Vector2 To2DIndex(int i, int width)
        {
            int x = i % width;
            int y = (i - x) / width;
            return new Vector2(x, y);
        }
        
        public void FillXRange(bool[,] map, int start, int finish, int y)
        {
            for(int i = start; i <= finish; i++)
                map[i,y] = true;
        }
        
        public void FillYRange(bool[,] map, int start, int finish, int x)
        {
            for(int i = start; i <= finish; i++)
                map[x,i] = true;
        }
        
        public void Mirror(bool[,] map)
        {
            for(int x = 0; x < Width / 2; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    map[x + (Width / 2),y] = map[(Width / 2) - (1 + x),y];
                }
            }
        }
    }
}

