using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class WhiteEyeEffect : AbstractDrugEffect
{
    public override void StartEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");

        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.WhiteEyeSprite != null)
            {
                tile.Object.GetComponent<SpriteRenderer>().sprite = tile.WhiteEyeSprite;
            }
        }
    }

    public override void EndEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");

        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.WhiteEyeSprite != null)
            {
                tile.Object.GetComponent<SpriteRenderer>().sprite = tile.BaseSprite;
            }
        }
    }
}
