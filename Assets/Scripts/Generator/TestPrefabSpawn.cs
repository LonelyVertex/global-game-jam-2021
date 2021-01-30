using Tiles;
using UnityEngine;

namespace Generator
{
    public class TestPrefabSpawn : MonoBehaviour
    {
        [SerializeField] private TileDefinition tileDefinition;
    
        void Start()
        {
            if (!tileDefinition) return;
            var obj = Instantiate(tileDefinition.Prefab, Vector3.zero, tileDefinition.Rotation);
            obj.transform.localScale = tileDefinition.Scale;

        }
    }
}
