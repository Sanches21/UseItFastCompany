using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Looting : MonoBehaviour
{
    public GameObject AcceptOrTrashPanel;
    public GameObject YouFindImage;
    public GameObject YouFindeName;

    

    public GameObject[] InventCells; //список объектов €чеек
    public GameObject[] lootTable; //список возможных дл€ выпадени€ предметов
    float a;
    float b;
    float c;
    int d;
    
    private void Start()
    {
        
        
    }
    public void LootingComand()
    {
        AcceptOrTrashPanel.SetActive(true);
        float num = Random.Range(0.000001f, 1);
        
       

        a = num * 10000000;
        Debug.Log(a);
        b = a;
        c = a - (b - (a % 10f));
        d = (int)c;
        Debug.Log(d);
        

        if (d == 0)
        {
            
            Image Imag0 = lootTable[d].GetComponent<Image>(); 
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag0.sprite;

            Text tx = YouFindeName.GetComponent<Text>();
            tx.text = lootTable[d].name;
            //Debug.Log(lootTable[d].name);
            //Debug.Log(tx.text);
        }

        if (d == 1)
        {
            
            Image Imag1 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag1.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name;
        }

        if (d == 2)
        {
            
            Image Imag2 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag2.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text= lootTable[d].name.ToString();
        }

        if (d == 3)
        {
            
            Image Imag3 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag3.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 4)
        {
            
            Image Imag4 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag4.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 5)
        {
            
            Image Imag5 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag5.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 6)
        {
            
            Image Imag6 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag6.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 7)
        {
            
            Image Imag7 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag7.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 8)
        {
            
            Image Imag8 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag8.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 9)
        {
            
            Image Imag9 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag9.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }

        if (d == 10)
        {
            
            Image Imag10 = lootTable[d].GetComponent<Image>();
            Image Imag = YouFindImage.GetComponent<Image>();
            Imag.sprite = Imag10.sprite;

            Text Tex = YouFindeName.GetComponent<Text>();
            Tex.text = lootTable[d].name.ToString();
        }
    }

    
}
