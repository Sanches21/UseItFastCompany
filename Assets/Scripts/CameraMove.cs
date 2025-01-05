using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject CurrentLocPlayerStay;//������� �� ������� ����� ���������. ����������� ������ � ��� ���������, � ����� � ������ ��� (�����)

    public bool wasSpawnFromList = true;

    private Vector3 TargetPosition;//���������� � ������������ �������, � �������� �� ��������� ������� ������ � ����������� ������� (����� �� ������� ��������� ��������� �����)
    private bool IsMoving;//����������, ������� ����������, ���������� �������� ������ ��� ���
    public int CurentEnergy; //������� ���������� ������� � ������, ���������, ��� �� ����� ���� �� ������ �������� ������ ���
    public bool TextTimer = false;//����������, ����������� ������ �� �������� ���� ��������� ��� ���� ��������

    public GameObject slider; //������ �� ������, ���������� �������, ������������ ����� �������
    public GameObject textNumberEnergy; //������ �� ������-�����, ������� ���������� ������� ����� �������
    public GameObject TextMessage; //������ �� ������-����� � ���������� � ��� ��� ����� ���� �� �� ����� ������� � �� ����� ��������
    public float TextDelay; //����� � ��������, �� ������� ��������� ������������� �� ������
    private float a; //��������������� ����������, � ������� ������������ ������������� �������� �������, ������� �������� �� �������� ���������

    public int MaxValueEnergy; //�������� �������, ������� ����������� ����� ������������ � ������
    public int MinutesEnergy; //���������� ��� ������ ������������� ���������� �����, ������������ �� ������� ����������� �������
    public float SecondsEnergy; // ���������� ��� ������ ������, ������������ �� ������� ����������� �������
    public GameObject textTimerRegenEnergy; //������ �� ������-�����, ������� ���������� ������ ����������� �������
    private int curentMinutes; //������������� ����������, � ������� ������������ ���������� �����, ���������� �� �������������� ������� 
    private bool RegenGo = false; //����������, ������������ ��������� ����������� ��� ��� � ����������� �� ����������� (����� ����� ������� ������)

    public GameObject LocationSprite;

    public GameObject[] LocStoreLVLArray;//������ ��� �������� ��������/������ �������, � ������� ��������� ������ �� ������� �������
    void Start()
    {
        TargetPosition = transform.position;//������� ������ ����������� � ������� �������, ��� �� ���� ������ ������ �� ��� ����� ��������������� �� ����������
        slider.GetComponent<Slider>().maxValue = MaxValueEnergy; //������� ��������, ��� ��� ������������ �������� ������ ���� ����� ������������� (���������� � ��������������� ����������) ��������
        CurentEnergy = MaxValueEnergy; //��� ������ ���� ������� ������� ������ ���������� ������������
        a = TextDelay;//���������� � ��������������� ���������� ������������ ����� �������� ������
                      //textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();//������������� � ��������� ���� ������� ����� �������
                      //if (CurentEnergy < MaxValueEnergy)//���� ������� ������� ��� ������ ������� ������ ������������ (������������ ��� int)
                      //{
                      //RegenGo = true;
        curentMinutes = MinutesEnergy;//��� ������ ������������ �������� "������� ������" � �������������, ��� �� ������� ������ (� ���������� "������� ������" ������ ����� ������������������ � "������" ��������� ����������� ��������� (���� ������ ���)
        SecondsEnergy = 59;//��� ������ ������������� �������� "������� �������" �� ������������ (� ����������, ��� � � ��������)
                           //textTimerRegenEnergy.SetActive(true);//������ ������� ����� ������� (����� ���������� �� ����������� ����� �������)
                           //}
        /*else// ���� ����� ��� ������ ������������
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);//������ ���, ��� �� ������ �������
        }*/
    }

    void FixedUpdate()
    {
        if (CurentEnergy < MaxValueEnergy)//���� ������� ������� ��� ������ ������� ������ ������������ (������������ ��� int)
        {
            RegenGo = true;

            textTimerRegenEnergy.SetActive(true);
        }
        else// ���� ����� ��� ������ ������������
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);
        }

        if (RegenGo)//���� ����� �� ������� �������� ���������� "������ �����������"
        {
            RegenEnergy();//����� ��������������� �������
        }

        slider.GetComponent<Slider>().value = CurentEnergy;// ��� ������� �����, �� ����� �������, ��� �� �������� �������� ��������� � ����������
        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();//������������� � ��������� ���� ������� ����� �������

        if (Input.GetMouseButton(0))//��� ������� ����� ������ ����
        {
            if (!IsMoving)
            {//���� ����� �� ��������� � �������� ��������
                SetTargetPosition();//��������� ������� ��������� ������� ������� �����, � ������� ������������ ������
            }
        }

        if (IsMoving)//���� ��������� � ��������
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 1 * Time.deltaTime);//������������ �������� �������, � �������� ��������� ���� ������

            if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)//���� ���������� ����� �������� ����� ������� � ������� �������� ������ ��� ����� ���������� ��������
            {
                IsMoving = false;//���������� ����������� ��������
            }
        }

        if (TextTimer)//����� ������������
        {
            TextOffInTime();//����� ������� ������ ������ �� ��������� �����
        }
    }
    private void SetTargetPosition()//������� ��������� ������� �������, ������� ����������� � ������� ��� ������� ����� ������� ����
    {
        if (!GameObject.Find("PlayerScreenCenter/Main Camera/Note").activeInHierarchy)//�������, ���������� �� ��������, ��� � ������ �� ������ �������
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//��������� ���� �� ������ � �����, ��� ��������� ������

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))//��������� ���� � ������ "ray" � ������� �������� ������ � ���� � ������ "hit"
            {

                TargetPosition = hit.transform.position + new Vector3(0, 0.5f, 0);//������������ ������� ������� �� ��, ���� ����� ��� � ��������������
                if (Vector3.Distance(transform.position, TargetPosition + new Vector3(0.6f, 0, 0)) <= 1.5f || Vector3.Distance(transform.position, TargetPosition - new Vector3(0.6f, 0, 0)) <= 1.5f)//������� �������� �������, ������� ������� ������ ������ �������� ������� ������ ���, �� ������� �� ��������
                {
                    if (Vector3.Distance(transform.position, TargetPosition) >= 0.5f)//������� �����������, ���� �� �� �������� ������� �������
                    {
                        IsMoving = true;//�� ������ ������ ������������� ������ ���������

                        if (CurentEnergy > 0)//���, ����������� ��� �������, ��� ������� ��� ����
                        {
                            CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.white;//�������� ��������� �������, �� ������� ����� � ������ � ���� �� �����, ������������, ��� �� � �� ����

                            //����� ������� ��� �������� ���������� ������� ��� �������� ����� � ����������, ������������ � ������� � ��� ��������� ��������� �� ����� ��� ������ �� �������

                            if (LocStoreLVLArray.Length != 0)//���� � ������� ���� ���� �� ���� ���� � ��������� ������� �� ��������� ��� �� ����������� � ������� ���������, � ����� ���������� ��������� � ������
                            {
                                

                                foreach (GameObject t2 in LocStoreLVLArray)//�������� �� ������� ������ ��������� ������� � ���������� � �������� ���������������� �����/������� �� ����� "t2"
                                {


                                    foreach (Transform ItemsInLocStore in t2.transform.GetComponentInChildren<Transform>())//���������� �� �������� �������� ����������� ����� ���������, ��������� � ������� ������� �� ����� "ItemsInLocStore"
                                    {
                                        CurrentLocPlayerStay.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Add(ItemsInLocStore.GetComponent<ItemInfo>().ItemIndex);
                                        Destroy(ItemsInLocStore.gameObject);
                                    }

                                    foreach (Transform t3 in t2.transform.parent.GetComponentInChildren<Transform>())
                                    {
                                        if (t3.GetComponent<CellFullOrNot>() != null)
                                        {
                                            t3.GetComponent<CellFullOrNot>().IsFull = false;
                                        }
                                    }
                                }
                            }
                            wasSpawnFromList = false;//�����, ��� �� ��� ������������ ������� ������ "��������� �����" �� ���������� ������ ������� �� ������
                            CurrentLocPlayerStay = hit.transform.gameObject;//��������� ������ �� ������-�������, �� ������� ����� ����� ����, ��� �������������

                            

                            CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.blue;
                            CurentEnergy -= 1;
                            textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();

                            LocationSprite.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;//������ �������� � ���� ������� ��� ��������

                            //SpawnItemsFromList();

                        }//����� ����, ������������ ���  �������, ��� ������� ��� ����

                        else
                        {
                            IsMoving = false;
                            TextMessage.SetActive(true);
                            TextMessage.GetComponent<Text>().text = "You heaven't energy!";
                            TextTimer = true;
                        }
                    }
                }
                else
                {
                    TextMessage.SetActive(true);
                    TextMessage.GetComponent<Text>().text = "This location to far!";
                    TextTimer = true;
                    IsMoving = false;
                }

            }
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
    private void RegenEnergy()
    {
        SecondsEnergy -= Time.deltaTime;
        textTimerRegenEnergy.GetComponent<Text>().text = curentMinutes.ToString() + ":" + SecondsEnergy.ToString("F0");
        if (SecondsEnergy <= 0)
        {
            if (curentMinutes > 0)
            {
                curentMinutes = curentMinutes - 1;
                SecondsEnergy = 59;
            }
            else
            {
                if (CurentEnergy < MaxValueEnergy)
                {
                    CurentEnergy += 1;
                    textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();
                    curentMinutes = MinutesEnergy;
                    SecondsEnergy = 59;
                }
                else
                {
                    RegenGo = false;
                    textTimerRegenEnergy.SetActive(false);
                }
            }
        }
    }

    public void SpawnItemsFromList()
    {
        for (int w = 0; w < CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count; w++)
        {
            int Itemindex = CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex[w];

            GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[Itemindex].GetComponent<ArtursVersionInventoryCode>().wasDecreas = true;
            Instantiate(GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[Itemindex], LocStoreLVLArray[0].transform, false);
            GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[Itemindex].GetComponent<ArtursVersionInventoryCode>().wasDecreas = false;
        }
    }
}
