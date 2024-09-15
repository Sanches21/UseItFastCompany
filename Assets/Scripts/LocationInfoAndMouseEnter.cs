using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationInfoAndMouseEnter : MonoBehaviour//���, ������� ������ �������� �� ��������� ����� ������� ��� ��������� ����, � � ���������� ��� � ����� ��������� � ���� ���������� � ��������������� �������
{
    public GameObject PointOnCenterMap;// ������ �� ������� ������ - �����, � ������� ������� ������ ������

    public GameObject sliderResurses;

    private int Resurse;
    public int MaxResurse;

    public GameObject sliderLocHP;

    private int LocHP;
    public int MaxLocHP;

    public GameObject textLocHP;
    public GameObject textResurses;
    public GameObject textLocHPRegen;
    public GameObject textResursesRegen;

    public int locHPRegen;
    public int ResursesRegen;

    SpriteRenderer sprite;

    void Start()
    {

        sliderResurses.GetComponent<Slider>().maxValue = MaxResurse;
        Resurse = MaxResurse;
        sliderResurses.GetComponent<Slider>().value = Resurse;

        sliderLocHP.GetComponent<Slider>().maxValue = MaxLocHP;
        LocHP = MaxLocHP;
        sliderLocHP.GetComponent<Slider>().value = LocHP;

        textLocHPRegen.GetComponent<Text>().text = "+ " + locHPRegen.ToString() + " in 5 minutes";
        textResursesRegen.GetComponent<Text>().text = "+ " + ResursesRegen.ToString() + " in 5 minutes";

        textLocHP.GetComponent<Text>().text = LocHP.ToString();
        textResurses.GetComponent<Text>().text = Resurse.ToString();

        sprite = GetComponent<SpriteRenderer>();
    }


    // Start is called before the first frame update
    private void OnMouseEnter() //����� ����� ��������, ���������� ���
    {
        if (Vector3.Distance(transform.position + new Vector3(0, 0.5f, 0), PointOnCenterMap.transform.position + new Vector3(0.7f, 0, 0)) <= 1.5f || Vector3.Distance(transform.position + new Vector3(0, 0.5f, 0), PointOnCenterMap.transform.position - new Vector3(0.7f, 0, 0)) <= 1.5f)
        {


            sprite.color = Color.green;

        }
        else
        {
            sprite.color = Color.red;
        }
    }
    private void OnMouseExit()//����� ���� �������� ������, ���������� ���
    {
        sprite.color = Color.white;
    }
}
