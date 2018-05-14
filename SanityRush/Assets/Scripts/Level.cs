using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {
    
    public int startingDrugLevel;
    public Tile[,] TileMatrix { get; private set; }
    public int Size { get; private set; }
    private int maxSize = 200;
    private int offset;

    private GameObject[,] interactiveObjects;

    // Use this for initialization
    void Start () {

        Size = maxSize;
        TileMatrix = new Tile[Size, Size];
        interactiveObjects = new GameObject[Size, Size];
        offset = Size / 2;

        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                TileMatrix[i, j] = new Tile(i - offset, j - offset);
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            int x = Mathf.RoundToInt(child.transform.localPosition.x);
            int y = Mathf.RoundToInt(child.transform.localPosition.y);
            var tile = TileMatrix[offset + x, offset + y];

            if (child.gameObject.tag == "Floor")
            {
                tile.Solid = false;
                var thorn = child.GetComponent<Thorn>();
                if (thorn != null)
                {
                    tile.ThornSprite = thorn.ThornSprite;
                }
            }

            if (child.gameObject.tag == "Wall")
            {
                tile.Wall = true;
                var thorn = child.GetComponent<Thorn>();
                if (thorn != null)
                {
                    tile.ThornSprite = thorn.ThornSprite;
                }
            }

            if (child.gameObject.tag == "Stairs")
            {
                tile.StairsLevelIncrement = child.GetComponent<Stairs>().LevelIncrement;
                tile.Solid = false;
            }

            if (child.gameObject.tag != "Untagged" && child.gameObject.tag != "Drug" && child.gameObject.tag != "Guard")
            {
                tile.Object = child.gameObject;
                tile.BaseSprite = child.gameObject.GetComponent<SpriteRenderer>().sprite;

                var whiteeye = child.GetComponent<WhiteEye>();
                if (whiteeye != null)
                {
                    tile.WhiteEyeSprite = whiteeye.TrueSprite;
                }
            }

            if (child.gameObject.tag == "Drug")
            {
                tile.Drug = child.GetComponent<Drug>().Type;
                interactiveObjects[offset + x, offset + y] = child.gameObject;

                child.gameObject.GetComponent<Knight>().BaseSprite = child.GetComponent<SpriteRenderer>().sprite;
            }

            if (child.gameObject.tag == "Guard")
            {
                tile.Solid = true;
                tile.Guard = true;
                interactiveObjects[offset + x, offset + y] = child.gameObject;
                var dir = child.gameObject.GetComponent<Guard>().direction;
                switch (dir)
                {
                    case Direction.Right:
                        GetTile(tile.X + 1, tile.Y).Guarded = true;
                        GetTile(tile.X + 2, tile.Y).Guarded = true;
                        break;
                    case Direction.Up:
                        GetTile(tile.X, tile.Y + 1).Guarded = true;
                        GetTile(tile.X, tile.Y + 2).Guarded = true;
                        break;
                    case Direction.Left:
                        GetTile(tile.X - 1, tile.Y).Guarded = true;
                        GetTile(tile.X - 2, tile.Y).Guarded = true;
                        break;
                    case Direction.Down:
                        GetTile(tile.X, tile.Y - 1).Guarded = true;
                        GetTile(tile.X, tile.Y - 2).Guarded = true;
                        break;
                    default:
                        break;
                }

                child.gameObject.GetComponent<Guard>().GuardBaseSprite = child.gameObject.GetComponent<SpriteRenderer>().sprite;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Tile GetTile(int x, int y)
    {
        return TileMatrix[offset + x, offset + y];
    }

    public void RemoveDrug(int x, int y)
    {
        TileMatrix[offset + x, offset + y].Drug = DrugType.None;
        GameObject.Destroy(interactiveObjects[offset + x, offset + y]);
    }

    public GameObject GetInteractiveObject(int x, int y)
    {
        return interactiveObjects[offset + x, offset + y];
    }

    public void AddDrug(int x, int y, DrugType type)
    {
        TileMatrix[offset + x, offset + y].Drug = type;
        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("prefab/" + type.ToString()), new Vector3(x, y, -1), Quaternion.identity);
    }

    public void KillSkeleton(int x, int y)
    {
        var obj = interactiveObjects[offset + x, offset + y];
        obj.GetComponent<Animator>().Play("skeleton_back_death");

        GetTile(x, y).Solid = false;
        var dir = obj.GetComponent<Guard>().direction;
        switch (dir)
        {
            case Direction.Right:
                GetTile(x + 1, y).Guarded = false;
                GetTile(x + 2, y).Guarded = false;
                break;
            case Direction.Up:
                GetTile(x, y + 1).Guarded = false;
                GetTile(x, y + 2).Guarded = false;
                break;
            case Direction.Left:
                GetTile(x - 1, y).Guarded = false;
                GetTile(x - 2, y).Guarded = false;
                break;
            case Direction.Down:
                GetTile(x, y - 1).Guarded = false;
                GetTile(x, y - 2).Guarded = false;
                break;
            default:
                break;
        }
    }
}
