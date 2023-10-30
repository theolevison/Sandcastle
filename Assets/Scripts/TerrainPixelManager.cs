using System.Linq.Expressions;
using UnityEngine;

public class TerrainPixelManager : MonoBehaviour
{
    public TerrainData terrainData;
    public GameObject pixelPrefab;
    public Sprite sandSprite;
    public Sprite waterSprite;
    private GameObject[,] terrainPixelObjects = new GameObject[TerrainData.CHUNKLENGTH, TerrainData.CHUNKLENGTH];
    // Start is called before the first frame update
    void Start()
    {
        PixelObjectsSetup();
    }

    // Update is called once per frame
    void Update()
    {
        PixelUpdate();
    }

    private void PixelObjectsSetup(){
        //instantiate 64*64 pixels
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {
            for (int y = 0; y < TerrainData.CHUNKLENGTH; y++)
            {
                terrainPixelObjects[x,y] = Instantiate(pixelPrefab, new Vector3(x, y, 0), Quaternion.identity);
            }
        }
    }

    public void PixelUpdate(){
        //enable pixel objects based on their corresponding value
        for (int x = 0; x < TerrainData.CHUNKLENGTH; x++)
        {
            for (int y = 0; y < TerrainData.CHUNKLENGTH; y++)
            {
                switch (terrainData.terrainPixelValues[x,y])
                {
                    case TerrainData.SAND:
                        terrainPixelObjects[x,y].GetComponent<SpriteRenderer>().sprite = sandSprite;
                        terrainPixelObjects[x,y].SetActive(true);
                        break;
                    case TerrainData.WATER:
                        terrainPixelObjects[x,y].GetComponent<SpriteRenderer>().sprite = waterSprite;
                        terrainPixelObjects[x,y].SetActive(true);
                        break;
                    case TerrainData.AIR:
                        terrainPixelObjects[x,y].SetActive(false);
                        break;
                }

                //terrainPixelObjects[x,y].GetComponent<SpriteRenderer>().sprite = waterSprite;
            }
        }
    }
}
