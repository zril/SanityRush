using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public Tile[,] TileMatrix { get; private set; }
    public int Size { get; private set; }
    private int maxSize = 1000;

    private GameObject[,] drugObjects;

    // Use this for initialization
    void Start () {

        Size = maxSize;
        TileMatrix = new Tile[Size, Size];
        drugObjects = new GameObject[Size, Size];

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                TileMatrix[i, j] = new Tile(i, j);
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            int x = Mathf.RoundToInt(child.transform.localPosition.x);
            int y = Mathf.RoundToInt(child.transform.localPosition.y);
            int offset = Size / 2;
            var tile = TileMatrix[offset + x, offset + y];

            if (child.gameObject.tag == "Floor")
            {
                tile.Solid = false;
            }

            if (child.gameObject.tag == "Drug")
            {
                tile.Drug = child.GetComponent<Drug>().Type;
                drugObjects[offset + x, offset + y] = child.gameObject;
            }

            if (child.gameObject.tag == "Stairs")
            {
                tile.StairsLevelIncrement = child.GetComponent<Stairs>().LevelIncrement;
                tile.Solid = false;
            }

            if (child.gameObject.tag != "Untagged")
            {
                tile.Object = child.gameObject;
                tile.BaseSprite = child.gameObject.GetComponent<SpriteRenderer>().sprite;

                var whiteeye = child.GetComponent<WhiteEye>();
                if (whiteeye != null)
                {
                    tile.WhiteEyeSprite = whiteeye.TrueSprite;
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Tile GetTile(int x, int y)
    {
        int offset = Size / 2;
        return TileMatrix[offset + x, offset + y];
    }

    public void RemoveDrug(int x, int y)
    {
        int offset = Size / 2;
        TileMatrix[offset + x, offset + y].Drug = DrugType.None;
        GameObject.Destroy(drugObjects[offset + x, offset + y]);
    }

    public void AddDrug(int x, int y, DrugType type)
    {
        int offset = Size / 2;
        TileMatrix[offset + x, offset + y].Drug = type;
        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/" + type.ToString()), new Vector3(x, y, -1), Quaternion.identity);
    }
}
