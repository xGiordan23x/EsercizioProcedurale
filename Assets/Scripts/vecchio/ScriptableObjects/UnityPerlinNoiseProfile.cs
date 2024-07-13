using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Noise Profile/ Unity Perlin Noise")]
public class UnityPerlinNoiseProfile : NoiseProfileBase
{
    [Min(1)]
    public int octaves = 4;

    [Range(0, 1)]
    public float persistance = 0.5f;

    [Min(1)]
    public float lacunarity = 2;

    public float maxAmplitude = 0.5f;




    public override float[,] GenerateNoiseMap(Vector2 position, int mapWidth, int mapHeight, float scale, Vector2 offset, int seed)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random random = new System.Random(seed);
        Vector2[] octaveOffset = new Vector2[octaves];

        float maxPossibleHeight =0;
        float amplitude = 1;
        float frequency = 1;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-10000, 10000) + offset.x;
            float offsetY = random.Next(-10000, 10000) + offset.y;

            octaveOffset[i] = new Vector2(offsetX, offsetY);
            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        if (scale < 0) scale = 0.0001f;

        float halfWidth = (float)mapWidth / 2;
        float halfHeight = (float)mapHeight / 2;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth + position.x) / scale * frequency + octaveOffset[i].x;
                    float sampleY = (y - halfHeight + position.y) / scale * frequency + octaveOffset[i].y;

                    float perlinValue = (Mathf.PerlinNoise(sampleX, sampleY) *2) -1;
                    noiseHeight += (perlinValue * amplitude);
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                noiseMap[x,y] =maxAmplitude * (noiseHeight +1)/2 *maxPossibleHeight;

            }
        }
        return noiseMap;
    }
}
