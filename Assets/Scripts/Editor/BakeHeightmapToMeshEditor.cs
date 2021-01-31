using System.IO;
using UnityEditor;
using UnityEngine;

public class BakeHeightmapToMeshEditor : EditorWindow
{
    [MenuItem(itemName: "Tools/BakeHeightmapToMeshEditor")]
    public static void Init()
    {
        CreateWindow<BakeHeightmapToMeshEditor>();
    }

    private GameObject _baseTerrainMeshGameObject;
    private Mesh _baseCollisionMesh;
    private Material _meshMaterial;
    private Vector2 _displacementScale;

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Height Texture Properties", EditorStyles.boldLabel);
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
            _baseTerrainMeshGameObject != null &&
            _baseCollisionMesh != null
        ) {
            BakeHeightmap();
        }
    }

    private void BakeHeightmap()
    {
        BakeSingleHeightmap(false, false, false, false, false, false);
        /* BakeSingleHeightmap(false, true, false, false, true, false);
        BakeSingleHeightmap(true, true, false, false, false, false);
        BakeSingleHeightmap(false, true, false, true, false, false);
        BakeSingleHeightmap(true, true, true, false, true, false);
        BakeSingleHeightmap(true, true, true, true, true, false);
        BakeSingleHeightmap(false, true, false, false, false, true); */
    }

    private void BakeSingleHeightmap(bool top, bool right, bool bottom, bool left, bool center, bool isBase)
    {
        var source = (GameObject)PrefabUtility.InstantiatePrefab(_baseTerrainMeshGameObject);

        var originalMeshFilter = _baseTerrainMeshGameObject.GetComponent<MeshFilter>();
        
        var meshCopy = CopyMesh(originalMeshFilter.sharedMesh);
        AdjustMeshBasedOnHeightMap(meshCopy, top, right, bottom, left, center, isBase);
        meshCopy.name = "Mesh";

        var colliderMeshCopy = CopyMesh(_baseCollisionMesh);
        AdjustMeshBasedOnHeightMap(colliderMeshCopy, top, right, bottom, left, center, isBase);
        colliderMeshCopy.name = "ColliderMesh";

        var topStr = top ? "T" : "";
        var rightStr = right ? "R" : "";
        var bottomStr = bottom ? "B" : "";
        var leftStr = left ? "L" : "";
        var centerStr = center ? "C" : "";
        var baseStr = isBase ? "Base" : "";
        
        var meshDirectory = $"{Path.GetDirectoryName(AssetDatabase.GetAssetPath(_baseTerrainMeshGameObject))}";
        var newObjectName = $"Tile_{topStr}{rightStr}{bottomStr}{leftStr}{centerStr}{baseStr}_{_displacementScale.x}_{_displacementScale.y}.prefab";
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

    private void AdjustMeshBasedOnHeightMap(Mesh mesh, bool top, bool right, bool bottom, bool left, bool center, bool isBase)
    {
        var vertices = mesh.vertices;
        var vertexColors = new Color[vertices.Length];

        var centerRadius = 10.0f;
        var rightRect = new Rect(0, -5, 19, 10);
        var topRect = new Rect(-5, -2, 10, 19);
        var leftRect = new Rect(-17, -5, 19, 10);
        var bottomRect = new Rect(-5, -17, 10, 19);
        var bigRectCenter = new Rect(-10, -10, 20, 20);
        
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = vertices[i];
            
            var color = Color.white;
            var newVertex = vertex + Vector3.up * _displacementScale.y;
            var vertexFlatPos = new Vector2(vertex.x, vertex.z);

            if (right)
            {
                var distToRightRaw = Mathf.InverseLerp(0, 4, DistancePointToRectangle(vertexFlatPos, rightRect));
                var distToRight = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 
                    1 - distToRightRaw);
                newVertex -= Vector3.up * distToRight;    
            }

            if (left)
            {
                var distToLeftRaw = Mathf.InverseLerp(0, 4, DistancePointToRectangle(vertexFlatPos, leftRect));
                var distToLeft = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 1 - distToLeftRaw);
                newVertex -= Vector3.up * distToLeft;
            }

            if (top)
            {
                var distToTopRaw = Mathf.InverseLerp(0, 4, DistancePointToRectangle(vertexFlatPos, topRect));
                var distToTop = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 1 - distToTopRaw);
                newVertex -= Vector3.up * distToTop;
            }

            if (bottom)
            {
                var distToBottomRaw = Mathf.InverseLerp(0, 4, DistancePointToRectangle(vertexFlatPos, bottomRect));
                var distToBottom = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 1 - distToBottomRaw);
                newVertex -= Vector3.up * distToBottom;    
            }

            if (center)
            {
                var distToCenterRaw = Mathf.InverseLerp(centerRadius, centerRadius + 4, Vector2.Distance(Vector3.zero, vertexFlatPos));
                var distToCenter = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 1 - distToCenterRaw);
                newVertex -= Vector3.up * distToCenter;    
            }

            if (isBase)
            {
                var distToBaseRaw = Mathf.InverseLerp(0, 4, DistancePointToRectangle(vertexFlatPos, bigRectCenter));
                var distToBase = Mathf.SmoothStep(_displacementScale.x, _displacementScale.y, 1 - distToBaseRaw);
                newVertex -= Vector3.up * distToBase;
            }
            
            newVertex = new Vector3(newVertex.x, Mathf.Max(0, newVertex.y), newVertex.z);
            
            if (rightRect.Contains(vertexFlatPos) && right)
            {
                color = Color.black;
            }

            if (leftRect.Contains(vertexFlatPos) && left)
            {
                color = Color.black;
            }

            if (topRect.Contains(vertexFlatPos) && top)
            {
                color = Color.black;
            }

            if (bottomRect.Contains(vertexFlatPos) && bottom)
            {
                color = Color.black;
            }

            if (Vector2.Distance(Vector2.zero, vertexFlatPos) < centerRadius && center)
            {
                color = Color.black;
            }

            if (bigRectCenter.Contains(vertexFlatPos) && isBase)
            {
                color = Color.black;
            }

            vertices[i] = newVertex;
            vertexColors[i] = color;
        }

        mesh.vertices = vertices;
        mesh.colors = vertexColors;
     
        mesh.Optimize();
        
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    // Taken from https://wiki.unity3d.com/index.php/Distance_from_a_point_to_a_rectangle
    private float DistancePointToRectangle(Vector2 point, Rect rect) {
        //  Calculate a distance between a point and a rectangle.
        //  The area around/in the rectangle is defined in terms of
        //  several regions:
        //
        //  O--x
        //  |
        //  y
        //
        //
        //        I   |    II    |  III
        //      ======+==========+======   --yMin
        //       VIII |  IX (in) |  IV
        //      ======+==========+======   --yMax
        //       VII  |    VI    |   V
        //
        //
        //  Note that the +y direction is down because of Unity's GUI coordinates.
 
        if (point.x < rect.xMin) { // Region I, VIII, or VII
            if (point.y < rect.yMin) { // I
                Vector2 diff = point - new Vector2(rect.xMin, rect.yMin);
                return diff.magnitude;
            }
            else if (point.y > rect.yMax) { // VII
                Vector2 diff = point - new Vector2(rect.xMin, rect.yMax);
                return diff.magnitude;
            }
            else { // VIII
                return rect.xMin - point.x;
            }
        }
        else if (point.x > rect.xMax) { // Region III, IV, or V
            if (point.y < rect.yMin) { // III
                Vector2 diff = point - new Vector2(rect.xMax, rect.yMin);
                return diff.magnitude;
            }
            else if (point.y > rect.yMax) { // V
                Vector2 diff = point - new Vector2(rect.xMax, rect.yMax);
                return diff.magnitude;
            }
            else { // IV
                return point.x - rect.xMax;
            }
        }
        else { // Region II, IX, or VI
            if (point.y < rect.yMin) { // II
                return rect.yMin - point.y;
            }
            else if (point.y > rect.yMax) { // VI
                return point.y - rect.yMax;
            }
            else { // IX
                return 0f;
            }
        }
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
