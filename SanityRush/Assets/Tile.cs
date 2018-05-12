using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Tile
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool Solid { get; set; }
    public DrugType Drug { get; set; }

    public Sprite BaseSprite { get; set; }
    public Sprite WhiteEyeSprite { get; set; }
    public GameObject Object { get; set; }

    public Tile (int x, int y)
    {
        X = x;
        Y = y;
        Solid = true;
        Drug = DrugType.None;
    }
}
