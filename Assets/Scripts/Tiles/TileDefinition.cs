using UnityEngine;

namespace Tiles
{
    [CreateAssetMenu]
    public class TileDefinition : ScriptableObject
    {
        [SerializeField] TileType tileType;
        [SerializeField] TilePrefab prefab;
        [SerializeField] Vector3 rotation = Vector3.zero;
        [SerializeField] Vector3 scale = Vector3.one;

        public TileType TileType => tileType;
        public TilePrefab Prefab => prefab;
        public Quaternion Rotation => Quaternion.Euler(rotation);
        public Vector3 Scale => scale;

        public bool IsEndTile => tileType == TileType.N || tileType == TileType.E ||
                                 tileType == TileType.S || tileType == TileType.W;

        public bool IsEmptyTile => tileType == TileType.Empty;

        public bool Match(int cmp)
        {
            return ((int) tileType & cmp) > 0;
        }
    }
}