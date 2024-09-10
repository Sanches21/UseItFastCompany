using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    private Vector3 TargetPosition;//Переменная с координатами объекта, к которому мы стремимся двигать объект с привязанной камерой (читай на который стремится двигаться игрок)
    private bool IsMoving;//переменная, которая обозначает, происходит движение игрока или нет
    public int CurentEnergy; //Текущее количество энергии у игрока, публичное, что бы можно было из других скриптов менять его
    private bool TextTimer = false;//определяет, запускается таймер на закрытие окна сообщения или пока отключен

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

    void Start()
    {
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
