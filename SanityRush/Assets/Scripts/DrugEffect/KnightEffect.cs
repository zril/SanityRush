using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class KnightEffect : AbstractDrugEffect
{
    public PostProcessProfile ppProfile;

    public KnightEffect()
    {
        Type = DrugType.Knight;
    }

    public override void StartEffect()
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        GameObject.FindGameObjectWithTag("Black").GetComponent<AudioSource>().volume = 1;
        GameObject.FindGameObjectWithTag("Glitch").GetComponent<AudioSource>().volume = 1;
        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.Guard)
            {
                var guard = level.GetComponent<Level>().GetInteractiveObject(tile.X, tile.Y);
                var animator = guard.GetComponent<Animator>();
                animator.enabled = true;

                var dir = guard.GetComponent<Guard>().direction;
                switch (dir)
                {
                    case Direction.Right:
                        animator.Play("skeleton_right_idle");
                        break;
                    case Direction.Up:
                        animator.Play("skeleton_back_idle");
                        break;
                    case Direction.Left:
                        animator.Play("skeleton_left_idle");
                        break;
                    case Direction.Down:
                        animator.Play("skeleton_down_idle");
                        break;
                    default:
                        break;
                }
            }
        }

        //player
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<Player>().knightController;
        
        //camera
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
        GameObject.FindGameObjectWithTag("Black").GetComponent<AudioSource>().volume = 0;
        GameObject.FindGameObjectWithTag("Glitch").GetComponent<AudioSource>().volume = 0;
        foreach (Tile tile in level.GetComponent<Level>().TileMatrix)
        {
            if (tile.Guard)
            {
                var guard = level.GetComponent<Level>().GetInteractiveObject(tile.X, tile.Y);
                guard.GetComponent<Animator>().enabled = false;
                if (tile.GuardKO)
                {
                    guard.GetComponent<SpriteRenderer>().sprite = guard.GetComponent<Guard>().GuardKOSprite;
                } else
                {
                    guard.GetComponent<SpriteRenderer>().sprite = guard.GetComponent<Guard>().GuardBaseSprite;
                }
                
            }
        }

        //player
        var player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Animator>().runtimeAnimatorController = player.GetComponent<Player>().baseController;

        //camera
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
