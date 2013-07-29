using System;

namespace AssemblyCSharp
{
    public class Pac : Map
    {
        public Pac ()
        {
            Width = 36;
            Height = 20;
            bool[,] MapData2D = new bool[Width,Height];
            //make all edges walls
            for(int i = 0; i < Width; i++)
            {
                MapData2D[i,0] = true;
                MapData2D[i,Height-1] = true;
            }
            for(int i = 0; i < Height; i++)
            {
                MapData2D[0,i] = true;
                MapData2D[Width-1,i] = true;
            }
            
            MapData2D[9,1] = true;
            MapData2D[9,2] = true;
            FillXRange(MapData2D, 3, 6, 3);
            MapData2D[9,3] = true;
            FillXRange(MapData2D, 12, 17, 3);
            FillXRange(MapData2D, 3, 6, 4);
            MapData2D[9,4] = true;
            FillXRange(MapData2D, 12, 17, 4);
            FillXRange(MapData2D, 3, 4, 5);
            FillXRange(MapData2D, 3, 4, 6);
            FillXRange(MapData2D, 3, 4, 7);
            FillXRange(MapData2D, 7, 9, 7);
            FillXRange(MapData2D, 12, 17, 7);
            FillXRange(MapData2D, 3, 4, 8);
            FillXRange(MapData2D, 7, 9, 8);
            FillXRange(MapData2D, 12, 17, 8);
            FillXRange(MapData2D, 12, 13, 9);
            FillXRange(MapData2D, 12, 13, 10);
            FillXRange(MapData2D, 3, 4, 11);
            FillXRange(MapData2D, 7, 9, 11);
            FillXRange(MapData2D, 12, 15, 11);
            FillXRange(MapData2D, 3, 4, 12);
            FillXRange(MapData2D, 7, 9, 12);
            FillXRange(MapData2D, 12, 15, 12);
            FillXRange(MapData2D, 3, 4, 13);
            FillXRange(MapData2D, 3, 4, 14);
            FillXRange(MapData2D, 3, 6, 15);
            MapData2D[9,15] = true;
            FillXRange(MapData2D, 12, 17, 15);
            FillXRange(MapData2D, 3, 6, 16);
            MapData2D[9,16] = true;
            FillXRange(MapData2D, 12, 17, 16);
            MapData2D[9,17] = true;
            MapData2D[9,18] = true;
            
            Mirror(MapData2D);
            
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

