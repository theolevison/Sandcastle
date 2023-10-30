using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.InputSystem;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePosition;
    }

    public static event Action<Vector3, float> EditTerrain;
    public static event Action<Vector3, float> AddWater;

    void OnLeftClick(){
        EditTerrain?.Invoke(transform.position, GetComponent<CircleCollider2D>().radius*this.transform.lossyScale.x);
    }

    void OnWKey(){
        AddWater?.Invoke(transform.position, GetComponent<CircleCollider2D>().radius*this.transform.lossyScale.x);
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        //collider has intersected spline, find location then pass location & radius of the circle to terrainObject
        Debug.Log("intersect");
        //EditTerrain?.Invoke(transform.position, GetComponent<CircleCollider2D>().radius);
    }
    
}
