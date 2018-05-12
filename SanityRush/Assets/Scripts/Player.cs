using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    

    private float moveTimer = 0;
    private float moveReminder = 0;
    private float moveSpeed = 2.5f;

    private int currentPositionX;
    private int currentPositionY;
    private int oldPositionX;
    private int oldPositionY;

    public AbstractDrugEffect CurrentActiveDrug { get; set; }
    public float DrugLevel { get; set; }
    public float DrugTimer { get; set; }

    private float withdrawalSpeed = 5;

    public DrugType Drug1 { get; set; }
    public DrugType Drug2 { get; set; }

    private GameObject level;

    private bool pickup; // pour ne pas ramasser la pillule en boucle

    // Use this for initialization
    void Start () {
        moveTimer = 0;

        currentPositionX = 0;
        currentPositionY = 0;

        oldPositionX = 0;
        oldPositionY = 0;

        level = GameObject.FindGameObjectWithTag("Level");

        CurrentActiveDrug = null;
        DrugLevel = level.GetComponent<Level>().startingDrugLevel;
        DrugTimer = 0;
        Drug1 = DrugType.None;
        Drug2 = DrugType.None;


        pickup = false;

    }
	
	// Update is called once per frame
	void Update () {
        DrugLevel -= withdrawalSpeed * Time.deltaTime;
        if (DrugLevel < 0 || DrugLevel > 100)
        {
            //respawn
            SceneManager.LoadScene(Global.CurrentLevel);
        }

        if (DrugTimer > 0)
        {
            DrugTimer -= Time.deltaTime;
        } else
        {
            DrugTimer = 0;
            if (CurrentActiveDrug != null)
            {
                CurrentActiveDrug.EndEffect();
                CurrentActiveDrug = null;
            }
        }
        

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
                    if (!pickup)
                    {
                        Drug2 = drug;
                        level.GetComponent<Level>().AddDrug(currentPositionX, currentPositionY, Drug2);
                    }
                }
                level.GetComponent<Level>().RemoveDrug(currentPositionX, currentPositionY);
                pickup = true;
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
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                x = -1;
                move = true;
            }
            else if (Input.GetAxis("Vertical") > 0)
            {
                y = 1;
                move = true;
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                y = -1;
                move = true;
            }

            var solid = CheckSolid(oldPositionX + x, oldPositionY + y);

            if (move && !solid)
            {
                moveTimer = -moveReminder + (1 / moveSpeed);
                currentPositionX = oldPositionX + x;
                currentPositionY = oldPositionY + y;
                pickup = false;
            }
            
        }

        if (moveTimer > 0)
        {
            gameObject.transform.localPosition = Vector3.Lerp(new Vector3(oldPositionX, oldPositionY, -1), new Vector3(currentPositionX, currentPositionY, -1), moveSpeed * ((1 / moveSpeed) - moveTimer));
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

    private bool CheckSolid(int x, int y)
    {
        var tile = level.GetComponent<Level>().GetTile(x, y);
        return tile.Solid;
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
                DrugLevel += 30;
                DrugTimer = 10;
                CurrentActiveDrug = new WhiteEyeEffect();
                break;
            case DrugType.Thorn:
                DrugLevel += 30;
                DrugTimer = 10;
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
