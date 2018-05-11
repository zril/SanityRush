using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    private Tile[,] tileMatrix;
    private int sizeMax = 1000;

    // Use this for initialization
    void Start () {

        tileMatrix = new Tile[sizeMax, sizeMax];
        for(int i = 0; i < sizeMax; i++)
        {
            for (int j = 0; j < sizeMax; j++)
            {
                tileMatrix[i, j] = new Tile(i, j);
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            int x = Mathf.RoundToInt(child.transform.localPosition.x);
            int y = Mathf.RoundToInt(child.transform.localPosition.y);
            int offset = sizeMax / 2;

            if (child.gameObject.tag == "Corridor")
            {
                tileMatrix[offset + x, offset + y].Solid = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Tile GetTile(int x, int y)
    {
        int offset = sizeMax / 2;
        return tileMatrix[offset + x, offset + y];
    }
}
