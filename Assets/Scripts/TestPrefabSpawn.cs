using UnityEngine;

public class TestPrefabSpawn : MonoBehaviour
{
    [SerializeField] private Tile tile;
    
    void Start()
    {
        if (!tile) return;
        var obj = Instantiate(tile.Prefab, Vector3.zero, tile.Rotation);
        obj.transform.localScale = tile.Scale;

    }
}
