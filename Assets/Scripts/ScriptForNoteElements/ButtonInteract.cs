using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteract : MonoBehaviour
{

    public GameObject[] ObjectForInteract;
    public GameObject[] ItemsPrefabs;

    private string names = "empty";
    private float numberOfLoot;

    public GameObject firstInventoryTryAttache;

    float a;
    float b;
    float c;
    int d;
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
                break;
            case "LocInfo":
                ObjectForInteract[1].SetActive(false);
                ObjectForInteract[2].SetActive(true);
                break;
            case "SearcheLoot":
                LootGeneration();
                break;
            case "ToogleBackPack":
                ObjectForInteract[4].SetActive(false);
                ObjectForInteract[3].SetActive(true);
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
        numberOfLoot = Random.Range(0.000001f, 1);
        a = numberOfLoot * 10000000;
        //Debug.Log(a);
        b = a;
        c = a - (b - (a % 10f));
        d = (int)c;
        
        Debug.Log(d);

        switch (d)
        {
            case 0:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 1:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 2:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 3:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 4:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            case 5:
                ItemsPrefabs[d].GetComponent<TetrisInventoryCheckFullForItem>().GameMove = true;
                Instantiate(ItemsPrefabs[d], firstInventoryTryAttache.transform, m);
                break;
            default:
                break;
        }
    }


}
