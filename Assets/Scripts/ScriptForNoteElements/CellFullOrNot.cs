using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFullOrNot : MonoBehaviour
{
    public GameObject ParentOfCell;
    public bool IsFull;
    public bool IsSelected = false;

    private float selectedTime = 0.01f;
    private float selectedTimer = 0f;

    private void Update()
    {
        if (IsSelected)
        {
            transform.GetComponent<SpriteRenderer>().color = Color.green;
            selectedTimer -= Time.deltaTime;
            if (selectedTimer <= 0f)
            {
                IsSelected = false;
            }
        }
        else
        {
            if (IsFull)
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 100/255f);
            }
            else
            {
                transform.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 100 / 255f);
            }
        }

    }

    public void Select()
    {
        selectedTimer = selectedTime;
        IsSelected = true;
    }
}
