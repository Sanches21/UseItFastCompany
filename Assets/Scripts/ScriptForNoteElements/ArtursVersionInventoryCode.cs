using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArtursVersionInventoryCode : MonoBehaviour
{
    public GameObject[] PointsForCheck;

    private bool CanAttache = true;
    private bool TextTimer = false;
    private Vector3 FirstCellPosition;
    private GameObject FirstCell;
    private Vector3 BeginPosition;


    public Vector3 ShiftAttach;
    public GameObject TextMessage;
    public float TextDelay;
    private float a;
    [SerializeField] private Camera mainCamera;

    private GameObject forSetParen;

    public bool PlayerMove = false;
    public bool GameMove;
    private bool checkAttach = false;

    public Transform[] arrayOfCells;// Массив, в который записываем трансформы дочерних клеток рассматриваемого родителя, к которому пытаемся присоединить айтем
    public GameObject[] parentOfCells = new GameObject[2];// массив с объектами родителей, к которым будем пытаться присоединить айтем
    int g = 0;

    private bool startClearCellsOnUnattache = false;

    private void attachAtCell(Vector3 cellPosition)
    {
        transform.SetParent(parentOfCells[0].transform.parent.transform); 

        RaycastHit hit;
        for (int pointForCheckIndex = 0; pointForCheckIndex < PointsForCheck.Length; pointForCheckIndex++) // проходимся по точкам для рейкаста
        {
            if (Physics.Raycast(PointsForCheck[pointForCheckIndex].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
            {
                var cellComponent = hit.transform.GetComponent<CellFullOrNot>();
                if (cellComponent != null)
                {
                    cellComponent.IsFull = true;
                    //Debug.Log(hit.transform.name);
                }

            }
        }
    }//Заполняем занятые ячейки и устанавливаем родителя


    private void Start()
    {

        parentOfCells[0] = GameObject.Find("BackpackLVL1");
        parentOfCells[1] = GameObject.Find("LocationStoreLvl1");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //TextMessage = GameObject.Find("TextLeftDownInfo");
        //TextMessage.SetActive(false);
        arrayOfCells = new Transform[parentOfCells[0].transform.childCount];// Присваиваем массиву трансформов размер, соответствуюий количеству детей инвенторя


        CanAttache = true;
        //Заполнение ячеек при старте


        if (!GameMove)
        {
            attachAtCell(transform.position - ShiftAttach);//Заполняем занятые ячейки и устанавливаем родителя
        }

        if (GameMove)
        {
            for (int j = 0; j < arrayOfCells.Length; j++)//Заполняем массив с ячейками рюкзака
            {
                foreach (Transform t in parentOfCells[0].transform)
                {
                    arrayOfCells[g++] = t;
                }
                g = 0;
            }

            bool checkAttach1 = false;
            for (int cellArrayIndex = 0; cellArrayIndex < arrayOfCells.Length; cellArrayIndex++)//Проходимся по заполненному миссиву ячеек рюкзака
            {

                var cellComponent1 = arrayOfCells[cellArrayIndex].gameObject.GetComponent<CellFullOrNot>(); //Получаем текущую ячейку
                if (cellComponent1 != null && !cellComponent1.IsFull)//Здесь нашли пустую ячейку
                {
                    checkAttach1 = true;
                    Debug.Log("Пустая ячейка найдена");

                    transform.position = arrayOfCells[cellArrayIndex].position + ShiftAttach + new Vector3(0, 0, -0.19f); //Переместили предмет чтобы проверить рейкасты

                    RaycastHit hit;
                    for (int i = 0; i < PointsForCheck.Length; i++)
                    {

                        if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                        {
                            if (i == 0)
                            {
                                FirstCellPosition = hit.transform.position;
                            }

                            var cellComponent = hit.transform.GetComponent<CellFullOrNot>(); //Проверка что рейкаст нашел имнено ячейку
                            if (cellComponent == null || cellComponent.IsFull)
                            {
                                checkAttach1 = false;
                                break;
                            }
                        }
                    }
                }
                if (checkAttach1)
                    break;

            }//проходимся по ячейкам рюкзака в поисках пустых, и если находим подходящий размер, присоединяем туда айтем

            if (checkAttach1)
            {
                attachAtCell(FirstCellPosition);
            }
            else
            {
                Debug.Log("Места нет, удаляем предмет" + name);
                Destroy(gameObject);
            }
            return;


            if (!checkAttach)
            {
                arrayOfCells = new Transform[parentOfCells[1].transform.childCount];//заполняем массив трансформами ячеек склада
                for (int j = 0; j < arrayOfCells.Length; j++)//в данном случае проходимся по ячейкам склада
                {
                    //transform.SetParent(parentOfCells[1].transform.parent.transform);
                    foreach (Transform t in parentOfCells[1].transform)
                    {
                        arrayOfCells[g++] = t;
                    }
                    g = 0;
                }

                /*
                                for (int h = 0; h < arrayOfCells.Length; h++)
                                {
 
                                    for (int i = 0; i < PointsForCheck.Length; i++)
                                    {
 
                                        if (i == PointsForCheck.Length - 1)
                                        {
                                            CanAttache = true;
                                        }
 
                                        RaycastHit hit;
                                        if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                                        {
                                            if (i == 0)
                                            {
                                                FirstCellPosition = hit.transform.position;
 
                                            }
 
                                            if (hit.transform.GetComponent<CellFullOrNot>() != null)
                                            {
                                                if (hit.transform.GetComponent<CellFullOrNot>().IsFull)
                                                {
                                                    CanAttache = false;
                                                    transform.position = BeginPosition;
                                                    TextMessage.SetActive(true);
                                                    TextTimer = true;
                                                    TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                                                    break;
                                                }
                                            }
 
 
 
                                            else
                                            {
                                                CanAttache = false;
                                                transform.position = BeginPosition;
                                                TextMessage.SetActive(true);
                                                TextTimer = true;
                                                TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                                                break;
                                            }
 
                                            hit.transform.GetComponent<CellFullOrNot>().IsFull = true;
                                            transform.position = arrayOfCells[h].position + ShiftAttach;
                                            Debug.Log("Was transpotrt on H");
                                            wasAttached = true;
 
                                        }
                                    }
 
                                    if (wasAttached)
                                    {
                                        Debug.Log("Item attach to inventory");
                                        transform.SetParent(parentOfCells[1].transform);
                                        break;
                                    }
 
 
                                }*/
                for (int j = 0; j < arrayOfCells.Length; j++)// в данном случае проходимся по ячейкам рюкзака
                {
                    if (arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>() != null)
                    {
                        if (!arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>().IsFull)//выполняем проверку на рамер айтема, как только нашли пустую ячейку
                        {
                            for (int i = 0; i < PointsForCheck.Length; i++)
                            {

                                RaycastHit hit;
                                if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                                {

                                    if (i == 0)
                                    {
                                        FirstCellPosition = hit.transform.position;

                                    }

                                    if (hit.transform.GetComponent<CellFullOrNot>() != null)
                                    {
                                        if (hit.transform.GetComponent<CellFullOrNot>().IsFull)
                                        {
                                            checkAttach = false;
                                            break;
                                        }
                                        else
                                        {
                                            checkAttach = true;
                                        }
                                    }
                                }
                            }
                            if (checkAttach)
                            {
                                transform.SetParent(parentOfCells[0].transform);
                                transform.position = FirstCellPosition + ShiftAttach;
                                //wasAttached = false;
                                break;
                            }
                            else
                            {
                                if (j == arrayOfCells.Length - 1)
                                {
                                    Debug.Log("LocStore is full too");
                                }
                            }




                        }
                    }
                }
                checkAttach = false;
            }
            /* for (int h = 0; h < arrayOfCells.Length; h++)
             {
 
                 for (int i = 0; i < PointsForCheck.Length; i++)
                 {
 
                     if (i == PointsForCheck.Length - 1)
                     {
                         CanAttache = true;
                     }
 
                     RaycastHit hit;
                     if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                     {
                         if (i == 0)
                         {
                             FirstCellPosition = hit.transform.position;
 
                         }
 
                         if (hit.transform.GetComponent<CellFullOrNot>() != null)
                         {
                             if (hit.transform.GetComponent<CellFullOrNot>().IsFull)
                             {
                                 CanAttache = false;
                                 transform.position = BeginPosition;
                                 TextMessage.SetActive(true);
                                 TextTimer = true;
                                 TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                                 wasAttached = false;
                                 break;
                             }
                         }
 
 
 
                         else
                         {
                             CanAttache = false;
                             transform.position = BeginPosition;
                             TextMessage.SetActive(true);
                             TextTimer = true;
                             TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                             wasAttached = false;
                             break;
                         }
                         hit.transform.GetComponent<CellFullOrNot>().IsFull = true;
                         transform.position = arrayOfCells[h].position + ShiftAttach;
                         wasAttached = true;
 
                     }
                 }
 
                 if (wasAttached)
                 {
                     Debug.Log("Item attach to inventory");
                     transform.SetParent(parentOfCells[0].transform);
                     break;
                 }
 
             }*/





            /*if (CanAttache)
            {
 
                transform.position = FirstCellPosition + ShiftAttach;
            }*/
            CanAttache = true;
            GameMove = false;
        }//Если код вызывается через спаун клона

        //a = TextDelay;
    }
    private void FixedUpdate()
    {
        if (TextTimer)
        {
            TextOffInTime();
        }
    }

    private void TextOffInTime()
    {

        a = a - Time.deltaTime;
        if (a <= 0)
        {
            TextMessage.SetActive(false);
            TextTimer = false;
            a = TextDelay;
        }
    }

    private void ClearCellsOnUnattache()
    {
        if (startClearCellsOnUnattache)
        {
            for (int i = 0; i < PointsForCheck.Length; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                {
                    if (hit.transform.GetComponent<CellFullOrNot>() != null)
                    {
                        hit.transform.GetComponent<CellFullOrNot>().IsFull = false;
                    }
                }
            }
            startClearCellsOnUnattache = false;
        }
    }

    private void OnMouseDown()
    {
        BeginPosition = transform.position;
    }
    private void OnMouseDrag()
    {


        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0.1f;
        transform.position = mouseWorldPosition - new Vector3(0f, 0f, 4.91f);
        startClearCellsOnUnattache = true;
        ClearCellsOnUnattache();
    }
    private void OnMouseOver()
    {

    }

    private void OnMouseUp()
    {
        CanAttache = false;
        for (int i = 0; i < PointsForCheck.Length; i++)
        {

            RaycastHit hit;
            if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
            {
                Debug.Log("is hit spown");

                if (hit.transform.GetComponent<CellFullOrNot>() != null)
                {
                    if (hit.transform.GetComponent<CellFullOrNot>().IsFull)
                    {
                        Debug.Log("is hit");
                        CanAttache = false;
                        transform.position = BeginPosition;
                        Debug.Log("Check breaked");
                        TextMessage.SetActive(true);
                        TextTimer = true;
                        TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                        break;
                    }
                    else
                    {
                        if (i == PointsForCheck.Length - 1)
                        {

                            forSetParen = hit.transform.parent.gameObject.transform.parent.gameObject;

                            //Debug.Log(forSetParen.name);
                            CanAttache = true;
                        }

                        if (i == 0)
                        {
                            FirstCellPosition = hit.transform.position;
                        }

                    }
                }



            }
            else
            {
                TextMessage.SetActive(true);
                TextTimer = true;
                TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                CanAttache = false;
                transform.position = BeginPosition;
                break;
            }


        }
        if (CanAttache)
        {
            for (int i = 0; i < PointsForCheck.Length; i++)
            {
                RaycastHit hit;
                if (Physics.Raycast(PointsForCheck[i].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                {
                    if (hit.transform.GetComponent<CellFullOrNot>() != null)
                    {
                        hit.transform.GetComponent<CellFullOrNot>().IsFull = true;
                    }
                }
            }
            transform.SetParent(forSetParen.transform);
            transform.position = FirstCellPosition + ShiftAttach + new Vector3(0, 0, -0.19f);
        }
        CanAttache = false;

    }


}
