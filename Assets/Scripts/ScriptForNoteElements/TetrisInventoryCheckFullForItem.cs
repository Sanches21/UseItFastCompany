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


    private void Start()
    {
        {
            {
                Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
                if (CanAttache)
                {

                    transform.position = FirstCellPosition + ShiftAttach;
                }
                CanAttache = true;

            }
        }
        a = TextDelay;
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
    }

    private void OnMouseDown()
    {
        BeginPosition = transform.position;
    }
    private void OnMouseDrag()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0.1f;
        transform.position = mouseWorldPosition - new Vector3 (0f, 0f, 4.71f);

        ClearCellsOnUnattache();
    }
    private void OnMouseOver()
    {

    }

    private void OnMouseUp()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
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
            }

        }
        if (CanAttache) 
        {
            
            transform.position = FirstCellPosition + ShiftAttach;
        }
        CanAttache = true;
        
    }

    
}
