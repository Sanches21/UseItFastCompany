using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInteract : MonoBehaviour
{

    public GameObject[] ObjectForInteract;
    public GameObject ItemsPrefabsArrayObject;

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
                ObjectForInteract[1].SetActive(true);
                ObjectForInteract[3].SetActive(true);
                ObjectForInteract[5].SetActive(true);
                break;
            case "LocInfo":
                ObjectForInteract[1].SetActive(false);
                ObjectForInteract[2].SetActive(true);
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
                ObjectForInteract[4].SetActive(true);
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

        Debug.Log(LootPrefabIndex);
        if (GameObject.Find("PlayerScreenCenter").transform.GetComponent<CameraMove>().CurentEnergy > 0)
        {
            ItemsPrefabsArrayObject.GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex].GetComponent<ArtursVersionInventoryCode>().GameMove = true;
            Instantiate(ItemsPrefabsArrayObject.GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex], firstInventoryTryAttache.transform, m);
            ItemsPrefabsArrayObject.GetComponent<PrefabArrayStore>().ItemsPrefabs[LootPrefabIndex].GetComponent<ArtursVersionInventoryCode>().GameMove = false;
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
