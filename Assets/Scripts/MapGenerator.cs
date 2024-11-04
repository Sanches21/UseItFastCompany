using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] prefabsTilse;
    public GameObject parentOfLocs;
    public GameObject MapImage;
    private int PixelRandIndex;
    private Color LocColor;

    private float xCoord;
    private float yCoord;
    private int OrderLayers;

    // Start is called before the first frame update
    void Start()
    {
        Sprite MapTexture = MapImage.GetComponent<Image>().sprite;

        float deltaX = 0.0f;
        float deltaY = 0.0f;

        OrderLayers = 10500;

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //Debug.Log( MapImage.GetComponent<Image>().sprite.texture.format);
                PixelRandIndex = Random.Range(0, 5);

                if (PixelRandIndex < 6)
                {
                    MapTexture.texture.SetPixel(i, j, new Color(60 / 255f, 115 / 255f, 42 / 255f));//—ейчас будет пустошь, но когда по€витс€ тайл больницы надо будет заменить
                    Instantiate(prefabsTilse[PixelRandIndex], new Vector3(deltaX - (j * 1f), deltaY + (j * 0.5f), -1.29f), Quaternion.identity, parentOfLocs.transform);
                    
                    prefabsTilse[PixelRandIndex].GetComponent<LocationInfoAndMouseEnter>().LocY = i;
                    prefabsTilse[PixelRandIndex].GetComponent<LocationInfoAndMouseEnter>().LocX = j;
                    if (i == 0 && j == 0)
                    {
                        GameObject.Find("ObjectForAllLocation").transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
                        GameObject.Find("PlayerScreenCenter").GetComponent<CameraMove>().CurrentLocPlayerStay = GameObject.Find("ObjectForAllLocation").transform.GetChild(0).gameObject;
                    }

                }

                /*switch (PixelRandIndex)
                {
                    case 0:
                        MapTexture.texture.SetPixel(i, j, new Color(88/255f, 88/255f, 70/255f));//Library
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f-0.25f, j*1f+ 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    case 1:
                        MapTexture.texture.SetPixel(i, j, new Color(97 / 255f, 75 / 255f, 44 / 255f));//Private house
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f- 0.25f, j*1f- 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    case 2:
                        MapTexture.texture.SetPixel(i, j, new Color(144 / 255f, 74 / 255f, 40 / 255f));//Minimarket
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f+ 0.25f, j*1f- 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    case 3:
                        MapTexture.texture.SetPixel(i, j, new Color(61 / 255f, 82 / 255f, 28 / 255f));//forest
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f+ 0.25f, j*1f- 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    case 4:
                        MapTexture.texture.SetPixel(i, j, new Color(93 / 255f, 93 / 255f, 93 / 255f));//road
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f+ 0.25f, j*1f- 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    case 5:
                        MapTexture.texture.SetPixel(i, j, new Color(60 / 255f, 115 / 255f, 42 / 255f));//—ейчас будет пустошь, но когда по€витс€ тайл больницы надо будет заменить
                        Instantiate(prefabsTilse[PixelRandIndex], new Vector3(i*1f+ 0.25f, j*1f- 0.25f, -1.29f), Quaternion.identity, parentOfLocs.transform);
                        break;
                    default:
                        break;
                }*/

                LocColor = MapTexture.texture.GetPixel(i, j);


            }
            deltaX += 1f;
            deltaY += 0.5f;
        }
        MapTexture.texture.Apply();

        foreach (Transform child in parentOfLocs.transform)
        {            
            child.gameObject.transform.GetComponent<SpriteRenderer>().sortingOrder = OrderLayers;
            OrderLayers -= 1;
        }
    }

    
}
