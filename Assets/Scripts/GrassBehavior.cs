using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GrassBehavior : MonoBehaviour
{
    float squareDimension;
    private TileType tileType;
    private GestionalTile tile;

    private List<GameObject> nature = new List<GameObject>();
    private List<GameObject> trees = new List<GameObject>();
    private List<GameObject> stones = new List<GameObject>();
    [Header("References")]
    public MeshRenderer rend;


    internal void SetVariables(float squareDimension, GestionalTile gestionalTile)
    {
        this.squareDimension = squareDimension;
        rend = GetComponentInChildren<MeshRenderer>();
        this.tile = gestionalTile;
       

        transform.localPosition = new Vector3(transform.localPosition.x + squareDimension / 2, transform.localPosition.y, transform.localPosition.z + squareDimension / 2);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void SetNature()
    {
        //set bushes
        int randomBushValue = UnityEngine.Random.Range(tile.gridGenerator.minBushQuantity, tile.gridGenerator.maxBushQuantity);
        
        for (int i = 0; i < randomBushValue ; i++)
        {
            GameObject bush = Instantiate(tile.gridGenerator.bushPrefab,Vector3.zero, Quaternion.identity,tile.transform);
            bush.transform.localPosition = GetRandomMeshPoint();
            bush.isStatic = true;           
            nature.Add(bush);
        }
        //set stones
        for (int i = 0; i < UnityEngine.Random.Range(tile.gridGenerator.minStoneQuantity, tile.gridGenerator.maxStoneQuantity); i++)
        {
            GameObject stone = Instantiate(tile.gridGenerator.stonePrefab, Vector3.zero, Quaternion.identity, tile.transform);
            stone.transform.localPosition = GetRandomMeshPoint();
            stone.isStatic = true;
            nature.Add(stone);
        }
        //set trees
        for (int i = 0; i < UnityEngine.Random.Range(tile.gridGenerator.minTreeQuantity, tile.gridGenerator.maxTreeQuantity); i++)
        {
            GameObject tree = Instantiate(tile.gridGenerator.treePrefab, Vector3.zero, Quaternion.identity, tile.transform);
            tree.transform.localPosition = GetRandomMeshPoint();
            tree.isStatic = true;
            nature.Add(tree);
        }
    }

    public Vector3 GetRandomMeshPoint()
    {
        Bounds bounds = tile.tileRenderer.bounds;

        float x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x) - tile.transform.position.x;
        float y =tile.squareDimension/20; // Or some height if needed
        float z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z) - tile.transform.position.z;

        Vector3 randomPoint = new Vector3(x, y, z);
        return randomPoint;


       
        
    }

    internal void DestroyNature()
    {
        if (nature != null && nature.Count > 0)
        {
            foreach (var nature in nature)
            {
                Destroy(nature);
            }
        }
    }
}
