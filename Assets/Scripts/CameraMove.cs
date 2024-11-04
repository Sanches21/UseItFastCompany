using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject CurrentLocPlayerStay;//локация на которой игрок находится. Заполняется сперва в мап генератор, а потом в камера мув (здесь)
    private Vector3 TargetPosition;//Переменная с координатами объекта, к которому мы стремимся двигать объект с привязанной камерой (читай на который стремится двигаться игрок)
    private bool IsMoving;//переменная, которая обозначает, происходит движение игрока или нет
    public int CurentEnergy; //Текущее количество энергии у игрока, публичное, что бы можно было из других скриптов менять его
    public bool TextTimer = false;//определяет, запускается таймер на закрытие окна сообщения или пока отключен

    public GameObject slider; //Ссылка на объект, содержащий слайдер, отображающий запас энергии
    public GameObject textNumberEnergy; //Ссылка на объект-текст, который отображает текущий запас энергии
    public GameObject TextMessage; //Ссылка на объект-текст с сообщением о том что игрок чего то не может сделать и по каким причинам
    public float TextDelay; //Время в секундах, на которое сообщение задерживается на экране
    private float a; //Вспомогательная переменная, в которою записывается промежуточное значение времени, которое осталось до закрытия сообщения

    public int MaxValueEnergy; //Значение энергии, которое максимально может отрегениться у игрока
    public int MinutesEnergy; //Переменная для записи минут, отображаемых на таймере регенерации энергии
    public float SecondsEnergy; // Переменная для записи секунд, отображаемых на таймере регенерации энергии
    public GameObject textTimerRegenEnergy; //Ссылка на объект-текст, который отображает таймер регенерации энергии
    private int curentMinutes; //Промежуточная переменная, в которую записывается количество минут, оставшихся до восстановления энергии 
    private bool RegenGo = false; //Переменная, определяющая запускать регенерацию или нет в зависимости от потребности (когда запас энергии полный)

    public GameObject LocationSprite;

    void Start()
    {
        TargetPosition = transform.position;
        slider.GetComponent<Slider>().maxValue = MaxValueEnergy; //сообщаю слайдеру, что его максимальное значение должно быть равно указанному в соответствующей переменной значению
        CurentEnergy = MaxValueEnergy; //При старте игры текущая энергия игрока становится максимальной
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

        

        slider.GetComponent<Slider>().value = CurentEnergy;// Эта строчка важна, всё время сверяет, что бы значение слайдера совпадало с переменной
        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();

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
        
        
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        
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
                        CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.white;
                        CurrentLocPlayerStay = hit.transform.gameObject;
                        CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.blue;
                        CurentEnergy -= 1;
                        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();
                        LocationSprite.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        CurrentLocPlayerStay = hit.transform.gameObject;
                    }

                    else
                    {

                    }
                }
            }
            else
            {
                TextMessage.SetActive(true);
                TextMessage.GetComponent<Text>().text = "This location to far";
                TextTimer = true;
                IsMoving = false;
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
