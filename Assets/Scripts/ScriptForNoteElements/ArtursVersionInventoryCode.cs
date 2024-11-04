using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArtursVersionInventoryCode : MonoBehaviour
{
    public GameObject[] PointsForCheck;
    private List<Transform> pointsForCheck = new List<Transform>();
    private GameObject CurrentLocPlayerStayThis;

    private bool TextTimer = false;
    private Vector3 BeginPosition;

    private string textForTextOffInTime;

    public Vector3 ShiftAttach;
    public GameObject TextMessage;
    public float TextDelay;
    private float a;
    [SerializeField] private Camera mainCamera;


    public bool PlayerMove = false;
    public bool GameMove;

    public Transform inventoryParent;
    public GameObject playerBackpack;
    public GameObject[] parentOfCells = new GameObject[2];// ������ � ��������� ���������, � ������� ����� �������� ������������ �����

    
    private RaycastHit hit;


    //�������� ������� � ������ ��� �����
    private void attachAtCell()
    {
        Vector3 attachablePointPosition = pointsForCheck[0].position;
        Physics.Raycast(attachablePointPosition, new Vector3(0, 0, 1f), out hit, 1f);
        Vector3 attachingCellPosition = hit.transform.position;
        transform.position = attachingCellPosition + ShiftAttach + new Vector3(0, 0, -0.19f);


        foreach (var rayCastPoint in pointsForCheck)
        {
            Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f);
            var cellComponent = hit.transform.GetComponent<CellFullOrNot>();
            cellComponent.IsFull = true;
        }
        
        transform.SetParent(hit.transform.parent.transform.parent);//��� ���� �� ���� ����������� ���� ������
        if (transform.parent.tag == "LocStore")
        {
            CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Add(transform.GetComponent<ItemInfo>().ItemIndex);//���� ������ ������� ������� ����/������
            Debug.Log(CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count);
            if (CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count == 5)
            {
                CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Clear();
            }
        }


    }


    //��������� ��� ����� �������� ������� � ������ ��� �����
    private bool checkCanAttachAtCell()
    {
        foreach (var rayCastPoint in pointsForCheck)
        {
            if (Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f))
            {
                var cellIsFullComponent = hit.transform.GetComponent<CellFullOrNot>(); //�������� ��� ������� ����� ������ ������
                if (cellIsFullComponent == null || cellIsFullComponent.IsFull)
                {
                    //Debug.Log("�� ���� " + rayCastPoint.gameObject.name + " �� ������� ������ ��� ��� ���������");
                    return false;
                }
            }
            else //������� �� ����� ������ ������
                return false;
        }
        return true;
    }


    private void Start()
    {
        CurrentLocPlayerStayThis = GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay;

        a = 1.5f;
        playerBackpack = GameObject.Find("BackpackLvl1");
        parentOfCells[1] = GameObject.Find("LocationStoreLvl1");
        inventoryParent = playerBackpack.transform.parent;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //TextMessage = GameObject.Find("TextLeftDownInfo");
        //TextMessage.SetActive(false);
        transform.SetParent(inventoryParent, true);

        foreach (Transform rayCastPoint in transform)
        {
            pointsForCheck.Add(rayCastPoint);
        }


        if (!GameMove)//������ �� ����� ������� �����������
        {
            attachAtCell();//��������� ������� ������ � ������������� ��������
        }

        if (GameMove)// ������ �� ����� ������ �������� �������
        {

            bool checkAttach = false;

            foreach (Transform cellTransform in playerBackpack.transform) //���������� �� ������������ ������� ����� �������
            {
                var cellComponent = cellTransform.gameObject.GetComponent<CellFullOrNot>(); //�������� ������� ������

                if (cellComponent.IsFull)
                    continue;

                //Debug.Log("�������� �������� ������� �� ������ " + cellTransform);
                transform.position = cellTransform.position + ShiftAttach + new Vector3(0, 0, -0.19f); //����������� ������� ����� ��������� ��������
                checkAttach = checkCanAttachAtCell();

                if (checkAttach)
                    break;
            }//���������� �� ������� ������� � ������� ������, � ���� ������� ���������� ������, ������������ ���� �����
            if (checkAttach)
            {
                attachAtCell();//��������� ������� ������ � ������������� ��������
                if (mainCamera.transform.parent.transform.GetComponent<CameraMove>().CurentEnergy != 0)
                {
                    GameObject.Find("Canvas/PlayerInfo/EnergySlider").transform.GetComponent<Slider>().value -= 1;

                    //textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();
                    mainCamera.transform.parent.transform.GetComponent<CameraMove>().CurentEnergy -= 1;
                    
                    //GameObject.Find("Canvas/LocInfo/") ����� �� �������� ��������� � ���������� �������, ��� �� ������� ������ ���� ��������� ���������� CurentResurse
                }//���� �������� ��� ��������� �������, �� ���� �� ������� � �������   !!!!!!!             
            }
            else
            {
                textForTextOffInTime = "BackPack Is full, Item Was Delited";
                Instantiate(TextMessage, GameObject.Find("Canvas").transform);
                TextTimer = true;

                //Debug.Log("����� ���, ������� �������" + name);
                //Destroy(gameObject); //���������������� ������ ��� �������� ������� � TextOffInTime. ������� ��������, �� �������� ��������� �������� ��������� � ���, ��� ������� ����� �����. ����� ������ ������������� ������� ����� �������� �� "������� ��������/��������"
            }
            return;
        }//���� ��� ���������� ����� ����� �����
    }


    private void FixedUpdate()
    {
        if (TextTimer)
        {
            TextOffInTime(textForTextOffInTime);
        }
    }

    private void TextOffInTime(string _text) //��������� ��� �������
    {
        GameObject.Find("Canvas/TextLeftDownInfo(Clone)").transform.GetComponent<Text>().text = _text;
        //TextMessage.GetComponent<Text>().text = _text;
        a = a - Time.deltaTime;
        if (a <= 0)
        {            
            if (textForTextOffInTime == "BackPack Is full, Item Was Delited")
            {
                TextTimer = false;
                a = TextDelay;               
                textForTextOffInTime = "";
                Destroy(this.gameObject);
            }
            TextTimer = false;
            a = TextDelay;            
            Destroy(GameObject.Find("Canvas/TextLeftDownInfo(Clone)"));
        }
    }

    private void ClearCellsOnUnattach()
    {
        foreach (var rayCastPoint in pointsForCheck)
        {
            Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f);
            hit.transform.GetComponent<CellFullOrNot>().IsFull = false;
        }
    }

    private void OnMouseDown()
    {
        BeginPosition = transform.position;
        ClearCellsOnUnattach();
    }

    private void OnMouseDrag()
    {
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0.1f;
        transform.position = mouseWorldPosition - new Vector3(0f, 0f, 4.91f);
        SelectCells();
    }

    private void SelectCells()
    {
        if (!checkCanAttachAtCell()) return;

        foreach (var rayCastPoint in pointsForCheck)
        {
            Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f);
            hit.transform.GetComponent<CellFullOrNot>().Select();
        }
    }

    //private void OnMouseOver()
    //{

    //}

    private void OnMouseUp()
    {
        if (!checkCanAttachAtCell())
        {
            transform.position = BeginPosition;
            textForTextOffInTime = "You can't to attache it there";
            Instantiate(TextMessage, GameObject.Find("Canvas").transform);
            TextTimer = true;
        }
        attachAtCell();
    }
}

