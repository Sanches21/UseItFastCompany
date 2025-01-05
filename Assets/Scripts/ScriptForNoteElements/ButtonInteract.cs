using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteract : MonoBehaviour
{

    public GameObject[] ObjectForInteract;
    
    private int lustLocY;
    private int lustLocX;

    private string names = "empty";
    private float numberOfLoot;

    public GameObject firstInventoryTryAttache;

    bool m = false;
    private void OnMouseDown()
    {
        //if (hit.collider != null)
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            names = hit.transform.name;

        }

        switch (names)
        {
            case "CloseNoteButton":
                ObjectForInteract[0].SetActive(false);
                transform.localScale -= new Vector3(0.01f, 0.01f, 0);
                break;
            case "BackPack":
                ObjectForInteract[2].SetActive(false);
                ObjectForInteract[6].SetActive(false);
                ObjectForInteract[7].SetActive(false);
                ObjectForInteract[1].SetActive(true);
                ObjectForInteract[3].SetActive(true);
                ObjectForInteract[5].SetActive(true);
                break;
            case "LocInfo":
                ObjectForInteract[1].SetActive(false);
                ObjectForInteract[6].SetActive(false);
                ObjectForInteract[2].SetActive(true);
                ObjectForInteract[4].SetActive(true);
                ObjectForInteract[5].SetActive(true);
                break;
            case "SearcheLoot":
                LootGeneration();
                break;
            case "ToogleBackPack":
                ObjectForInteract[1].SetActive(true);
                ObjectForInteract[3].SetActive(true);
                ObjectForInteract[4].SetActive(false);
                ObjectForInteract[5].SetActive(false);
                break;
            case "ToogleLocInfo":
                ObjectForInteract[3].SetActive(false);
                ObjectForInteract[5].SetActive(false);
                ObjectForInteract[4].SetActive(true);
                ObjectForInteract[6].SetActive(true);
                break;
            case "SearchStoreButton":
                ObjectForInteract[0].SetActive(true);
                ObjectForInteract[1].SetActive(false);

                if (!GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().wasSpawnFromList)//Нужно, что бы при многократном нажатии кнопки "осмотреть склад" не спаунились лишние объекты из списка
                {
                    if (lustLocY != GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().LocY || lustLocX != GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().LocX)
                    {
                        GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().SpawnItemsFromList();
                        GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().wasSpawnFromList = true;
                        lustLocY = GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().LocY;
                        lustLocX = GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay.transform.GetComponent<LocationInfoAndMouseEnter>().LocX;
                    }
                }
                break;
            default:
                break;
        }





    }

    private void OnMouseEnter()
    {
        transform.localScale += new Vector3(0.01f, 0.01f, 0);
    }

    private void OnMouseExit()
    {
        transform.localScale -= new Vector3(0.01f, 0.01f, 0);
    }

    private void LootGeneration()
    {
        int LootPrefabIndex = Random.Range(0, 10);

        //Debug.Log(LootPrefabIndex);
        if (GameObject.Find("PlayerScreenCenter").transform.GetComponent<CameraMove>().CurentEnergy > 0)
        {
            GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex].GetComponent<ArtursVersionInventoryCode>().ButtonSpownMove = true;
            Instantiate(GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex], firstInventoryTryAttache.transform, m);
            GameObject.Find("PrefabArrayStoreObject").GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex].GetComponent<ArtursVersionInventoryCode>().ButtonSpownMove = false;
        }
        else
        {
            GameObject.Find("PlayerScreenCenter").transform.GetComponent<CameraMove>().TextMessage.SetActive(true);
            GameObject.Find("PlayerScreenCenter").transform.GetComponent<CameraMove>().TextMessage.GetComponent<Text>().text = "You heaven't Energy";
            GameObject.Find("PlayerScreenCenter").transform.GetComponent<CameraMove>().TextTimer = true;
        }

        /*switch (d)
        {
            case 0:
                
                break;
            case 1:
                ItemsPrefabs[d].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 2:
                ItemsPrefabs[d].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 3:
                ItemsPrefabs[d].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 4:
                ItemsPrefabs[d].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 5:
                ItemsPrefabs[d].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            default:
                break;
        }*/
    }


}
