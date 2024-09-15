using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TetrisInventoryCheckFullForItem : MonoBehaviour
{
    public GameObject[] PointsForCheck;

    private bool CanAttache = true;
    private bool TextTimer = false;
    private Vector3 FirstCellPosition;
    private Vector3 BeginPosition;
    

    public Vector3 ShiftAttach;
    public GameObject TextMessage;
    public float TextDelay;
    private float a;
    [SerializeField] private Camera mainCamera;

    private GameObject forSetParen;

    public bool PlayerMove = false;
    public bool GameMove;
    private bool wasAttached = false;

    public Transform[] arrayOfCells;
    public GameObject [] parentOfCells = new GameObject[2];
    int g = 0;

    private bool startClearCellsOnUnattache = false;

    private void Start()
    {
        
        parentOfCells[0] = GameObject.Find("BackpackLVL1");
        parentOfCells[1] = GameObject.Find("LocationStoreLvl1");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //TextMessage = GameObject.Find("TextLeftDownInfo");
        //TextMessage.SetActive(false);
        arrayOfCells = new Transform[parentOfCells[0].transform.childCount];//заполн€ем массив трансформами €чеек рюкзака
        

        if (PlayerMove)
        {

            Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);//вообще не помню дл€ чего надо


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
                            Debug.Log("Check breaked");
                            TextMessage.SetActive(true);
                            TextTimer = true;
                            TextMessage.GetComponent<Text>().text = "You can't to attache it there";
                            break;
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
                    hit.transform.GetComponent<CellFullOrNot>().IsFull = true;
                }


            }
        }// од который на текущий момент нигде не вызываетс€, был нужен дл€ момента, когда игрок перетаскивает предмет

        if (CanAttache)
        {
            //transform.position = FirstCellPosition + ShiftAttach;
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
                //CanAttache = false;
                PlayerMove = false;
            }
        }// пройтись по проверочным точкам и заполнить €чейки

        if (GameMove)
        {
            for (int j = 0; j < arrayOfCells.Length; j++)//в данном случае проходимс€ по €чейкам рюкзака
            {
                foreach (Transform t in parentOfCells[0].transform)
                {
                    arrayOfCells[g++] = t;
                }
                g = 0;
            }

            for (int j = 0; j < arrayOfCells.Length; j++)// в данном случае проходимс€ по €чейкам рюкзака
            {
                if (arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>() != null)
                    
                {
                    
                    if (!arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>().IsFull)//выполн€ем проверку на рамер айтема, как только нашли пустую €чейку
                    {
                        
                        transform.position = arrayOfCells[j].position + ShiftAttach + new Vector3 (0,0,-0.19f);
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
                                        wasAttached = false;
                                        break;
                                    }
                                    else
                                    {
                                        if (i == PointsForCheck.Length - 1)
                                        {
                                            for (int e = 0; e < PointsForCheck.Length; e++)
                                            {
                                                if (Physics.Raycast(PointsForCheck[e].transform.position, new Vector3(0, 0, 1f), out hit, 1f))
                                                {
                                                    hit.transform.GetComponent<CellFullOrNot>().IsFull = true;
                                                    Debug.Log(hit.transform.name);
                                                }
                                            }
                                            wasAttached = true;
                                            break;
                                        }
                                    }
                                }
                                
                            }
                        }
                        if (wasAttached)
                        {

                            transform.SetParent(parentOfCells[0].transform);
                            transform.position = FirstCellPosition + ShiftAttach + new Vector3(0, 0, -0.19f);
                            //wasAttached = false;
                            break;
                        }
                        else
                        {
                            if (j == arrayOfCells.Length - 1)
                            {
                                Debug.Log("BackPack is full");
                            }
                        }




                    }//выполн€ем проверку на рамер айтема, как только нашли пустую €чейку
                }

                //transform.SetParent(parentOfCells[0].transform.parent.transform);

                /*foreach (Transform t in transform)
                {
                    arrayOfCells[g++] = t;
                }
                g = 0;*/
            }//проходимс€ по €чейкам рюкзака в поисках пустых, и если находим подход€щий размер, присоедин€ем туда айтем


            if (!wasAttached)
            {

                arrayOfCells = new Transform[parentOfCells[1].transform.childCount];//заполн€ем массив трансформами €чеек склада
                for (int j = 0; j < arrayOfCells.Length; j++)//в данном случае проходимс€ по €чейкам склада
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
                for (int j = 0; j < arrayOfCells.Length; j++)// в данном случае проходимс€ по €чейкам рюкзака
                {
                    if (arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>() != null)
                    {
                        if (!arrayOfCells[j].gameObject.GetComponent<CellFullOrNot>().IsFull)//выполн€ем проверку на рамер айтема, как только нашли пустую €чейку
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
                                            wasAttached = false;
                                            break;
                                        }
                                        else
                                        {
                                            wasAttached = true;
                                        }
                                    }
                                }
                            }
                            if (wasAttached)
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
                wasAttached = false;
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
        }//≈сли код вызываетс€ через спаун клона

                a = TextDelay;
        GameMove = false;

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
        transform.position = mouseWorldPosition - new Vector3 (0f, 0f, 4.91f);
        startClearCellsOnUnattache = true;
        ClearCellsOnUnattache();
    }
    private void OnMouseOver()
    {

    }

    private void OnMouseUp()
    {
        CanAttache = false;
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
