using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            PostProcessVolume active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                ChromaticAberration settings;
                Bloom settings2;

                active.profile.TryGetSettings(out settings2);
                active.profile.TryGetSettings(out settings);
                if (settings != null) { settings.enabled.Override(true); }
                if (settings2 != null) { settings.enabled.Override(true); }
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
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            PostProcessVolume active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                ChromaticAberration settings;
                Bloom settings2;

                active.profile.TryGetSettings(out settings2);
                active.profile.TryGetSettings(out settings);
                if (settings != null) { settings.enabled.Override(false); }
                if (settings2 != null) { settings.enabled.Override(false); }
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
