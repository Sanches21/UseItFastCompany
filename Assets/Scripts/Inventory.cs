using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject CheckObject;

    public void TakeCell (GameObject a){
        CheckObject = a;
}
    public void ClearCell()
    {
        CheckObject = null;
    }



    }
