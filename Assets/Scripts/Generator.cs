using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
    const int N = 8;
    const int E = 4;
    const int S = 2;
    const int W = 1;
    const float TileSize = 32f;

    [Header("Settings")] [SerializeField] int sGridSize;
    [SerializeField] int sMinTiles;
    [SerializeField] int sMaxTiles;

    [Header("Tiles")] [SerializeField] List<Tile> bases;
    [SerializeField] List<Tile> tiles;

    int gridSize;
    int minTiles;
    int maxTiles;
    int[,] grid;
    int tilesPlaced;
    Queue<(int, int)> queue;

    void Start()
    {
        Generate(sGridSize, sMinTiles, sMaxTiles);
    }

    void Generate(int newGridSize, int newMinTiles, int newMaxTiles)
    {
        Init(newGridSize, newMinTiles, newMaxTiles);

        PlaceBase();
        PlaceTiles();
        FillBlankTiles();
    }


    void Init(int newGridSize, int newMinTiles, int newMaxTiles)
    {
        gridSize = newGridSize;
        minTiles = newMinTiles;
        maxTiles = newMaxTiles;

        InitGrid();
        tilesPlaced = 0;
        queue = new Queue<(int, int)>();
    }

    void InitGrid()
    {
        grid = new int[gridSize, gridSize];
        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                grid[i, j] = -1;
            }
        }
    }

    void PlaceBase()
    {
        var middle = Mathf.CeilToInt(gridSize / 2.0f);
        var baseTile = PickRandomTile(bases);

        InstantiateTile(baseTile, middle, middle);
        QueueNextTiles(baseTile, middle, middle);
    }

    void PlaceTiles()
    {
        while (!queue.IsEmpty())
        {
            var (top, left) = queue.Dequeue();
            if (TilePlacedAt(top, left)) continue;

            PlaceTile(top, left);
        }
    }

    void PlaceTile(int top, int left)
    {
        var possibleTiles = tiles.Where(t => true);

        // from top
        if (IsNotMinEdge(top) && TilePlacedAt(top - 1, left))
        {
            var topTile = grid[top - 1, left];
            possibleTiles = ItoB(topTile & S)
                ? possibleTiles.Where(tile => tile.Match(N))
                : possibleTiles.Where(tile => !tile.Match(N));
        }
        else if (top == 0)
        {
            possibleTiles = possibleTiles.Where(tile => !tile.Match(N));
        }

        // from right
        if (IsNotMaxEdge(left) && TilePlacedAt(top, left + 1))
        {
            var rightTile = grid[top, left + 1];
            possibleTiles = ItoB(rightTile & W)
                ? possibleTiles.Where(tile => tile.Match(E))
                : possibleTiles.Where(tile => !tile.Match(E));
        }
        else if (left == gridSize - 1)
        {
            possibleTiles = possibleTiles.Where(tile => !tile.Match(E));
        }

        // from bot
        if (IsNotMaxEdge(top) && TilePlacedAt(top + 1, left))
        {
            var botTile = grid[top + 1, left];
            possibleTiles = ItoB(botTile & N)
                ? possibleTiles.Where(tile => tile.Match(S))
                : possibleTiles.Where(tile => !tile.Match(S));
        }
        else if (top == gridSize - 1)
        {
            possibleTiles = possibleTiles.Where(tile => !tile.Match(S));
        }

        // from left
        if (IsNotMinEdge(left) && TilePlacedAt(top, left - 1))
        {
            var leftTitle = grid[top, left - 1];
            possibleTiles = ItoB(leftTitle & E)
                ? possibleTiles.Where(tile => tile.Match(W))
                : possibleTiles.Where(tile => !tile.Match(W));
        }
        else if (left == 0)
        {
            possibleTiles = possibleTiles.Where(tile => !tile.Match(W));
        }

        // closing
        if (tilesPlaced < minTiles && possibleTiles.Any(tile => !tile.IsEndTile))
        {
            possibleTiles = possibleTiles.Where(tile => !tile.IsEndTile);
        }
        else if (tilesPlaced > maxTiles && possibleTiles.Any(tile => tile.IsEndTile))
        {
            possibleTiles = possibleTiles.Where(tile => tile.IsEndTile);
        }

        if (possibleTiles.Count() == 0)
        {
            Debug.LogWarning("Out of tiles");
        }

        var selectedTile = PickRandomTile(possibleTiles.ToList());
        InstantiateTile(selectedTile, top, left);
        QueueNextTiles(selectedTile, top, left);
    }

    void FillBlankTiles()
    {
        var possibleTiles = tiles.Where(tile => tile.IsEmptyTile).ToList();

        for (var i = 0; i < gridSize; i++)
        {
            for (var j = 0; j < gridSize; j++)
            {
                if (grid[i, j] == -1)
                {
                    var selectedTile = PickRandomTile(possibleTiles);
                    InstantiateTile(selectedTile, i, j);
                }
            }
        }
    }

    void InstantiateTile(Tile tile, int top, int left)
    {
        grid[top, left] = (int) tile.TileType;
        tilesPlaced++;

        var obj = Instantiate(tile.Prefab, new Vector3(left * TileSize, 0, -top * TileSize), tile.Rotation);
        obj.transform.localScale = tile.Scale;
    }

    void QueueNextTiles(Tile tile, int top, int left)
    {
        if (IsNotMaxEdge(top) && tile.Match(S)) queue.Enqueue((top + 1, left));
        if (IsNotMinEdge(top) && tile.Match(N)) queue.Enqueue((top - 1, left));
        if (IsNotMaxEdge(left) && tile.Match(E)) queue.Enqueue((top, left + 1));
        if (IsNotMinEdge(left) && tile.Match(W)) queue.Enqueue((top, left - 1));
    }


    // utils

    static Tile PickRandomTile(List<Tile> tileList)
    {
        return tileList[Random.Range(0, tileList.Count)];
    }

    static bool ItoB(int i)
    {
        return i > 0;
    }

    static bool IsNotMinEdge(int i)
    {
        return i - 1 >= 0;
    }

    bool IsNotMaxEdge(int i)
    {
        return i + 1 < gridSize;
    }

    bool TilePlacedAt(int top, int left)
    {
        return grid[top, left] > -1;
    }
}