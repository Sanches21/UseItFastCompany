using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInfoAndMouseEnter : MonoBehaviour
{
    public GameObject PointOnCenterMap;

    SpriteRenderer sprite;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    private void OnMouseEnter()
    {
        if (Vector3.Distance(transform.position + new Vector3(0, 0.5f, 0), PointOnCenterMap.transform.position + new Vector3(0.7f, 0, 0)) <= 1.5f || Vector3.Distance(transform.position + new Vector3(0, 0.5f, 0), PointOnCenterMap.transform.position - new Vector3(0.7f, 0, 0)) <= 1.5f)
        {


            sprite.color = Color.green;
        }
        else
        {
            sprite.color = Color.red;
        }
    }
    private void OnMouseExit()
    {
        sprite.color = Color.white;
    }
}
