using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ThornEffect : AbstractDrugEffect
{
    public double ChromaticValue;
    public bool ascending;
    public PostProcessVolume active;
    public ChromaticAberration settings;
    public override void StartEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        GameObject.FindGameObjectWithTag("Yellow").GetComponent<AudioSource>().volume=1; 
        ChromaticValue = 0;
        ascending = true;
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
            active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                Bloom settings2;

                active.profile.TryGetSettings(out settings2);
                active.profile.TryGetSettings(out settings);
                if (settings != null) { settings.enabled.Override(true); settings.intensity.Override((float)ChromaticValue); }
                if (settings2 != null) { settings.enabled.Override(true); }
            }
        }
    }

    public override void UpdateEffect()
    {
        if (ascending == true && ChromaticValue != 0.5) {ChromaticValue += 0.05;}
        else { ChromaticValue -= 0.05; }
        if (settings != null)
            settings.intensity.Override((float)ChromaticValue);
        if (ascending == true && ChromaticValue == 1)
            ascending = false;
        else if (ascending == false && ChromaticValue == 0.3)
            ascending = true;
    }

    public override void EndEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        GameObject.FindGameObjectWithTag("Yellow").GetComponent<AudioSource>().volume = 0;



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
            active = cam.GetComponent<PostProcessVolume>();
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
