using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject TargetCellChoose;
    public void OnPointerEnter(PointerEventData eventData)
    {
        Inventory inv = TargetCellChoose.GetComponent<Inventory>();
        inv.TakeCell(this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Inventory inv = TargetCellChoose.GetComponent<Inventory>();
        inv.ClearCell();
    }

    

    

}
