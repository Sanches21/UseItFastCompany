using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttacheLootToInventory : MonoBehaviour
{
    public int InventoryHorizontal;
    public int InventoryVertical;
    public GameObject AttachingItem;
    
    public GameObject CheckingObject;
    public GameObject ObjectWithCellArray;
    
    
    public RectTransform RT;

    public bool CanAttach = true;

 /*������� �������� ����� � ������������� ������ � ������ ���������� ������
                               */
    public void AttacheLoot() //������� ������� ���������� ��� ������� �� ������ ������� (������� ������� ��� � ���������)
    {
        Looting LootingScript = ObjectWithCellArray.GetComponent<Looting>(); //�������� ������ � ����������/������� Looting
        ItemSize itemSize = AttachingItem.GetComponent<ItemSize>();
        for (int i = 0; i<(InventoryHorizontal*InventoryVertical); i++) //�������� �� ���� �������
        {
            Debug.Log("Checking Cell Index " + i);
            CheckingObject = LootingScript.InventCells[i];
            IsCellFull isCellFull = CheckingObject.GetComponent<IsCellFull>();

            if (!isCellFull.CellFull) //����� �� ����������� ������
            {
                Debug.Log("Finde Empty Cell");
                if (InventoryHorizontal - (i % InventoryHorizontal) >= itemSize.Horizontal) //��������, ��� ��� ������ �� �� ��������� ��������� ������ ��������� - ���������� ����� ������ >= ��������������� ������� ������
                {

                    if (InventoryVertical - (i/InventoryVertical) >= itemSize.Vertical ) //��������, ���������� �� ������ �� ������  
                    {


                    }
                    else
                    {
                        Debug.Log("ToVerticalMuch");
                        CanAttach = false;
                        continue;
                    }
                }
                
                else
                {
                    Debug.Log("ToHorizontalMuch"); 
                    CanAttach = false;
                    continue;
                }




//TO DO TO DO TODO ����� �����, ��� ���� ����� �� ���������, � ������, ���� ������ ��� �������� 
                for (int itemVert = itemSize.Vertical; itemVert > 0; itemVert--) //�������� ���������� �������� ����� � �������� �������� � �����
                {
                    
                    for (int itemHor = itemSize.Horizontal; itemHor > 0; itemHor--)
                    {
                        int n = i + (itemHor - 1) + (InventoryHorizontal * (itemVert - 1));
                        if (n < (InventoryHorizontal * InventoryVertical))
                        { 
                            CheckingObject = LootingScript.InventCells[n]; //��������� ��������������� ������ � ����������
                            IsCellFull isCellFull1 = CheckingObject.GetComponent<IsCellFull>();
                            if (CheckingObject != null) //��������, ��� ��� ����������
                            {
                                if (!isCellFull1.CellFull)
                                {
                                    Debug.Log("itemSize.Vertical = " + itemVert);
                                    Debug.Log("itemSize.Horizontal = " + itemHor);
                                    
                                }
                                else
                                {
                                    CanAttach = false;
                                    break;
                                }
                            }
                            else
                            {
                                Debug.Log("Null a = " + itemVert);
                                Debug.Log("Null b = " + itemHor);
                                Debug.Log(n);
                                CanAttach = false;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }  
            }
            else
            {
                CanAttach = false;
            }   
        }
        if (CanAttach)
        {
            
            Debug.Log("Object Created");
        }
        CanAttach = true;
    }
}
