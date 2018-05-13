using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class WhiteEyeEffect : AbstractDrugEffect
{
    public PostProcessProfile ppProfile;

    public override void StartEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        GameObject.FindGameObjectWithTag("Red").GetComponent<AudioSource>().volume = 1;

        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.WhiteEyeSprite != null)
            {
                tile.Object.GetComponent<SpriteRenderer>().sprite = tile.WhiteEyeSprite;
            }
        }

        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            PostProcessVolume active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                LensDistortion settings;
                active.profile.TryGetSettings(out settings);
                if (settings != null) { settings.enabled.Override(true); }
            }
        }
    }

    public override void EndEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        GameObject.FindGameObjectWithTag("Red").GetComponent<AudioSource>().volume = 0;
        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.WhiteEyeSprite != null)
            {
                tile.Object.GetComponent<SpriteRenderer>().sprite = tile.BaseSprite;
            }
        }
        var cam = GameObject.FindGameObjectWithTag("MainCamera");
        if (cam != null)
        {
            PostProcessVolume active = cam.GetComponent<PostProcessVolume>();
            if (active != null)
            {
                LensDistortion settings;
                active.profile.TryGetSettings(out settings);
                if (settings != null) { settings.enabled.Override(false); }
            }
        }
    }
}
