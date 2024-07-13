using System.Collections.Generic;
using UnityEngine;

public class GestionalGridGenerator : MonoBehaviour
{
    [Header("Map Variables")]
    public int gridWidth;
    public int gridHeight;
    public int tileCount;
    public Dictionary<GestionalTile, List<GestionalTile>> worldSectionTileMap;

    [Header("Single Tile Variables")]
    public int innerTileCount;
    public Material innerDefaultMaterial;
    [Header("Street")]
    public GameObject roadPrefab;
    public GameObject trafficLightPrefab;
    public GameObject streetlightPrefab;
    [Header("Green")]
    public GameObject grassPrefab;
    public GameObject bushPrefab;
    public int minBushQuantity;
    public int maxBushQuantity;
    public GameObject stonePrefab;
    public int minStoneQuantity;
    public int maxStoneQuantity;
    public GameObject treePrefab;
    public int minTreeQuantity;
    public int maxTreeQuantity;




    private void Start()
    {
        InitializeGrid();
    }

    public void InitializeGrid()
    {
        var initialTileList = GenerateGrid(gameObject, gridWidth, gridHeight, tileCount, innerDefaultMaterial);

        worldSectionTileMap = new Dictionary<GestionalTile, List<GestionalTile>>();
        Cursor.lockState = CursorLockMode.Confined;

        foreach (var singleTile in initialTileList)
        {
            worldSectionTileMap.Add(singleTile, GenerateGrid(singleTile.gameObject, (int)singleTile.squareDimension, (int)singleTile.squareDimension, innerTileCount, innerDefaultMaterial));
            singleTile.GetComponent<MeshRenderer>().enabled = false;
            singleTile.GetComponent<MeshCollider>().enabled = false;
        }
    }

    public List<GestionalTile> GenerateGrid(GameObject parent, int gridWidth, int gridHeight, int tileSquareCount, Material tileMaterial)
    {
        List<GestionalTile> resultList = new List<GestionalTile>();
        float tileSizeX = (float)gridWidth / tileSquareCount;
        float tileSizeZ = (float)gridHeight / tileSquareCount;

        for (int x = 0; x < tileSquareCount; x++)
        {
            for (int z = 0; z < tileSquareCount; z++)
            {
                Mesh newMesh = GenerateNewMesh(tileSizeX, tileSizeZ);
                GameObject newTile = new GameObject($"Tile_{x}_{z}");
                newTile.transform.SetParent(parent.transform);
                newTile.transform.localPosition = new Vector3(x * tileSizeX, 0, z * tileSizeZ);

                MeshFilter meshFilter = newTile.AddComponent<MeshFilter>();
                MeshRenderer meshRenderer = newTile.AddComponent<MeshRenderer>();
                MeshCollider meshCollider = newTile.AddComponent<MeshCollider>();

                meshRenderer.material = tileMaterial;
                meshFilter.mesh = newMesh;
                meshCollider.sharedMesh = newMesh;

                var gestionalTile = newTile.AddComponent<GestionalTile>();
                gestionalTile.squareDimension = tileSizeX;
                resultList.Add(gestionalTile);

                gestionalTile.tileCollider = meshCollider;
                gestionalTile.tileRenderer = meshRenderer;
                gestionalTile.gridGenerator = this;

                newTile.isStatic = true;



            }
        }

        return resultList;

    }
    public static Mesh GenerateNewMesh(float xPosition, float zPosition)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertces = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(xPosition,0,0),
            new Vector3(0,0,zPosition),
            new Vector3(xPosition,0,zPosition),
        };

        int[] triangles = new int[]
        {
            0,2,1,2,3,1
        };

        mesh.vertices = vertces;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;

    }


    public void DestroyTileMap()
    {
        foreach (GestionalTile bigTile in worldSectionTileMap.Keys)
        {
            Destroy(bigTile.gameObject);
        }

    }

    public List<GestionalTile> GetSmallTileListFromBigTileName(string name)
    {
        foreach (KeyValuePair<GestionalTile, List<GestionalTile>> entry in worldSectionTileMap)
        {         
            if (entry.Key.gameObject.name == name)
            {
                return entry.Value;
            }
        }
        return null; // No match found
    }


}




