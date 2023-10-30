using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSimulator : MonoBehaviour 
{
    public TerrainData tD;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        SimulateChunk();
    }

    private void SimulateChunk(){
        //buffer on edge of chunk for avoiding simulation edge conditions
        //TODO: combine these loops, so we aren't doing two unecessarily
        for (int x = TerrainData.BUFFER; x < TerrainData.CHUNKLENGTH-TerrainData.BUFFER; x++)
        {
            for (int y = TerrainData.BUFFER; y < TerrainData.CHUNKLENGTH-TerrainData.BUFFER; y++)
            {
                SimulateSand(x, y);
                GetComponent<WaterSimulator>().SimulateWater(x, y);
            }
        }
    }

    private void SimulateSand(int x, int y){
        //TODO: handle edge cases more elegantly, at the moment sand can disappear off the edge
        //check pixel is sand
        if (tD.terrainPixelValues[x, y]==TerrainData.SAND){
            if (tD.terrainPixelValues[x, y-1]==TerrainData.AIR){
                //if pixel below is air, move down
                tD.terrainPixelValues[x, y-1]=TerrainData.SAND;
                tD.terrainPixelValues[x, y]=TerrainData.AIR;
            } else if (tD.terrainPixelValues[x, y-1]==TerrainData.WATER){
                //if pixel below is water, move down
                tD.terrainPixelValues[x, y-1]=TerrainData.SAND;
                tD.terrainPixelValues[x, y]=TerrainData.WATER;
            } else if (tD.terrainPixelValues[x-1, y-2]==TerrainData.AIR && tD.terrainPixelValues[x-1, y-1]==TerrainData.AIR && tD.terrainPixelValues[x-1, y]==TerrainData.AIR){
                //if slope is steep enough, move left and down 2
                tD.terrainPixelValues[x-1, y-2]=TerrainData.SAND;
                tD.terrainPixelValues[x, y]=TerrainData.AIR;
            } else if (tD.terrainPixelValues[x+1, y-2]==TerrainData.AIR && tD.terrainPixelValues[x+1, y-1]==TerrainData.AIR && tD.terrainPixelValues[x+1, y]==TerrainData.AIR){
                //if slope is steep enough, move down and down 2
                tD.terrainPixelValues[x+1, y-2]=TerrainData.SAND;
                tD.terrainPixelValues[x, y]=TerrainData.AIR;
            }
        }
    }
}
