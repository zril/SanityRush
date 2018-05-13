using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    

    private float moveTimer = 0;
    private float moveReminder = 0;
    private float moveSpeed = 2f;

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
        Global.CurrentLevel = level.GetComponent<Level>().levelNumber - 1;

        CurrentActiveDrug = null;
        DrugLevel = level.GetComponent<Level>().startingDrugLevel;
        DrugTimer = 0;
        Drug1 = DrugType.None;
        Drug2 = DrugType.None;
        
        animator = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {

        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        } else
        {
            if (moveTimer < 0)
            {
                moveReminder = -moveTimer;
            } else
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
                if (Drug1 == DrugType.None)
                {
                    Drug1 = drug;
                } else if (Drug2 == DrugType.None)
                {
                    Drug2 = drug;
                } else
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
                //respawn
                SceneManager.LoadScene(Global.CurrentLevel);
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

            if (move)
            {
                var fall = CheckFall(oldPositionX + x, oldPositionY + y);

                if (fall)
                {
                    //respawn
                    SceneManager.LoadScene(Global.CurrentLevel);
                }
            }

            var solid = CheckSolid(oldPositionX + x, oldPositionY + y);

            if (move && !solid)
            {
                moveTimer = -moveReminder + (1 / moveSpeed);
                UpdateDrugLevel(moveReminder);
                UpdateDrugTimer(moveReminder);
                currentPositionX = oldPositionX + x;
                currentPositionY = oldPositionY + y;
                animator.Play(direction + "Walk");
            }
            else
            {
                animator.Play(direction + "Idle");

                DrugLevel = Mathf.Round(DrugLevel);
                DrugTimer = Mathf.Round(DrugTimer);
                UpdateDrugLevel(0);
                UpdateDrugTimer(0);
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
            Drug1 = Drug2;
            Drug2 = DrugType.None;
        }

        //drugs
        if (CurrentActiveDrug != null)
        {
            CurrentActiveDrug.UpdateEffect();
        }

    }

    private void UpdateDrugLevel(float time)
    {
        DrugLevel -= moveSpeed * time;
        if (DrugLevel <= 0 || DrugLevel >= maxDrugLevel) // max en nombre de cases
        {
            //respawn
            SceneManager.LoadScene(Global.CurrentLevel);
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
        switch(Drug1){
            case DrugType.WhiteEye:
                DrugLevel += 10;
                DrugTimer = 6;
                CurrentActiveDrug = new WhiteEyeEffect();
                break;
            case DrugType.Thorn:
                DrugLevel += 10;
                DrugTimer = 6;
                CurrentActiveDrug = new ThornEffect();
                break;
            default:
                break;
        }

        if (Drug1 != DrugType.None)
        {
            CurrentActiveDrug.StartEffect();
        }
    }
}
