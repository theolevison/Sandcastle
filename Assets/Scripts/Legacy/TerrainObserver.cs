using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TerrainObserver : MonoBehaviour
{
    [SerializeField] private LineGenerator subjectToObserve;

    private void OnTerrainChange()
    {
        // any logic that responds to event goes here
        Debug.Log("Observer responds");
        //make changes to sprite shape
        //GetComponent<SpriteShapeController>().GetComponent<Spline>().InsertPointAt();
    }

    private void Awake()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.TerrainUpdate += OnTerrainChange;
        }
    }

    private void OnDestroy()
    {
        if (subjectToObserve != null)
        {
            subjectToObserve.TerrainUpdate -= OnTerrainChange;
        }
    }
}
