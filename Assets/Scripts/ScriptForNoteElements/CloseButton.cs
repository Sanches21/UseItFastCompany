using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour
{
    public GameObject ObjectForHide;

    private void OnMouseDown()
    {
        ObjectForHide.SetActive(false);
        transform.localScale -= new Vector3(0.01f, 0.01f, 0);
    }

    private void OnMouseEnter()
    {
        transform.localScale += new Vector3(0.01f, 0.01f, 0);
    }

    private void OnMouseExit()
    {
        transform.localScale -= new Vector3(0.01f, 0.01f, 0);
    }

    
}
