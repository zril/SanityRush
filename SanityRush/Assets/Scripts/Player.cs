﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    

    private float moveTimer = 0;
    private float moveReminder = 0;
    private float moveSpeed = 2f;

    private float knightAttackTimer = 0;

    private int maxDrugLevel = 50; //nombre de cases

    private int currentPositionX;
    private int currentPositionY;
    private int oldPositionX;
    private int oldPositionY;
    private int previousPositionX;
    private int previousPositionY;

    public AbstractDrugEffect CurrentActiveDrug { get; set; }
    public float DrugLevel { get; set; }
    public float DrugTimer { get; set; }

    public DrugType Drug1 { get; set; }
    public DrugType Drug2 { get; set; }

    private GameObject level;
    
    private Animator animator;

    private string direction = "Face";

    public RuntimeAnimatorController knightController;
    public RuntimeAnimatorController baseController { get; set; }

    private bool dead;
    private SoundManager soundManager;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        moveTimer = 0;

        int x = Mathf.RoundToInt(transform.localPosition.x);
        int y = Mathf.RoundToInt(transform.localPosition.y);

        currentPositionX = x;
        currentPositionY = y;

        oldPositionX = x;
        oldPositionY = y;

        previousPositionX = x;
        previousPositionY = y;

        level = GameObject.FindGameObjectWithTag("Level");

        CurrentActiveDrug = null;
        DrugLevel = level.GetComponent<Level>().startingDrugLevel;
        DrugTimer = 0;
        Drug1 = DrugType.None;
        Drug2 = DrugType.None;
        
        animator = GetComponent<Animator>();

        baseController = animator.runtimeAnimatorController;


        dead = false;

        GameObject soundManagerObject = GameObject.FindGameObjectWithTag("SoundManager");
        soundManager = soundManagerObject.GetComponent<SoundManager>();

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_ELEVATOR"));



        if (GameObject.Find("MusicDrogue") != null)
        {
            GameObject.FindGameObjectWithTag("Blue").GetComponent<AudioSource>().volume = 1;
            GameObject.FindGameObjectWithTag("Yellow").GetComponent<AudioSource>().volume = 0;
            GameObject.FindGameObjectWithTag("Red").GetComponent<AudioSource>().volume = 0;
            GameObject.FindGameObjectWithTag("Black").GetComponent<AudioSource>().volume = 0;
            GameObject.FindGameObjectWithTag("Glitch").GetComponent<AudioSource>().volume = 0;
        }

        Global.CurrentLevel = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update () {
        if (!dead)
        {
            var knight = false;
            if (moveTimer > 0)
            {
                moveTimer -= Time.deltaTime;
            }
            else
            {
                if (moveTimer < 0)
                {
                    moveReminder = -moveTimer;
                }
                else
                {
                    moveReminder = 0;
                }

                moveTimer = 0;
                if (oldPositionX != currentPositionX || oldPositionY != currentPositionY)
                {
                    gameObject.transform.localPosition = new Vector3(currentPositionX, currentPositionY, -1);
                    previousPositionX = oldPositionX;
                    previousPositionY = oldPositionY;
                    oldPositionX = currentPositionX;
                    oldPositionY = currentPositionY;
                }

                //drug
                var drug = CheckDrug(currentPositionX, currentPositionY);
                if (drug != DrugType.None)
                {
                    audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_PICKUP"));
                    if (Drug1 == DrugType.None)
                    {
                        Drug1 = drug;
                    }
                    else if (Drug2 == DrugType.None)
                    {
                        Drug2 = drug;
                    }
                    else
                    {
                        //on gache pas la drogue
                        UseDrug();
                        Drug2 = drug;
                    }
                    level.GetComponent<Level>().RemoveDrug(currentPositionX, currentPositionY);
                }

                //kill
                var kill = CheckKill(currentPositionX, currentPositionY);
                if (kill)
                {
                    soundManager.stopLoop("Walk");
                    audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_ANXIETY"));
                    dead = true;
                    animator.Play("player-m-cry");
                    Global.tryNumber++;
                    StartCoroutine(ReLoadLevelAfterDelay(3));

                }


                //knight
                if (CurrentActiveDrug != null && CurrentActiveDrug.Type == DrugType.Knight && knightAttackTimer <= 0)
                {
                    var skeleton = CheckSkeleton(currentPositionX, currentPositionY);
                    if (skeleton != null)
                    {
                        SkeletonKill(skeleton);
                    }
                    knight = true;
                }

                //guard
                if (!knight)
                {

                    var guard = CheckGuard(currentPositionX, currentPositionY);
                    if (guard)
                    {
                        var guards = GameObject.FindGameObjectsWithTag("Guard");
                        foreach (GameObject obj in guards)
                        {
                            obj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }
                        
                        dead = true;
                        soundManager.stopLoop("Walk");
                        StartCoroutine(ReLoadLevelAfterDelay(3));
                        animator.Play(direction + "Idle");
                        Global.tryNumber++;
                        audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_BUSTED"));

                    }
                }


                //stairs
                var levelIncrement = CheckStairs(currentPositionX, currentPositionY);
                if (levelIncrement > 0)
                {
                    Global.CurrentLevel += levelIncrement;
                    SceneManager.LoadScene(Global.CurrentLevel);
                }
            }

            //move
            knightAttackTimer -= Time.deltaTime;
            if (moveTimer == 0)
            {
                var position = gameObject.transform.localPosition;
                var x = 0;
                var y = 0;
                bool move = false;
                if (Input.GetAxis("Horizontal") > 0)
                {
                    x = 1;
                    move = true;
                    direction = "Right";
                }
                else if (Input.GetAxis("Horizontal") < 0)
                {
                    x = -1;
                    move = true;
                    direction = "Left";
                }
                else if (Input.GetAxis("Vertical") > 0)
                {
                    y = 1;
                    move = true;
                    direction = "Back";
                }
                else if (Input.GetAxis("Vertical") < 0)
                {
                    y = -1;
                    move = true;
                    direction = "Face";
                }

                if (knightAttackTimer > 0)
                {
                    move = false;
                }

                if (move)
                {
                    var fall = CheckFall(oldPositionX + x, oldPositionY + y);

                    if (fall && !knight)
                    {
                        animator.Play("player-m-fall");
                        dead = true;
                        move = false;
                        soundManager.stopLoop("Walk");
                        audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_FALL"));
                        Global.tryNumber++;
                        StartCoroutine(ReLoadLevelAfterDelay(3));
                    }
                }

                var solid = CheckSolid(oldPositionX + x, oldPositionY + y);
                
                if (move && !solid)
                {
                    soundManager.playLoop("Walk");
                    moveTimer = -moveReminder + (1 / moveSpeed);
                    UpdateDrugLevel(moveReminder);
                    UpdateDrugTimer(moveReminder);
                    currentPositionX = oldPositionX + x;
                    currentPositionY = oldPositionY + y;
                    animator.Play(direction + "Walk");
                }
                else
                {
                    soundManager.stopLoop("Walk");
                    if (knightAttackTimer < 0)
                    {
                        if (!dead)
                        {
                            animator.Play(direction + "Idle");
                        }

                        DrugLevel = Mathf.Round(DrugLevel);
                        DrugTimer = Mathf.Round(DrugTimer);
                        UpdateDrugLevel(0);
                        UpdateDrugTimer(0);
                    }
                }
            }

            if (moveTimer > 0)
            {
                gameObject.transform.localPosition = Vector3.Lerp(new Vector3(oldPositionX, oldPositionY, -1), new Vector3(currentPositionX, currentPositionY, -1), moveSpeed * ((1 / moveSpeed) - moveTimer));

                var time = Time.deltaTime;
                if (moveTimer - Time.deltaTime < 0)
                {
                    time = moveTimer;
                }
                UpdateDrugLevel(time);
                UpdateDrugTimer(time);
            }

            //actions
            if (Input.GetButtonDown("Fire2"))
            {
                var tmp = Drug2;
                Drug2 = Drug1;
                Drug1 = tmp;
            }

            if (Input.GetButtonDown("Fire1"))
            {
                UseDrug();
            }

            
        } else
        {
            //CurrentActiveDrug.EndEffect();
        }

        //drugs
        if (CurrentActiveDrug != null)
        {
            CurrentActiveDrug.UpdateEffect();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(GameObject.Find("MusicDrogue"));
            Global.CurrentLevel = 1;
            Global.tryNumber = 1;
            SceneManager.LoadScene(0);
        }
    }

    private void UpdateDrugLevel(float time)
    {
        DrugLevel -= moveSpeed * time;
        if (DrugLevel <= 0 || DrugLevel >= maxDrugLevel) // max en nombre de cases
        {
            dead = true;
            if (DrugLevel <= 0)
            {
                animator.Play("player-m-cry");
                soundManager.stopLoop("Walk");
                audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_ANXIETY"));
            } else if (DrugLevel >= maxDrugLevel)
            {
                animator.Play("player-m-puke");
                soundManager.stopLoop("Walk");
                audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_OVERDOSE"));
            }

            Global.tryNumber++;
            StartCoroutine(ReLoadLevelAfterDelay(3));
        }
    }

    private void UpdateDrugTimer(float time)
    {
        if (DrugTimer > 0)
        {
            DrugTimer -= time * moveSpeed;
        }
        else
        {
            DrugTimer = 0;
            if (CurrentActiveDrug != null)
            {
                CurrentActiveDrug.EndEffect();
                CurrentActiveDrug = null;
            }
        }
    }

    private bool CheckGuard(int x, int y)
    {
        var tile = level.GetComponent<Level>().GetTile(x, y);
        return tile.Guarded;
    }

    private Tile CheckSkeleton(int x, int y)
    {
        var comp = level.GetComponent<Level>();
        if (comp.GetTile(x + 1, y).Guard && !comp.GetTile(x + 1, y).GuardKO)
        {
            direction = "Right";
            return comp.GetTile(x + 1, y);
        }
        if (comp.GetTile(x, y + 1).Guard && !comp.GetTile(x, y + 1).GuardKO)
        {
            direction = "Back";
            return comp.GetTile(x, y + 1);
        }
        if (comp.GetTile(x - 1, y).Guard && !comp.GetTile(x - 1, y).GuardKO)
        {
            direction = "Left";
            return comp.GetTile(x - 1, y);
        }
        if (comp.GetTile(x, y - 1).Guard && !comp.GetTile(x, y - 1).GuardKO)
        {
            direction = "Face";
            return comp.GetTile(x, y - 1);
        }
        return null;
    }

    private void SkeletonKill(Tile tile)
    {
        if (!tile.GuardKO)
        {
            tile.GuardKO = true;
            level.GetComponent<Level>().KillSkeleton(tile.X, tile.Y);

            knightAttackTimer = 0.5f;
            animator.Play(direction + "Attack");
            audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_SWORD_1"));
        }
    }

    private bool CheckSolid(int x, int y)
    {
        var tile = level.GetComponent<Level>().GetTile(x, y);
        return tile.Solid;
    }

    private bool CheckFall(int x, int y)
    {
        return x == previousPositionX && y == previousPositionY;
    }

    private bool CheckKill(int x, int y)
    {
        var tile = level.GetComponent<Level>().GetTile(x, y);
        return tile.Kill;
    }

    private DrugType CheckDrug(int x, int y)
    {
        var drug = level.GetComponent<Level>().GetTile(x, y).Drug;
        return drug;
    }

    private int CheckStairs(int x, int y)
    {
        var increment = level.GetComponent<Level>().GetTile(x, y).StairsLevelIncrement;
        return increment;
    }

    private void UseDrug()
    {

        if (Drug1 != DrugType.None)
        {

            if (CurrentActiveDrug != null)
            {
                CurrentActiveDrug.EndEffect();
            }

            switch (Drug1){
                case DrugType.WhiteEye:
                    DrugLevel += 6; 
                    DrugTimer = 4; 
                    CurrentActiveDrug = new WhiteEyeEffect();
                    break;
                case DrugType.Thorn:
                    DrugLevel += 10;
                    DrugTimer = 6;
                    CurrentActiveDrug = new ThornEffect();
                    break;
                case DrugType.Knight:
                    DrugLevel += 18;
                    DrugTimer = 10;
                    CurrentActiveDrug = new KnightEffect();
                    break;
                default:
                    break;
            }
            audioSource.PlayOneShot((AudioClip)Resources.Load("sfx/SFX_POWERUP"));
            CurrentActiveDrug.StartEffect();
            
            Drug1 = Drug2;
            Drug2 = DrugType.None;
        }
    }

    IEnumerator LoadLevelAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(index);
    }

    IEnumerator ReLoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
