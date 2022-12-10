using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persona : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Modicables;
    //public GameObject[] BodyParts;
    //public GameObject Legs;

    private void OnEnable()
    {
        for(int i = 0; i < Canvas.transform.childCount; i++)
        {
            if (Canvas.transform.GetChild(i).GetComponent<ClothingBaseSwitcher>() != null)
            {
                for (int j = 0; j < Modicables.transform.childCount; j++)
                {
                    if (Canvas.transform.GetChild(i).GetComponent<ClothingBaseSwitcher>().type == Modicables.transform.GetChild(j).name)
                    {
                        Canvas.transform.GetChild(i).GetComponent<ClothingBaseSwitcher>().listOfObjects = Modicables.transform.GetChild(j).gameObject;
                    }
                }
            }
            else if (Canvas.transform.GetChild(i).GetComponent<HairColorSwitch>() != null)
            {
                for (int j = 0; j < Modicables.transform.childCount; j++)
                {
                    if (Canvas.transform.GetChild(i).GetComponent<HairColorSwitch>().type == Modicables.transform.GetChild(j).name)
                    {
                        Canvas.transform.GetChild(i).GetComponent<HairColorSwitch>().listOfHairsGO = Modicables.transform.GetChild(j).gameObject;
                    }
                }
            }
        }
    }
}
