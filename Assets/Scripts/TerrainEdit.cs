using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainEdit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        MouseFollower.EditTerrain += UserEdit;
    }

    private void OnDisable()
    {
        MouseFollower.EditTerrain -= UserEdit;
    }
    public TerrainData tD;
    private int materialInSuspension = 0;
    void UserEdit(Vector3 position, float radius)
    {
        Debug.Log("click");
        //from position, find all coords within circle, then change values
        //get radius from components
        //TODO: iterate from mouse position rather than the whole chunk to same a bit of performance, could implement quadtree to avoid comparing vector length everytime
        int tempMaterialInSuspension = 0;
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {
            for (int y = 0; y < TerrainData.CHUNKLENGTH; y++)
            {
                //find length of vector, if it's less than the radius, enable
                if (Mathf.Sqrt(Mathf.Pow(x - position.x,2)+Mathf.Pow(y - position.y,2))< radius) {
                    //check if there is material in the "buffer"
                    if (materialInSuspension > 0 && tD.terrainPixelValues[x,y]==TerrainData.AIR){
                        Debug.Log("added material");
                        tD.terrainPixelValues[x,y]=TerrainData.SAND;
                        materialInSuspension--;
                    } else if (tD.terrainPixelValues[x,y]==TerrainData.SAND){
                        Debug.Log("removed material");
                        tD.terrainPixelValues[x,y]=TerrainData.AIR;
                        tempMaterialInSuspension++;
                    }
                }
            }
        }
        materialInSuspension+=tempMaterialInSuspension;
    }   
}
