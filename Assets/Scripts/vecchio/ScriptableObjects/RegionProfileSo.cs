using UnityEngine;

[CreateAssetMenu(menuName = "RegionProfile")]
public class RegionProfileSo : ScriptableObject
{
    public AnimationCurve terrainCurve;
    public terrainRegion[] regions;

    public Color[] GetColorMap(float[,] noiseMap)
    {
        Color[] colorMap = new Color[noiseMap.GetLength(0) * noiseMap.GetLength(1)];
        for (int y = 0; y < noiseMap.GetLength(0); y++)
        {
            for (int x = 0; x < noiseMap.GetLength(1); x++)
            {
                for (int i = 0; i < regions.Length; i++)
                {
                    if (noiseMap[x, y] <= regions[i].height)
                    {
                        colorMap[y * noiseMap.GetLength(0) + x] = regions[i].color;
                        break;
                    }

                }
            }
        }
        return colorMap;
    }
}
[System.Serializable]
public struct terrainRegion
{
    public string name;
    public float height;
    public Color color;
}
