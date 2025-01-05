using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraMove : MonoBehaviour
{
    public GameObject CurrentLocPlayerStay;//локация на которой игрок находится. Заполняется сперва в мап генератор, а потом в камера мув (здесь)

    public bool wasSpawnFromList = true;

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
    public int MinutesEnergy; //Переменная для записи максимального количества минут, отображаемых на таймере регенерации энергии
    public float SecondsEnergy; // Переменная для записи секунд, отображаемых на таймере регенерации энергии
    public GameObject textTimerRegenEnergy; //Ссылка на объект-текст, который отображает таймер регенерации энергии
    private int curentMinutes; //Промежуточная переменная, в которую записывается количество минут, оставшихся до восстановления энергии 
    private bool RegenGo = false; //Переменная, определяющая запускать регенерацию или нет в зависимости от потребности (когда запас энергии полный)

    public GameObject LocationSprite;

    public GameObject[] LocStoreLVLArray;//массив для хранения объектов/листов локации, к которым цепляются айтемы на складах локаций
    void Start()
    {
        TargetPosition = transform.position;//Целевой объект переместили к текущей позиции, что бы наша камера никуда ни при каких обстоятельствах не стремилась
        slider.GetComponent<Slider>().maxValue = MaxValueEnergy; //сообщаю слайдеру, что его максимальное значение должно быть равно максимальному (указанному в соответствующей переменной) значению
        CurentEnergy = MaxValueEnergy; //При старте игры текущая энергия игрока становится максимальной
        a = TextDelay;//Записываем в вспомогательную переменную максимальное время задержки текста
                      //textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();//Визуализируем в текстовом поле текущий запас энергии
                      //if (CurentEnergy < MaxValueEnergy)//Если текущая энергия при старте скрипта меньше максимальной (сравниваются два int)
                      //{
                      //RegenGo = true;
        curentMinutes = MinutesEnergy;//При старте приравниваем значение "текущие минуты" к максимальному, как бы начиная отсчёт (в дальнейшем "текущие менуты" должны будут синхронизироваться с "личным" времененм регенерации персонажа (пока такого нет)
        SecondsEnergy = 59;//При старте Устанавливаем значение "текущие секунды" на максимальное (в дальнейшем, как и с минутами)
                           //textTimerRegenEnergy.SetActive(true);//Делаем видимым текст таймера (время оставшееся до регенерации одной энергии)
                           //}
        /*else// Если равно или больше максимальной
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);//Делаем так, что бы таймер скрылся
        }*/
    }

    void FixedUpdate()
    {
        if (CurentEnergy < MaxValueEnergy)//Если текущая энергия при старте скрипта меньше максимальной (сравниваются два int)
        {
            RegenGo = true;

            textTimerRegenEnergy.SetActive(true);
        }
        else// Если равно или больше максимальной
        {
            RegenGo = false;
            textTimerRegenEnergy.SetActive(false);
        }

        if (RegenGo)//Если какое то условие изменило переменную "запуск регенерации"
        {
            RegenEnergy();//Вызов соответствующей функции
        }

        slider.GetComponent<Slider>().value = CurentEnergy;// Эта строчка важна, всё время сверяет, что бы значение слайдера совпадало с переменной
        textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();//Визуализируем в текстовом поле текущий запас энергии

        if (Input.GetMouseButton(0))//При нажатии левой кнопки мыши
        {
            if (!IsMoving)
            {//Если игрок не находится в процессе движения
                SetTargetPosition();//Запустить функцию изменения позиции целевой точки, к которой перемещается камера
            }
        }

        if (IsMoving)//Пока находимся в движении
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetPosition, 1 * Time.deltaTime);//Осуществляем движение объекта, к которому прикреплён этот скрипт

            if (Vector3.Distance(transform.position, TargetPosition) <= 0.05f)//Если расстояние между позицией этого объекта и целевой позицией меньше или равно указанного значения
            {
                IsMoving = false;//Установить прекращение движения
            }
        }

        if (TextTimer)//Когда активируется
        {
            TextOffInTime();//Вызов функции показа текста на некоторое время
        }
    }
    private void SetTargetPosition()//Функция установки целевой позиции, которая запускается в апдейте при нажатии левой кнопкой мыши
    {
        if (!GameObject.Find("PlayerScreenCenter/Main Camera/Note").activeInHierarchy)//Условие, отвечающее за проверку, что у игрока не открыт блокнот
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//Генерация луча от камеры в точку, где находится курсор

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))//Генерация луча с именем "ray" и попытка записать данные в поле с именем "hit"
            {

                TargetPosition = hit.transform.position + new Vector3(0, 0.5f, 0);//Переписываем целевую позицию на ту, куда попал луч с корректировкой
                if (Vector3.Distance(transform.position, TargetPosition + new Vector3(0.6f, 0, 0)) <= 1.5f || Vector3.Distance(transform.position, TargetPosition - new Vector3(0.6f, 0, 0)) <= 1.5f)//Сложный комплекс условий, который призван учесть разное удаление локаций вокруг той, от которой мы стартуем
                {
                    if (Vector3.Distance(transform.position, TargetPosition) >= 0.5f)//условие выполняемое, пока мы не достигли целевой позиции
                    {
                        IsMoving = true;//На всякий случай устанавливаем приказ двигаться

                        if (CurentEnergy > 0)//Код, выполняемый при условии, что энергия ещё есть
                        {
                            CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.white;//Получаем компонент локации, на которой стоим и меняем её цвет на белый, визуализируя, что мы с неё ушли

                            //Далее написан код отчистки инвентарей складов без удаления листа с предметами, привязанного к локации и код подгрузки предметов на склад при заходе на локацию

                            if (LocStoreLVLArray.Length != 0)//Если в массиве есть хотя бы один лист с инвентарём локации то запустить код на уничтожение и очистку инвентаря, а также сохранение предметов в список
                            {
                                

                                foreach (GameObject t2 in LocStoreLVLArray)//Пройтись по массиву листов инвентаря локации и обращаться к текущему рассматриваемому листу/объекту по имени "t2"
                                {


                                    foreach (Transform ItemsInLocStore in t2.transform.GetComponentInChildren<Transform>())//Проходимся по дочерним объектам конкретного листа инвентаря, обращаясь к каждому объекту по имени "ItemsInLocStore"
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
                            wasSpawnFromList = false;//Нужно, что бы при многократном нажатии кнопки "осмотреть склад" не спаунились лишние объекты из списка
                            CurrentLocPlayerStay = hit.transform.gameObject;//обновляем ссылку на объект-локацию, на которой стоим после того, как переместились

                            

                            CurrentLocPlayerStay.GetComponent<SpriteRenderer>().color = Color.blue;
                            CurentEnergy -= 1;
                            textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();

                            LocationSprite.GetComponent<SpriteRenderer>().sprite = hit.transform.GetComponent<SpriteRenderer>().sprite;//Замена картинки в инфо локации при переходе

                            //SpawnItemsFromList();

                        }//Конец кода, выполняемого при  условии, что энергия ещё есть

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
