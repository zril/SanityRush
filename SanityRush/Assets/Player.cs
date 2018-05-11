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


    // Use this for initialization
    void Start () {
        moveTimer = 0;

        currentPositionX = 0;
        currentPositionY = 0;

        oldPositionX = 0;
        oldPositionY = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (moveTimer > 0)
        {
            moveTimer -= Time.deltaTime;
        } else
        {
            moveTimer = 0;
            oldPositionX = currentPositionX;
            oldPositionY = currentPositionY;
        }

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
        
    }

    private bool CheckSolid(int x, int y)
    {
        var level = GameObject.FindGameObjectWithTag("Level");
        var tile = level.GetComponent<Level>().GetTile(x, y);

        return tile.Solid;
    }
}
