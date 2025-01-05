using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationInfoAndMouseEnter : MonoBehaviour//Код, который сейчас отвечает за изменение цвета локации при наведении мыши, а в дальнейшем ещё и будет содержать в себе информацию о характеристиках локации
{
    [SerializeField] public List <int> ItemStoreOnLocationIndex; //массив индексов объектов, предназначенный хранить айтемы, выложенные на склад

    public int LocY;
    public int LocX;

    public GameObject PointOnCenterMap;// Ссылка на главный объект - точку, в которую смотрит камера игрока

    public GameObject sliderResurses;

    private int CurentResurse;
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
        PointOnCenterMap = GameObject.Find("PlayerScreenCenter");
        sliderResurses = GameObject.Find("Canvas/LocInfo/SliderResurse");
        sliderLocHP = GameObject.Find("Canvas/LocInfo/SliderLocHP");
        textLocHP = GameObject.Find("Canvas/LocInfo/SliderLocHP/Text Loc HP");
        textResurses = GameObject.Find("Canvas/LocInfo/SliderResurse/Text Resurse");
        textLocHPRegen = GameObject.Find("Canvas/LocInfo/SliderLocHP/Text LocHP Regen");
        textResursesRegen = GameObject.Find("Canvas/LocInfo/SliderResurse/Text Resurse Regen");


        sliderResurses.GetComponent<Slider>().maxValue = MaxResurse;
        CurentResurse = MaxResurse;
        

        sliderLocHP.GetComponent<Slider>().maxValue = MaxLocHP;
        LocHP = MaxLocHP;
        sliderLocHP.GetComponent<Slider>().value = LocHP;

        textLocHPRegen.GetComponent<Text>().text = "+ " + locHPRegen.ToString() + " in 5 minutes";
        textResursesRegen.GetComponent<Text>().text = "+ " + ResursesRegen.ToString() + " in 5 minutes";

        textLocHP.GetComponent<Text>().text = LocHP.ToString();
        textResurses.GetComponent<Text>().text = CurentResurse.ToString();

        sprite = GetComponent<SpriteRenderer>();
        //sprite.color = new Color(50/255f, 100/255f, 150/255f);
    }

    void FixedUpdate()
    {
        sliderResurses.GetComponent<Slider>().value = CurentResurse;
        textResurses.GetComponent<Text>().text = CurentResurse.ToString();
    }

        // Start is called before the first frame update
        private void OnMouseEnter() //когда мышка наведена, происходит код
    {
        if (sprite.color != Color.blue)
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
    }
    private void OnMouseExit()//Когда мышь покидает объект, происходит код
    {
        if (sprite.color != Color.blue)
        {
            sprite.color = Color.white;
        }
    }
}
