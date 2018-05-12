using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    

    private float moveTimer = 0;
    private float moveSpeed = 1f;

    private int currentPositionX;
    private int currentPositionY;
    private int oldPositionX;
    private int oldPositionY;

    public AbstractDrugEffect CurrentActiveDrug { get; set; }
    public float DrugLevel { get; set; }
    public float DrugTimer { get; set; }

    private float withdrawalSpeed = 2;

    public DrugType Drug1 { get; set; }
    public DrugType Drug2 { get; set; }

    private GameObject level;

    // Use this for initialization
    void Start () {
        moveTimer = 0;

        currentPositionX = 0;
        currentPositionY = 0;

        oldPositionX = 0;
        oldPositionY = 0;

        CurrentActiveDrug = null;
        DrugLevel = 50;
        DrugTimer = 0;
        Drug1 = DrugType.None;
        Drug2 = DrugType.None;

        level = GameObject.FindGameObjectWithTag("Level");

    }
	
	// Update is called once per frame
	void Update () {
        DrugLevel -= withdrawalSpeed * Time.deltaTime;
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
            moveTimer = 0;
            oldPositionX = currentPositionX;
            oldPositionY = currentPositionY;

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
                    //TODO
                }
                level.GetComponent<Level>().RemoveDrug(currentPositionX, currentPositionY);
                
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
                moveTimer = moveSpeed;
                currentPositionX = oldPositionX + x;
                currentPositionY = oldPositionY + y;
            }
            
        }

        if (moveTimer > 0)
        {
            gameObject.transform.localPosition = Vector3.Lerp(new Vector3(oldPositionX, oldPositionY, 0), new Vector3(currentPositionX, currentPositionY, 0), moveSpeed - moveTimer);
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

    private DrugType CheckDrug(int x, int y)
    {
        var drug = level.GetComponent<Level>().GetTile(x, y).Drug;
        return drug;
    }

    private void UseDrug()
    {
        switch(Drug1){
            case DrugType.WhiteEye:
                DrugLevel += 30;
                DrugTimer = 10;
                CurrentActiveDrug = new WhiteEyeEffect(); ;
                CurrentActiveDrug.StartEffect();
                break;
            default:
                break;
        }
    }
}
