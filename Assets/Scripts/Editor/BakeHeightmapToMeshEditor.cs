using System.IO;
using Tiles;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

public class BakeHeightmapToMeshEditor : EditorWindow
{
    [MenuItem(itemName: "Tools/BakeHeightmapToMeshEditor")]
    public static void Init()
    {
        EditorWindow.CreateWindow<BakeHeightmapToMeshEditor>();
    }

    private Texture2D _heightMapTexture;
    private GameObject _baseTerrainMeshGameObject;
    private Mesh _baseCollisionMesh;
    private Material _meshMaterial;
    private Vector2 _displacementScale;
    
    private void OnGUI()
    {
        EditorGUILayout.LabelField("Height Texture Properties", EditorStyles.boldLabel);
        _heightMapTexture = (Texture2D)EditorGUILayout.ObjectField(
            "Heightmap Texture",
            _heightMapTexture,
            typeof(Texture2D),
            allowSceneObjects: false);
        _displacementScale = EditorGUILayout.Vector2Field("Displacement Scale", _displacementScale);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Mesh Properties", EditorStyles.boldLabel);
        _meshMaterial = (Material) EditorGUILayout.ObjectField(
            "Mesh Material",
            _meshMaterial,
            typeof(Material),
            allowSceneObjects: false);
        _baseCollisionMesh = (Mesh) EditorGUILayout.ObjectField(
            "Base Collision Mesh",
            _baseCollisionMesh,
            typeof(Mesh),
            allowSceneObjects: false);
        _baseTerrainMeshGameObject = (GameObject) EditorGUILayout.ObjectField(
            "Base Mesh",
            _baseTerrainMeshGameObject,
            typeof(GameObject),
            allowSceneObjects: false);

        if (
            GUILayout.Button("Bake Heightmap") &&
            _heightMapTexture != null &&
            _baseTerrainMeshGameObject != null &&
            _baseCollisionMesh != null
        ) {
            BakeHeightmap();
        }
    }

    private void BakeHeightmap()
    {
        var source = (GameObject)PrefabUtility.InstantiatePrefab(_baseTerrainMeshGameObject);

        var originalMeshFilter = _baseTerrainMeshGameObject.GetComponent<MeshFilter>();
        
        var meshCopy = CopyMesh(originalMeshFilter.sharedMesh);
        AdjustMeshBasedOnHeightMap(meshCopy, _heightMapTexture);
        meshCopy.name = "Mesh";

        var colliderMeshCopy = CopyMesh(_baseCollisionMesh);
        AdjustMeshBasedOnHeightMap(colliderMeshCopy, _heightMapTexture);
        colliderMeshCopy.name = "ColliderMesh";
        
        var meshDirectory = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(_baseTerrainMeshGameObject))}";
        var newObjectName = $"Tile_{_heightMapTexture.name}_{_displacementScale.x}_{_displacementScale.y}.prefab";
        var prefab = PrefabUtility.SaveAsPrefabAsset(source, Path.Combine(meshDirectory, "Tiles" ,newObjectName));
        
        var prefabMeshRenderer = prefab.GetComponent<MeshRenderer>();
        prefabMeshRenderer.material = _meshMaterial;
        
        var prefabMeshFilter = prefab.GetComponent<MeshFilter>();
        prefabMeshFilter.mesh = meshCopy;

        var prefabMeshCollider = prefab.AddComponent<MeshCollider>();
        prefabMeshCollider.sharedMesh = colliderMeshCopy;
        
        AssetDatabase.AddObjectToAsset(meshCopy, prefab); 
        AssetDatabase.AddObjectToAsset(colliderMeshCopy, prefab);

        AssetDatabase.SaveAssets();
        
        DestroyImmediate(source);
    }

    private void AdjustMeshBasedOnHeightMap(Mesh mesh, Texture2D texture)
    {
        var vertices = mesh.vertices;
        var uvs = mesh.uv;
        var vertexColors = new Color[vertices.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            var uv = uvs[i];
            var vertex = vertices[i];
            
            var color = texture.GetPixelBilinear(uv.x, uv.y, 0);

            vertex += Vector3.up * Mathf.Lerp(_displacementScale.x, _displacementScale.y, color.r);
            
            vertices[i] = vertex;
            vertexColors[i] = color;
        }

        mesh.vertices = vertices;
        mesh.colors = vertexColors;
     
        mesh.Optimize();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    private Mesh CopyMesh(Mesh baseMesh)
    {
        var mesh = new Mesh
        {
            vertices = baseMesh.vertices,
            triangles = baseMesh.triangles,
            uv = baseMesh.uv,
            normals = baseMesh.normals,
            colors = baseMesh.colors,
            tangents = baseMesh.tangents
        };
        
        return mesh;
    }
}
