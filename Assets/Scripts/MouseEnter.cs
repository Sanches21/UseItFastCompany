using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEnter : MonoBehaviour
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
        if (Vector3.Distance(transform.position, PointOnCenterMap.transform.position) <= 1.3f)
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
