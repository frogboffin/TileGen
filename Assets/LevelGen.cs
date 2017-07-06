using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class LevelGen : MonoBehaviour {

    //int mapSizeX = 31;
    //int mapSizeY = 22;

    //int mapSizeX = 500;
    //int mapSizeY = 500;
    public int depth = 20;
    public int limit = 10000;
    public GameObject[] tileObjects;
    public int mapSize = 100;
    public int tileAmount = 1000;
    int tileAmountHidden;
    TileType[] tileTypes;
    int[,] tiles;
    //List<Vector2> tiles;
    public GameObject grassPrefab;
    public GameObject sandPrefab;
    public GameObject waterPrefab;

    List<Vector3> trees = new List<Vector3>();
    public GameObject treePrefab;

    public Text depthText;
    public Text gameObjectsText;
    public Slider slider;
    public Button go;

	// Use this for initialization
	void Start () 
    {
        go.onClick.AddListener(() => Go());
        CreateLevel();
	}
	
	// Update is called once per frame
	void Update () 
    {
        depth = (int)slider.value;
        depthText.text = depth.ToString();

        gameObjectsText.text = "GameObjects - " + GameObject.FindGameObjectsWithTag("Tile").Length;
        tileObjects = GameObject.FindGameObjectsWithTag("Tile");
	}

    void Go()
    {
        CreateLevel();
        StartCoroutine("GenerateIsland", slider.value);
    }

    void DestroyWorld()
    {
        for (int i = 0; i < tileObjects.Length; i++)
        {
            Destroy(tileObjects[i]);
        }
    }

    bool IsClose(Vector3 p)
    {
        foreach(Vector3 position in trees)
        {
            if ((p - position).magnitude < 5)
            {
                return true;
            }
        }
        return false;
    }

    void CreateLevel()
    {
        // Initialises tile types
        tileTypes = new TileType[3];
        tileTypes[0] = new TileType("Grass", grassPrefab, true);
        tileTypes[1] = new TileType("Sand", sandPrefab, true);
        tileTypes[2] = new TileType("Water", waterPrefab, false);

        DestroyWorld();
        tiles = new int[mapSize, mapSize];

        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                tiles[x, y] = 2;
            }
        }
    }

    void SpawnLevel()
    {
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (IsGrass(new Vector2(x, y)))
                {
                    tiles[x, y] = 0;
                }

                if (!CanCull(new Vector2(x, y)))
                    Instantiate(tileTypes[tiles[x, y]].tileVisual, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }


    void AttemptToPlace(Vector2 p)
    {

        int dir = Random.Range(1, 5);

        switch (dir)
        {
            case 1: //right
                p.x++;
                break;
            case 2: //down
                p.y++;
                break;
            case 3:
                p.x--; //left
                break;
            case 4:
                p.y--; //up
                break;
        }

        try
        {
        //if (p.y + 1 < mapSize)
            tiles[(int)p.x, (int)p.y + 1] = 1;

        //if (p.y - 1 >= 0)
            tiles[(int)p.x, (int)p.y - 1] = 1;

        //if (p.x + 1 < mapSize)
            tiles[(int)p.x + 1, (int)p.y] = 1;

        //if (p.x - 1 >= 0)
            tiles[(int)p.x - 1, (int)p.y] = 1;
        }
        catch
        {
           // do nothing
        }


        //int r = Random.Range(0, mapSize);
        //if (r == 0)
        //    p = new Vector2((mapSize / 2) - 1, (mapSize / 2) - 1);


        tileAmountHidden--;

        if (tileAmountHidden > 0)
            AttemptToPlace(p);

    }

    bool IsGrass(Vector2 p)
    {
        int sandAmount = Random.Range(1, 4);
        try
        {
            if (tiles[(int)p.x + sandAmount, (int)p.y + sandAmount] != 2 && tiles[(int)p.x - sandAmount, (int)p.y - sandAmount] != 2 && tiles[(int)p.x + sandAmount, (int)p.y - sandAmount] != 2 && tiles[(int)p.x - sandAmount, (int)p.y + sandAmount] != 2)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    bool CanCull(Vector2 p)
    {
        try
        {
            if (tiles[(int)p.x + 1, (int)p.y] == 2 && tiles[(int)p.x - 1, (int)p.y] == 2 && tiles[(int)p.x, (int)p.y - 1] == 2 && tiles[(int)p.x, (int)p.y + 1] == 2)
                return true;
            else
                return false;
        }
        catch
        {
            return true;
        }
    }

    IEnumerator GenerateIsland(int depth)
    {
        while (depth > 0)
        {
            Resources.UnloadUnusedAssets();
            tileAmountHidden = tileAmount;
            AttemptToPlace(new Vector2(Random.Range(1, mapSize - 1), Random.Range(1, mapSize - 1)));
            SpawnLevel();
            depth--;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
