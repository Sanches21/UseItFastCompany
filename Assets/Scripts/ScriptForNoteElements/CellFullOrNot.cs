using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFullOrNot : MonoBehaviour
{
    public GameObject ParentOfCell;
    public bool IsFull;

    private void Update()
    {
        if (IsFull)
        {
            transform.GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
