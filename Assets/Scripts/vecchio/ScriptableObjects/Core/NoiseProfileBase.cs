using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class NoiseProfileBase : ScriptableObject
{
    public abstract float[,] GenerateNoiseMap(Vector2 position, int mapWidth, int mapHeight,float scale, Vector2 offset, int seed);
}
