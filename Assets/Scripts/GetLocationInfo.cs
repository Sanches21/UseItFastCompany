using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetLocationInfo : MonoBehaviour
{
    public Image locImage;
    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            ChangeInfo();
        }       
    }

    private void ChangeInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (hit.collider != null)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Image Im = locImage.GetComponent<Image>();
            SpriteRenderer Ima = hit.transform.GetComponent<SpriteRenderer>();
            Im.sprite = Ima.sprite;
        }
    }
}
