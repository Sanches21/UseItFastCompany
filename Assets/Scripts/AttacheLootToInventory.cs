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

 /*‘ункци€ проверок €чеек и присоединение айтема к первой подход€щей €чейке
                               */
    public void AttacheLoot() //функци€ котора€ вызываетс€ при нажатии на зелЄную галочку (попытка прин€ть лут в инвентарь)
    {
        Looting LootingScript = ObjectWithCellArray.GetComponent<Looting>(); //получаем достум к компоненту/скрипту Looting
        ItemSize itemSize = AttachingItem.GetComponent<ItemSize>();
        for (int i = 0; i<(InventoryHorizontal*InventoryVertical); i++) //ѕрошлись по всем €чейкам
        {
            Debug.Log("Checking Cell Index " + i);
            CheckingObject = LootingScript.InventCells[i];
            IsCellFull isCellFull = CheckingObject.GetComponent<IsCellFull>();

            if (!isCellFull.CellFull) //ЌашЄл не заполненную €чейку
            {
                Debug.Log("Finde Empty Cell");
                if (InventoryHorizontal - (i % InventoryHorizontal) >= itemSize.Horizontal) //ѕровер€ю, что эта €чейка не за пределами инвентар€ Ўирина инвентар€ - количество €чеек справа >= горизонтального размера айтема
                {

                    if (InventoryVertical - (i/InventoryVertical) >= itemSize.Vertical ) //ѕровер€ю, помещаетс€ ли объект по высоте  
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




//TO DO TO DO TODO найти место, где надо выйти из алгоритма, в случае, если €чейка нам подходит 
                for (int itemVert = itemSize.Vertical; itemVert > 0; itemVert--) //Ќачинаем построчную проверку €чеек в квадрате размером с айтем
                {
                    
                    for (int itemHor = itemSize.Horizontal; itemHor > 0; itemHor--)
                    {
                        int n = i + (itemHor - 1) + (InventoryHorizontal * (itemVert - 1));
                        if (n < (InventoryHorizontal * InventoryVertical))
                        { 
                            CheckingObject = LootingScript.InventCells[n]; //«аписываю рассматриваемую €чейку в переменную
                            IsCellFull isCellFull1 = CheckingObject.GetComponent<IsCellFull>();
                            if (CheckingObject != null) //ѕровер€ю, что она существует
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
