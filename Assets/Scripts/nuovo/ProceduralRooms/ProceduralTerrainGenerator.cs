using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PerlinSettings
{
    public float minHeight = 0;
    public float maxHeight = 10;
    public int seed;
    public int maxRandomness = 1000;
    public float scale = 20;
    public bool useLerp;
    public bool subtract;
}
public class ProceduralTerrainGenerator : MonoBehaviour
{
    [SerializeField] Terrain terrain;
    [SerializeField] int width = 300;
    [SerializeField] int height = 20;
    [SerializeField] int lenght = 300;


    [Header("Perlin Layers")]
    [SerializeField] List<PerlinSettings> perlinSettings;

    [Header("Enviroment Objects")]
    [SerializeField] List<GameObject> treesPrefabsList;
    [SerializeField] List<GameObject> rocksPrefabsList;
    [SerializeField] List<GameObject> bushesPrefabsList;

    [SerializeField] float treesDesnity = 0.2f;
    [SerializeField] float rocksDensity = 0.4f;
    [SerializeField] float bushesDesnity = 0.3f;

    [SerializeField] float treesNoiseScale = 0.1f;
    [SerializeField] float rocksNoiseScale = 0.1f;
    [SerializeField] float bushesNoiseScale = 0.1f;




    private void OnValidate()
    {
        terrain.terrainData = GenerateTerrain(terrain.terrainData);
    }
    private void Start()
    {
        PopulateTerrain(terrain.terrainData);
    }



    private void PopulateTerrain(TerrainData terrainData)
    {
        bool[,] objectMap = new bool[width, lenght];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < lenght; z++)
            {
                float currentHeight = terrainData.GetHeight(x, z);

                Vector3 spawnPosition = new Vector3(x, currentHeight, z);

                float treeNoise = Mathf.PerlinNoise(x * treesNoiseScale, z * treesNoiseScale);
                float rockNoise = Mathf.PerlinNoise(x * rocksNoiseScale, z * rocksNoiseScale);
                float bushNoise = Mathf.PerlinNoise(x * bushesNoiseScale, z * bushesNoiseScale);

                if (treeNoise < treesDesnity)
                {
                    SpawnObjectOnMap(objectMap, x, z, spawnPosition, treesPrefabsList);
                }
                if (rockNoise < rocksDensity)
                {
                    SpawnObjectOnMap(objectMap, x, z, spawnPosition, rocksPrefabsList);
                }
                if (bushNoise < bushesDesnity)
                {
                    SpawnObjectOnMap(objectMap, x, z, spawnPosition, bushesPrefabsList);
                }
            }
        }
    }


    private void SpawnObjectOnMap(bool[,] objectMap, int x, int z, Vector3 spawnPosition, List<GameObject> prefabList)
    {
        if (!objectMap[x, z])
        {
            GameObject newObject = Instantiate(prefabList[UnityEngine.Random.Range(0, prefabList.Count)]);
            newObject.transform.position = spawnPosition;
            objectMap[x, z] = true;
        }
    }

    private TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width;
        terrainData.size = new Vector3(width, height, lenght);
        float[,] heights = new float[width, lenght];

        foreach (PerlinSettings settings in perlinSettings)
        {
            HeightsPerlin(ref heights, width, lenght, settings.scale, settings.minHeight, settings.maxHeight, settings.seed, settings.maxRandomness, settings.useLerp, settings.subtract);
        }


        terrainData.SetHeights(0, 0, heights);
        return terrainData;
    }
    private void HeightsPerlin(ref float[,] heights, int width, int lenght, float scale, float minHeight, float maxHeight, int seed, int maxRandomness, bool useLerp, bool subtract)
    {

        System.Random random = new System.Random(seed);
        float offsetX = random.Next(0, maxRandomness);
        float offsetY = random.Next(0, maxRandomness);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < lenght; z++)
            {
                float xCoordinate = (float)x / width * scale + offsetX;
                float yCoordinate = (float)z / lenght * scale + offsetY;
                float perlineHeightsValue = Mathf.PerlinNoise(xCoordinate, yCoordinate);


                if (useLerp)
                    heights[x, z] += subtract ? -Mathf.Lerp(minHeight, maxHeight, perlineHeightsValue) : Mathf.Lerp(minHeight, maxHeight, perlineHeightsValue);

                else
                    heights[x, z] += subtract ? -Mathf.Clamp(perlineHeightsValue, minHeight, maxHeight) : Mathf.Clamp(perlineHeightsValue, minHeight, maxHeight);

            }
        }


    }
}
