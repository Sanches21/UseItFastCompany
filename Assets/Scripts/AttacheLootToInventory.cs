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
    
    public bool CanAttach = true;
    public RectTransform RT;


    public void AttacheLoot() //надо добавить переменную при заполнении которой дальнейши проверки проводитьс€ не будут, что б после нахождени€ необходимого пространства CanAttach не помен€лс€
    {
        Looting LootingScript = ObjectWithCellArray.GetComponent<Looting>();
        ItemSize itemSize = AttachingItem.GetComponent<ItemSize>();
        for (int i = 0; i<(InventoryHorizontal*InventoryVertical); i++) //ѕрошлись по всем €чейкам
        {
            CheckingObject = LootingScript.InventCells[i];
            IsCellFull isCellFull = CheckingObject.GetComponent<IsCellFull>();

            if (!isCellFull.CellFull) //ЌашЄл не заполненную €чейку
            {
                Debug.Log("Finde Empty Cell");
                if (InventoryHorizontal - (i % InventoryHorizontal) >= itemSize.Horizontal) //ѕровер€ю, что эта €чейка не за пределами инвентар€ Ўирина инвентар€ - количество €чеек справа >= горизонтального размера айтема
                {

                    if ((InventoryVertical * InventoryHorizontal - 1) - i >= InventoryHorizontal * (itemSize.Vertical)) //ѕровер€ю, помещаетс€ ли объект по высоте  
                    {

                    }
                    else
                    {
                        Debug.Log("ToVerticalMuch");
                        CanAttach = false;
                    }
                }
                else
                {
                    Debug.Log("ToHorizontalMuch");
                    CanAttach = false;
                }


                for (int a = itemSize.Vertical; a > 0; a--) //Ќачинаем построчную проверку €чеек в квадрате размером с айтем
                {
                    Debug.Log("i = " + i);
                    for (int b = itemSize.Horizontal; b > 0; b--)
                    {
                        
                        int n = i + (b - 1) + (InventoryHorizontal * (a - 1));
                        
                        

                        if (n < (InventoryHorizontal * InventoryVertical))
                        { 
                            CheckingObject = LootingScript.InventCells[n]; //«аписываю рассматриваемую €чейку в переменную
                            IsCellFull isCellFull1 = CheckingObject.GetComponent<IsCellFull>();
                            if (CheckingObject != null) //ѕровер€ю, что она существует
                            {
                                if (!isCellFull1.CellFull)
                                {
                                    Debug.Log("a = " + a);
                                    Debug.Log("b = " + b);
                                    Debug.Log(n);
                                }
                                else
                                {
                                    CanAttach = false;
                                }
                            }
                            else
                            {
                                Debug.Log("Null a = " + a);
                                Debug.Log("Null b = " + b);
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
            if (CanAttach)
            {

                Debug.Log(i);
                Debug.Log("Object Created");
                break;
                
            }
            
        }
        CanAttach = true;
    }
}
