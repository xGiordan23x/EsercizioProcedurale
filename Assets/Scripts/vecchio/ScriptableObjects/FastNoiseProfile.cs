using UnityEngine;

[CreateAssetMenu(menuName = "Noise Profile/Fast Noise")]
public class FastNoiseProfile : NoiseProfileBase
{
    [Min(1)]
    public int octaves = 4;
    [Range(0f, 1f)]
    public float persistance = 0.5f;
    [Min(1)]
    public float lacunarity = 2;
    [Min(0)]
    public float frequency = 0.01f;

    public FastNoiseLite.NoiseType noiseType;
    public FastNoiseLite.FractalType fractalType;

    public float noiseAmplitude = 1f;
    public float noiseYOffset = 0;

    public override float[,] GenerateNoiseMap(Vector2 position, int mapWidth, int mapHeight, float scale, Vector2 offset, int seed)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];
        if (scale <= 0)
            scale = 0.001f;

        FastNoiseLite fastNoisefunction = new FastNoiseLite(seed);
        fastNoisefunction.SetNoiseType(noiseType);
        fastNoisefunction.SetFractalType(fractalType);
        fastNoisefunction.SetFractalGain(persistance);
        fastNoisefunction.SetFractalLacunarity(lacunarity);
        fastNoisefunction.SetFractalOctaves(octaves);
        fastNoisefunction.SetFrequency(frequency);

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float sampleX = (position.x + (x - mapWidth / 2)) / scale;
                float sampleY = (position.y + (y - mapHeight / 2)) / scale;

                noiseMap[x,y] = (fastNoisefunction.GetNoise(sampleX, sampleY) * noiseAmplitude) + noiseYOffset;
            }

        }
        return noiseMap;
    }
}
