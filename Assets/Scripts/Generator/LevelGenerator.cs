using System.Collections.Generic;
using System.Linq;
using GamePlay;
using ModestTree;
using Tiles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Generator
{
    public class LevelGenerator : MonoBehaviour
    {
        const int N = 8;
        const int E = 4;
        const int S = 2;
        const int W = 1;
        const float TileSize = 32f;

        [Header("Tiles")] [SerializeField] List<TileDefinition> bases;
        [SerializeField] List<TileDefinition> tiles;

        [Inject] MineTrapGenerator mineTrapGenerator;
        [Inject] ResourceBoxGenerator resourceBoxGenerator;

        int gridSize;
        int minTiles;
        int maxTiles;
        int boxCount;
        int mineCount;
        int[,] grid;
        int tilesPlaced;
        Queue<(int, int)> queue;
        List<Transform> boxPoints;
        List<Transform> minePoints;
        List<GameObject> spawnedObjects;
        
        int Middle => Mathf.FloorToInt(gridSize / 2.0f);

        public Vector3 SpawnPoint => new Vector3(Middle * TileSize, 1, -Middle * TileSize);

        public void Generate(GameConfiguration.LevelConfiguration levelConfiguration)
        {
            Init(levelConfiguration);

            PlaceBase();
            PlaceTiles();
            FillBlankTiles();

            PlaceAllObjects();
        }

        // Initialization

        void Init(GameConfiguration.LevelConfiguration levelConfiguration)
        {
            if (spawnedObjects != null && spawnedObjects.Count > 0)
            {
                foreach (var spawnedObject in spawnedObjects)
                {
                    Destroy(spawnedObject);
                }
            }
            
            gridSize = levelConfiguration.GridSize;
            minTiles = levelConfiguration.MinTiles;
            maxTiles = levelConfiguration.MaxTiles;
            boxCount = levelConfiguration.BoxCount;
            mineCount = levelConfiguration.MineCount;

            InitGrid();
            tilesPlaced = 0;
            queue = new Queue<(int, int)>();
            boxPoints = new List<Transform>();
            minePoints = new List<Transform>();
            spawnedObjects = new List<GameObject>();
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

        // Tiles placement

        void PlaceBase()
        {
            var baseTile = PickRandomTile(bases);
            InstantiateTile(baseTile, Middle, Middle);
            QueueNextTiles(baseTile, Middle, Middle);
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

        void InstantiateTile(TileDefinition tileDefinition, int top, int left)
        {
            grid[top, left] = (int) tileDefinition.TileType;
            tilesPlaced++;

            var obj = Instantiate(tileDefinition.Prefab, new Vector3(left * TileSize, 0, -top * TileSize),
                tileDefinition.Rotation);
            obj.transform.localScale = tileDefinition.Scale;
            spawnedObjects.Add(obj.gameObject);

            var prefab = obj.GetComponent<TilePrefab>();
            if (prefab.BoxPoints != null)
            {
                boxPoints.AddRange(prefab.BoxPoints);
            }

            if (prefab.MinePoints != null)
            {
                minePoints.AddRange(prefab.MinePoints);
            }
        }

        void QueueNextTiles(TileDefinition tileDefinition, int top, int left)
        {
            if (IsNotMaxEdge(top) && tileDefinition.Match(S)) queue.Enqueue((top + 1, left));
            if (IsNotMinEdge(top) && tileDefinition.Match(N)) queue.Enqueue((top - 1, left));
            if (IsNotMaxEdge(left) && tileDefinition.Match(E)) queue.Enqueue((top, left + 1));
            if (IsNotMinEdge(left) && tileDefinition.Match(W)) queue.Enqueue((top, left - 1));
        }

        // Object placement

        void PlaceAllObjects()
        {
            resourceBoxGenerator.SpawnBoxes(boxPoints.OrderBy(p => Random.value).Take(boxCount));
            mineTrapGenerator.SpawnMines(minePoints.OrderBy(p => Random.value).Take(mineCount));
        }

        // Utils

        static TileDefinition PickRandomTile(List<TileDefinition> tileList)
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
}
