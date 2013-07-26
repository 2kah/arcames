using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    public class Equals : Map
    {
        public Equals ()
        {
            Width = 15;
            Height = Width;
            MapData = new bool[Width,Height];
            //make all edges walls
            for(int i = 0; i < Width; i++)
            {
                MapData[0,i] = true;
                MapData[i,0] = true;
                MapData[Width-1,i] = true;
                MapData[i,Height-1] = true;
            }
            
            //add the equals bits
            for(int i = 4; i < 11; i++)
            {
                MapData[i,4] = true;
                MapData[i,10] = true;
            }
        }
    }
}

