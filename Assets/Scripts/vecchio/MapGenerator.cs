using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private NoiseProfileBase noiseProfile;
    [SerializeField] private RegionProfileSo regionProfile;
    [SerializeField] private MapDisplay mapDisplay;

    public Vector2 position;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [Min(0.01f)]
    [SerializeField] private float noiseScale;
    [SerializeField] private Vector2 offset;
    public float terrainHeight =1;
    public float scale = 1;
    public int seed;

    [SerializeField] bool generateOnAwake;
    [SerializeField] bool regenerate;
   public void GenerateMap()
    {
        float[,] noisemap = noiseProfile.GenerateNoiseMap(position, width, height, noiseScale, offset, seed);

        Texture2D texture = TextureGenerator.GenerateTextureFromHeightMap(noisemap);
        mapDisplay.DrawTexture(texture);

        Texture2D terrainTexture = TextureGenerator.GenerateTextureFromColorMap(regionProfile.GetColorMap(noisemap),width,height);
        MeshData mdata = MeshGenerator.GenerateMesh(noisemap, terrainHeight, regionProfile.terrainCurve, scale);
        mapDisplay.DrawMesh(mdata, terrainTexture);
    }

    private void Awake()
    {
        if(generateOnAwake) GenerateMap();
    }
    private void OnValidate()
    {
        if(regenerate) GenerateMap();
    }
}
