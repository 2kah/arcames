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
    }
}

