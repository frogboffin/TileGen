using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Tile {

}

[System.Serializable]
public class TileType {

    public string name;
    public GameObject tileVisual;
    public bool isWalkable;

    public TileType(string tileName, GameObject tileVis, bool walkable)
    {
        name = tileName;
        tileVisual = tileVis;
        isWalkable = walkable;
    }
}

static public class TestMaps {
    public static int[,] LoadLevel(string fName, int mapX, int mapY)
    {
        int[,] map = new int[mapX, mapY];
        StreamReader reader = new StreamReader(fName);

        for (int y = 0; y < mapY; y++)
        {
            for (int x = 0; x < mapX; x++)
            {
                switch(reader.Read())
                {
                    case 48: map[x, y] = 0;
                        break;
                    case 49: map[x, y] = 1;
                        break;
                    case 50: map[x, y] = 2;
                        break;
                }
            }
        }
        return map;
    }

}

public class Node {
    public List<Node> neighbours = new List<Node>(); //Edges
    bool passable;
    public Vector3 worldPos;

    public float DistanceTo(Node other)
    {
        return Vector2.Distance(worldPos, other.worldPos);
    }
}