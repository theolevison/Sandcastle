using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrainData : ScriptableObject
{
    public const int AIR = 0;
    public const int SAND = 1;
    public const int WATER = 2;
    public const int CHUNKLENGTH = 64;
    public const int BUFFER = 2;
    public int[,] terrainPixelValues = new int[CHUNKLENGTH, CHUNKLENGTH];
}
