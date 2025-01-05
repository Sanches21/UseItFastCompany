using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ArtursVersionInventoryCode : MonoBehaviour
{    
    public List<Transform> pointsForCheck = new List<Transform>();
    public GameObject CurrentLocPlayerStayThis;//Заполняется в апдейте, не помню почему

    private bool TextTimer = false;
    private Vector3 BeginPosition;// Поле для записи координат айтема, с которых мы начали тянуть айтем и на которые, в случае неудачного перетаскивания, будет возвращён айтем

    private string textForTextOffInTime;

    public Vector3 ShiftAttach;//Координаты для корректировки точки присоединения айтемов, сделана публичной, что бы можно было редактировать для разных размеров
    public GameObject TextMessage;
    public float TextDelay;
    private float a;
    [SerializeField] private Camera mainCamera;


    public bool PlayerMove = false;
    public bool ButtonSpownMove;
    public bool CameraMoveSpownMove;
    public bool wasDecreas = false;//Нужна для того, что бы лишь ОДИН раз ремувнуть объект в функции OnMouseDrag, а не на каждый кадр 

    public Transform inventoryParent;
    public GameObject playerBackpack;
    public GameObject[] parentOfCells = new GameObject[2];// массив с объектами родителей, к которым будем пытаться присоединить айтем

    
    private RaycastHit hit;


    //Вставить предмет в ячейку под собой
    private void attachAtCell()//изменить присваивание родителя на обращение через родителя ячейки, что бы норм работало и в рюкзаке и при аттаче к складу
    {       
        Physics.Raycast(pointsForCheck[0].position, new Vector3(0, 0, 1f), out hit, 1f);
        Vector3 attachingCellPosition = hit.transform.position;//Генерируем луч и записываем координаты в соответствующее поле для запоминания начальных координат айтема

        transform.position = attachingCellPosition + ShiftAttach + new Vector3(0, 0, -0.09f);//Странное число по оси z было настроено для качественного отображения в игре
        


        foreach (var rayCastPoint in pointsForCheck)//пройтись по массиву pointsForCheck, обращаясь к элементам по имени rayCastPoint
        {
            Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f);
            var cellComponent = hit.transform.GetComponent<CellFullOrNot>();
            cellComponent.IsFull = true;//установить ячейке состояние заполненная
        }
        
        transform.SetParent(hit.transform.parent.transform.GetChild(0));//Установить родителем нулевой объект во всех дочерних родителя (первый, перед всеми ячейками)


        if (transform.parent.parent.tag == "LocStore")//Если тэг родителя родителя LocStore
        {
/*
            CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Add(transform.GetComponent<ItemInfo>().ItemIndex);
            Debug.Log(CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count);
            transform.GetComponent<ItemInfo>().ListOfLocationStoreCurentIndex = CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count - 1;
*/        }
        

    }


    //Проверить что можно вставить предмет в ячейку под собой
    private bool checkCanAttachAtCell()
    {
        foreach (var rayCastPoint in pointsForCheck)//пройтись по массиву pointsForCheck и обращаться к теущему объекту по имени rayCastPoint
        {
            if (Physics.Raycast(rayCastPoint.position, new Vector3(0, 0, 1f), out hit, 1f))//Генерируем лучи из соответствующих точек
            {
                var cellIsFullComponent = hit.transform.GetComponent<CellFullOrNot>(); //Проверка что рейкаст нашел имнено ячейку, то есть исключительно её компонент
                if (cellIsFullComponent == null || cellIsFullComponent.IsFull)
                {
                    //Debug.Log("На луче " + rayCastPoint.gameObject.name + " не найдена ячейка или она заполнена");
                    return false;
                }
            }
            else //Рейкаст не нашел вообще ничего
                return false;
        }
        return true;
    }


    private void Start()
    {

        a = 1.5f;
        playerBackpack = GameObject.Find("PlayerScreenCenter/Main Camera/Note/PlayerHeroWhithBackpack/Inventory/Backpack/BackpackLVL1");
        parentOfCells[1] = GameObject.Find("LocationStoreLvl1");//Адресацию родителей ячеек необходимо оптимизировать, сделать универсальнее для двух видов инвентарей, рюкзака и склада
        inventoryParent = playerBackpack.transform;
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        //TextMessage = GameObject.Find("TextLeftDownInfo");
        //TextMessage.SetActive(false);
        
        //Следующее перечисление заполняет лист трансформов объектами точек для проверки лучами
        foreach (Transform rayCastPoint in transform)
        {
            pointsForCheck.Add(rayCastPoint);
        }// До сюда


        if (!ButtonSpownMove)//делаем во время ручного перемещения (игрок двигает)
        {
            if (!wasDecreas)
            {
                attachAtCell();//Заполняем занятые ячейки и устанавливаем родителя
            }
        }

        if (wasDecreas)
        {
            bool checkAttach = false;
            for (int p = 1; p <= parentOfCells[1].transform.parent.transform.childCount; p++)//Пройтись по ячейкам рюкзака, проигнорировав нулевую, ибо к ней привязываются айтемы
            {
                var cellComponent = parentOfCells[1].transform.parent.transform.GetChild(p).gameObject.GetComponent<CellFullOrNot>(); //Получаем текущую ячейку
                //Debug.Log(parentOfCells[1].transform.parent.transform.GetChild(p).name);
                if (cellComponent.IsFull)
                    continue;

                //Debug.Log("Пытаемся спавнить предмет на ячейке " + cellTransform);
                transform.position = parentOfCells[1].transform.parent.transform.GetChild(p).transform.position + ShiftAttach + new Vector3(0, 0, -0.19f); //Переместили предмет чтобы проверить рейкасты
                checkAttach = checkCanAttachAtCell();

                if (checkAttach)
                    break;
            }//проходимся по ячейкам склада локации в поисках пустых, и если находим подходящий размер, присоединяем туда айтем

            if (checkAttach)
            {
                attachAtCell();//Заполняем занятые ячейки и устанавливаем родителя
                            
            }
            else
            {
                textForTextOffInTime = "BackPack Is full, Item Was Delited";
                Instantiate(TextMessage, GameObject.Find("Canvas").transform);
                TextTimer = true;

                Debug.Log("Места нет, удаляем предмет" + name);
                Destroy(gameObject); //закомментировано потому что вставлен костыль в TextOffInTime. Предмет удалялся, не позволяя выполнить задержку сообщения о том, что предмет будет удалён. Далее лишнее существование объекта будет спрятано за "экраном перехода/ожидания"
            }
            return;
        }

        if (ButtonSpownMove)// Делаем во время спауна предмета условием игры (кнопкой лутинга локации)
        {
            bool checkAttach = false;
            //foreach (Transform cellTransform in playerBackpack.transform) //Проходимся по заполненному миссиву ячеек рюкзака
            for (int q = 1; q <= playerBackpack.transform.parent.transform.childCount; q++)//Пройтись по ячейкам рюкзака, проигнорировав нулевую, ибо к ней привязываются айтемы
            {
                var cellComponent = playerBackpack.transform.parent.transform.GetChild(q).gameObject.GetComponent<CellFullOrNot>(); //Получаем текущую ячейку
                Debug.Log(playerBackpack.transform.parent.transform.GetChild(q).name);
                if (cellComponent.IsFull)
                    continue;

                //Debug.Log("Пытаемся спавнить предмет на ячейке " + cellTransform);
                transform.position = playerBackpack.transform.parent.transform.GetChild(q).transform.position + ShiftAttach + new Vector3(0, 0, -0.19f); //Переместили предмет чтобы проверить рейкасты
                checkAttach = checkCanAttachAtCell();

                if (checkAttach)
                    break;
            }//проходимся по ячейкам рюкзака в поисках пустых, и если находим подходящий размер, присоединяем туда айтем
            if (checkAttach)
            {
                attachAtCell();//Заполняем занятые ячейки и устанавливаем родителя
                if (mainCamera.transform.parent.transform.GetComponent<CameraMove>().CurentEnergy != 0)
                {
                    GameObject.Find("Canvas/PlayerInfo/EnergySlider").transform.GetComponent<Slider>().value -= 1;

                    //textNumberEnergy.GetComponent<Text>().text = CurentEnergy.ToString();
                    mainCamera.transform.parent.transform.GetComponent<CameraMove>().CurentEnergy -= 1;
                    
                    //GameObject.Find("Canvas/LocInfo/") Здесь не дописано обращение к конкретной локации, что бы вычесть ресурс путём убавления переменной CurentResurse
                }//Сюда вставлен код вычитания энергии, но пока не ресурса с локации   !!!!!!!             
            }
            else
            {
                textForTextOffInTime = "BackPack Is full, Item Was Delited";
                Instantiate(TextMessage, GameObject.Find("Canvas").transform);
                TextTimer = true;

                Debug.Log("Места нет, удаляем предмет" + name);
                Destroy(gameObject); //закомментировано потому что вставлен костыль в TextOffInTime. Предмет удалялся, не позволяя выполнить задержку сообщения о том, что предмет будет удалён. Далее лишнее существование объекта будет спрятано за "экраном перехода/ожидания"
            }
            return;
        }//Если код вызывается через спаун клона
    }


    private void FixedUpdate()
    {
        if (CurrentLocPlayerStayThis == null)
        {
            CurrentLocPlayerStayThis = GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay;
        }

        if (TextTimer)
        {
            TextOffInTime(textForTextOffInTime);
        }
    }

    private void TextOffInTime(string _text) //подправил эту функцию
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
        /*if (transform.parent.parent.tag == "LocStore")//Если тэг родителя родителя LocStore
        {
            while (wasDecreas)//Необходимо, что бы удаление из листа происходило ОДИН раз за перетаскивание. Возвращается в true в функции OnMouseUp
            {
                CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.RemoveAt(transform.GetComponent<ItemInfo>().ListOfLocationStoreCurentIndex);
                if (transform.GetComponent<ItemInfo>().ListOfLocationStoreCurentIndex < (CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count - 1)) 
                {
                    for (int y = transform.GetComponent<ItemInfo>().ListOfLocationStoreCurentIndex; y == CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count - 1; y++)
                    {
                        .GetComponent<ItemInfo>().ListOfLocationStoreCurentIndex -= 1;
                    }
                }
                Debug.Log(CurrentLocPlayerStayThis.GetComponent<LocationInfoAndMouseEnter>().ItemStoreOnLocationIndex.Count);
                wasDecreas = false;
            }
        }*/
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

