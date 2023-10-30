using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PerlinNoise();
    }

    // Update is called once per frame
    void Update()
    {
        if (updateTerrainSetup){
            PerlinNoise();
        }
    }
    public TerrainData tD;
    private float[] surfaceHeights = new float[TerrainData.CHUNKLENGTH];
    public float amplitude = 3f;
    public float frequency = 1.75f;
    public int surfaceHeight = 6;
    public bool updateTerrainSetup = false;

    private void PerlinNoise(){
        //64*64 pixel chunk
        
        for (int i = 0; i < TerrainData.CHUNKLENGTH; i++)
        {
            //combine different amplitudes & frequencies of noise to make an array of surface heights
            surfaceHeights[i] = 
            Mathf.PerlinNoise(i/Mathf.Pow(frequency,1),0)*Mathf.Pow(amplitude,0)+
            Mathf.PerlinNoise(i/Mathf.Pow(frequency,2),0)*Mathf.Pow(amplitude,1)+
            Mathf.PerlinNoise(i/Mathf.Pow(frequency,3),0)*Mathf.Pow(amplitude,2)+
            Mathf.PerlinNoise(i/Mathf.Pow(frequency,4),0)*Mathf.Pow(amplitude,3)+
            surfaceHeight;
        }

        //start terrain at y = 0, then we can always move it up if we need, using chunks beneath, especially if terrain is infinite anyway.
        //set each pixel value according to perlin noise, except 1 line buffer
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {   
            for (int y = TerrainData.CHUNKLENGTH-1; y>(int)surfaceHeights[x]; y--){
                tD.terrainPixelValues[x, y] = TerrainData.AIR;
            }
            for (int y = (int)surfaceHeights[x]; y >= 0; y--)
            {
                tD.terrainPixelValues[x, y] = TerrainData.SAND;
            }
        }
    }
}
