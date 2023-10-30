using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterSimulator : MonoBehaviour
{
    private Vector2[,] waterVector = new Vector2[TerrainData.CHUNKLENGTH, TerrainData.CHUNKLENGTH];
    public TerrainData tD;
    // Start is called before the first frame update
    void Start()
    {
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {
            for (int y = 0; y < TerrainData.CHUNKLENGTH; y++)
            {
                waterVector[x, y] = Vector2.zero;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        MouseFollower.AddWater += AddWater;
    }

    private void OnDisable()
    {
        MouseFollower.AddWater -= AddWater;
    }
    void AddWater(Vector3 position, float radius)
    {
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {
            for (int y = 0; y < TerrainData.CHUNKLENGTH; y++)
            {
                //find length of vector, if it's less than the radius, enable
                if (Mathf.Sqrt(Mathf.Pow(x - position.x,2)+Mathf.Pow(y - position.y,2))< radius) {
                    //check if there is material in the "buffer"
                    if (tD.terrainPixelValues[x,y]==TerrainData.AIR){
                        Debug.Log("added water");
                        tD.terrainPixelValues[x,y]=TerrainData.WATER;
                        waterVector[x, y].y = -1f;
                    }
                }
            }
        }       
    }

    //handles waters interaction with anything, including moving sand & air
    public void SimulateWater(int x, int y){
        //TODO: handle edge cases more elegantly, at the moment sand can disappear off the edge
        //TODO: can sand have velocity? If everything is swapped right I don't think it can every have it anyway, something to auto test
        //check pixel is water
        if (tD.terrainPixelValues[x, y]==TerrainData.WATER){
            if (tD.terrainPixelValues[x, y-1]==TerrainData.AIR){
                //if pixel below is air, move down
                tD.terrainPixelValues[x, y-1]=TerrainData.WATER;
                tD.terrainPixelValues[x, y]=TerrainData.AIR;
                //swap velocities & gain velocity
                waterVector[x, y].y-=1f;
                waterVector[x, y-1] = waterVector[x, y];
                waterVector[x, y] = Vector2.zero;
            } else if (waterVector[x, y].y<-1f && tD.terrainPixelValues[x, y-1]==TerrainData.SAND){
                //if pixel below is sand & velocity is large enough, displace sand
                tD.terrainPixelValues[x, y-1]=TerrainData.WATER;
                tD.terrainPixelValues[x, y]=TerrainData.SAND;
                //swap velocities & lose velocity
                waterVector[x, y].y += 0.5f;
                waterVector[x, y-1] = waterVector[x, y];
                waterVector[x, y] = Vector2.zero;
            } else if (tD.terrainPixelValues[x-1, y]==TerrainData.AIR && tD.terrainPixelValues[x+1, y]==TerrainData.AIR){
                //if both left & right are options, pick one at random
                switch (Random.Range(0, 2)){
                    case 0:
                        GoLeft(x, y);
                        break;
                    case 1:
                        GoRight(x, y);
                        break;
                }
            } else if (tD.terrainPixelValues[x-1, y]==TerrainData.AIR || tD.terrainPixelValues[x+1, y]==TerrainData.AIR){
                GoLeft(x, y);
                GoRight(x, y);
            } else {
                //stationary, reset velocity
                waterVector[x, y].x=0;
            }
        }
        //don't allow a negative y velocity
        //waterVector[x, y].y = waterVector[x, y].y < 0 ? 0f : waterVector[x, y].y;
    }
    //TODO: if start moving one direction, stay that way till vertical drop or wall, which means vectors
    private void GoLeft(int x, int y){
        if (tD.terrainPixelValues[x-1, y]==TerrainData.AIR && waterVector[x, y].x <= 0){
            //if left pixel is air, move left & lose velocity
            tD.terrainPixelValues[x-1, y]=TerrainData.WATER;
            tD.terrainPixelValues[x, y]=TerrainData.AIR;
            waterVector[x, y].x -= 0.1f;
            waterVector[x-1, y] = waterVector[x, y];
            waterVector[x, y] = Vector2.zero;
        }
    }

    private void GoRight(int x, int y){
        if (tD.terrainPixelValues[x+1, y]==TerrainData.AIR  && waterVector[x, y].x >= 0){
            //if right pixel is air, move right & lose velocity
            tD.terrainPixelValues[x+1, y]=TerrainData.WATER;
            tD.terrainPixelValues[x, y]=TerrainData.AIR;
            waterVector[x, y].x += 0.1f;
            waterVector[x+1, y] = waterVector[x, y];
            waterVector[x, y] = Vector2.zero;
        }
    }
}
