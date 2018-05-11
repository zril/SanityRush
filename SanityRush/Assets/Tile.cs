using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Solid { get; set; }

    public Tile (int x, int y)
    {
        X = x;
        Y = y;
        Solid = true;
    }
}
