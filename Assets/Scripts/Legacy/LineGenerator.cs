using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

public class LineGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        spline = GetComponent<SpriteShapeController>().spline;
        PerlinNoise();
    }

    // Update is called once per frame
    void Update()
    {
        //PerlinNoise();
    }
    private const int ChunkLength = 64;
    private float[] chunk1 = new float[ChunkLength];
    private float[] chunk2 = new float[ChunkLength];
    private float[] chunk3 = new float[ChunkLength];
    private float[] chunk4 = new float[ChunkLength];
    
    public float amplitude = 0.5f; 
    public float frequency = 1.1f;
    private Spline spline;

    public event Action TerrainUpdate;

    void EditSpline(Vector3 position, float radius)
    { 
        //find point in spline corresponding to position, then change y values of each according to radius calculation
        Debug.Log(spline.GetHeight(0));
        spline.SetPosition(0, new Vector3(0, spline.GetPosition(0).y+1, 0.0f));
        //spline.SetHeight(0, spline.GetHeight(0)+1);
        //getPosition
        //setHeight
    }

    private void OnEnable()
    {
        MouseFollower.EditTerrain += EditSpline;
    }

    private void OnDisable()
    {
        MouseFollower.EditTerrain -= EditSpline;
    }

    void PerlinNoise(){
        //64*64 pixel chunk
        
        spline.Clear();

        for (int i = 0; i < ChunkLength; i++)
        {
            chunk1[i] = Mathf.PerlinNoise(i/Mathf.Pow(frequency,1),0)*Mathf.Pow(amplitude,0);
            chunk2[i] = Mathf.PerlinNoise(i/Mathf.Pow(frequency,2),0)*Mathf.Pow(amplitude,1);
            chunk3[i] = Mathf.PerlinNoise(i/Mathf.Pow(frequency,3),0)*Mathf.Pow(amplitude,2);
            chunk4[i] = Mathf.PerlinNoise(i/Mathf.Pow(frequency,4),0)*Mathf.Pow(amplitude,3);
        }

        for (int i = 0; i < ChunkLength; i++)
        {
            chunk1 = chunk1.Zip(chunk4.Zip(chunk3, (x, y) => x + y).Zip(chunk2, (x, y) => x + y), (x, y) => x + y).ToArray();

            spline.InsertPointAt(i,new Vector3(i, chunk1[i], 0.0f));
        }
        GetComponent<PolygonCollider2D>().CreateMesh(true,true);
        //GetComponent<PolygonCollider2D>().SetPath(0, chunk1.Zip);
        
        //raise event so that observers can do something with it
        TerrainUpdate?.Invoke();
    }
}
