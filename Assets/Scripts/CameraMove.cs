using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    private Vector3 TargetPosition;//���������� � ������������ �������, � �������� �� ��������� ������� ������ � ����������� ������� (����� �� ������� ��������� ��������� �����)
    private bool IsMoving;//����������, ������� ����������, ���������� �������� ������ ��� ���
    public int CurentEnergy; //������� ���������� ������� � ������, ���������, ��� �� ����� ���� �� ������ �������� ������ ���
    private bool TextTimer = false;//����������, ����������� ������ �� �������� ���� ��������� ��� ���� ��������

    public GameObject slider; //������ �� ������, ���������� �������, ������������ ����� �������
    public GameObject textNumberEnergy; //������ �� ������-�����, ������� ���������� ������� ����� �������
    public GameObject TextMessage; //������ �� ������-����� � ���������� � ��� ��� ����� ���� �� �� ����� ������� � �� ����� ��������
    public float TextDelay; //����� � ��������, �� ������� ��������� ������������� �� ������
    private float a; //��������������� ����������, � ������� ������������ ������������� �������� �������, ������� �������� �� �������� ���������

    public int MaxValueEnergy; //�������� �������, ������� ����������� ����� ������������ � ������
    public int MinutesEnergy; //���������� ��� ������ �����, ������������ �� ������� ����������� �������
    public float SecondsEnergy; // ���������� ��� ������ ������, ������������ �� ������� ����������� �������
    public GameObject textTimerRegenEnergy; //������ �� ������-�����, ������� ���������� ������ ����������� �������
    private int curentMinutes; //������������� ����������, � ������� ������������ ���������� �����, ���������� �� �������������� ������� 
    private bool RegenGo = false; //����������, ������������ ��������� ����������� ��� ��� � ����������� �� ����������� (����� ����� ������� ������)

    void Start()
    {
        slider.GetComponent<Slider>().maxValue = MaxValueEnergy; //������� ��������, ��� ��� ������������ �������� ������ ���� ����� ���������� � ��������������� ���������� ��������
        CurentEnergy = MaxValueEnergy; //��� ������ ���� ������� ������� ������ ���������� ������������
        a = TextDelay;
        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();
        if (CurentEnergy < MaxValueEnergy)
        {
            RegenGo = true;
            curentMinutes = MinutesEnergy;
            SecondsEnergy = 59;
            textTimerRegenEnergy.SetActive(true);
        }
        else
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CurentEnergy < MaxValueEnergy)
        {
            RegenGo = true;
            
            textTimerRegenEnergy.SetActive(true);
        }
        else
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);
        }

        if (RegenGo)
        {
            RegenEnergy();
        }

        

        slider.GetComponent<Slider>().value = CurentEnergy;
        if (Input.GetMouseButton(0))
        {

            if(!IsMoving){
                SetTargetPosition();
            }
        }

        if (IsMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 1 * Time.deltaTime);

            if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)
            {              

                IsMoving = false;

            }
        }

        if (TextTimer)
        {
            TextOffInTime();
        }



    }

    private void SetTargetPosition()
    {
        
        
        //RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)), Vector2.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //if (hit.collider != null)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {

            TargetPosition = hit.transform.position + new Vector3(0, 0.5f, 0);
            if (Vector3.Distance(transform.position, TargetPosition + new Vector3(0.6f, 0, 0)) <= 1.5f || Vector3.Distance(transform.position, TargetPosition - new Vector3(0.6f, 0, 0)) <= 1.5f)
            {

                

                if (Vector3.Distance(transform.position, TargetPosition) >= 0.5f)
                {
                    IsMoving = true;
                    if (CurentEnergy > 0)
                    {
                        CurentEnergy -= 1;
                        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();

                    }

                    else
                    {
                        TextMessage.SetActive(true);
                        TextMessage.GetComponent<Text>().text = "You heaven't Energy";
                        TextTimer = true;
                        IsMoving = false;
                    }
                }
            }
            else
            {

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
}
