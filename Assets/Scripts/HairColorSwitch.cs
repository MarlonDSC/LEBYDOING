using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HairColorSwitch : MonoBehaviour
{
    //public Material colorPeloMat;
    public Button colorPeloButton1, colorPeloButton2, colorPeloButton3, colorPeloButton4;
    public GameObject listOfHairsGO;
    public string type;
    private void Start()
    {
        colorPeloButton1.onClick.AddListener(() => ChangeColor(colorPeloButton1.image.color));
        colorPeloButton2.onClick.AddListener(() => ChangeColor(colorPeloButton2.image.color));
        colorPeloButton3.onClick.AddListener(() => ChangeColor(colorPeloButton3.image.color));
        colorPeloButton4.onClick.AddListener(() => ChangeColor(colorPeloButton4.image.color));
    }
    void ChangeColor(Color colorButton)
    {
        for (int i = 0; i < listOfHairsGO.transform.childCount; i++)
        {
            listOfHairsGO.transform.GetChild(i).gameObject.GetComponent<SkinnedMeshRenderer>().material.color = colorButton;
        }
             
    }
}
