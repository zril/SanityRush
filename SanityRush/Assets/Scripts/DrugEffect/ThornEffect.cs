using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ThornEffect : AbstractDrugEffect
{
    public override void StartEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");

        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.Wall)
            {
                foreach (Tile adj in FindAdjacentTile(tile.X, tile.Y, level.GetComponent<Level>()))
                {
                    adj.Kill = true;

                    if (adj.Object != null)
                    {
                        tile.Object.GetComponent<SpriteRenderer>().sprite = tile.ThornSprite;
                    }
                }
            }
        }
    }

    public override void EndEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");

        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.Wall)
            {
                foreach (Tile adj in FindAdjacentTile(tile.X, tile.Y, level.GetComponent<Level>()))
                {
                    adj.Kill = false;
                    
                    if (adj.Object != null)
                    {
                        tile.Object.GetComponent<SpriteRenderer>().sprite = tile.BaseSprite;
                    }
                }
            }
        }
    }

    private List<Tile> FindAdjacentTile(int x, int y, Level level)
    {
        var list = new List<Tile>();

        list.Add(level.GetTile(x + 1, y));
        list.Add(level.GetTile(x, y + 1));
        list.Add(level.GetTile(x - 1, y));
        list.Add(level.GetTile(x, y - 1));

        return list;
    }
}
