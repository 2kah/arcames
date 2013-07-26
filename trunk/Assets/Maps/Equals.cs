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
            bool[,] MapData2D = new bool[Width,Height];
            //make all edges walls
            for(int i = 0; i < Width; i++)
            {
                MapData2D[0,i] = true;
                MapData2D[i,0] = true;
                MapData2D[Width-1,i] = true;
                MapData2D[i,Height-1] = true;
            }
            
            //add the equals bits
            for(int i = 4; i < 11; i++)
            {
                MapData2D[i,4] = true;
                MapData2D[i,10] = true;
            }
            
            MapData = new bool[Width * Height];
            
            //convert to 1D array
            for(int i = 0; i < Width; i++)
            {
                for(int j = 0 ; j < Height; j++)
                {
                    MapData[To1DIndex(i,j,Width)] = MapData2D[i,j];
                }
            }
        }
    }
}

