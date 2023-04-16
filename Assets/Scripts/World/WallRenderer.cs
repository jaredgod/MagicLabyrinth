using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    U,
    R,
    D,
    L,
}

public class WallRenderer : MonoBehaviour
{

    public GameObject[] Tile;


    private Dictionary<Vector2Int, GameObject> gameObjectList;
    private Dictionary<Vector2Int, int> tileList;



    // Start is called before the first frame update
    void Start()
    {
        gameObjectList = new Dictionary<Vector2Int, GameObject>();
        tileList = new Dictionary<Vector2Int, int>();
    }

    public void placeTile(int x, int y, int tileIndex)
    {
        Vector2Int pos = new Vector2Int(x, y);
        if (!tileList.ContainsKey(pos))
        {
            GameObject newTile = Instantiate(Tile[tileIndex], transform);
            newTile.transform.SetPositionAndRotation(new Vector3(x, 0, y), new Quaternion());

            gameObjectList.Add(pos, newTile);
            tileList.Add(pos, tileIndex);
        }
    }

    public void changeTile(int x, int y, int t)
    {
        Vector2Int pos = new Vector2Int(x, y);
        if (tileList.ContainsKey(pos))
        {
            tileList[pos] = t;
        }
        if (gameObjectList.ContainsKey(pos))
        {
            gameObjectList.Remove(pos);
            GameObject newTile = Instantiate(Tile[t], transform);
            newTile.transform.SetPositionAndRotation(new Vector3(x, 0, y), new Quaternion());

            gameObjectList.Add(pos, newTile);
        }
    }

    public void placeRoom(Room room)
    {
        for (int i = 0; i < room.RoomSize; i++)
        {
            for (int j = 0; j < room.RoomSize; j++)
            {
                (int x, int y) = rotate(i, j, room.Rotation);
                int t = ((int)room.Tiles[i][j]);
                for (int ind = 0; ind < room.Rotation; ind++)
                {
                    t = t * 2;
                    t = t > 15 ? t + 1 : t;
                    t = t % 16;
                }

                placeTile(room.X + x, room.Y + y, t);
            }
        }
    }

    private (int, int) rotate(int x, int y, int rotation)
    {
        if (rotation == 0) return (x, y);
        if (rotation == 1) return (y, 3 - x);
        if (rotation == 2) return (3 - x, 3 - y);
        if (rotation == 3) return (3 - y, x);
        return (0, 0);
    }

    public bool canMove(Direction direction, int x, int y)
    {
        Vector2Int pos = new Vector2Int(x, y);
        int tile = tileList.ContainsKey(pos) ? tileList[pos] : -1;


        if (tile == -1) return false;
        if (direction == Direction.U) return tile % 2 == 0;
        if (direction == Direction.R) return tile % 4 < 2;
        if (direction == Direction.D) return tile % 8 < 4;
        if (direction == Direction.L) return tile < 8;

        return false;
    }

    public void clear() {
        foreach(GameObject g in gameObjectList.Values) {
            Destroy(g);
        }
        gameObjectList.Clear();
        tileList.Clear();
    }
}
