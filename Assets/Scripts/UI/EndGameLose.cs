using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameLose : MonoBehaviour
{
    GameObject ListOfStars;
    TextMeshProUGUI Title;
    public GameObject PrefabStar, PrefabEmptyStar;

    private void Awake()
    {
        /*
        End Game win
            Top Stars
        */
        ListOfStars = transform.GetChild(2).gameObject;
        /*
        End Game win
            Headline
                Title text
        */
        Title = transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    public void SetData(string title, Int16 result)
    {
        Title.text = title;
        Title.fontSize = 60;
        var star = gameObject;
        for (int i = 0; i < 3 - result; i++)
        {
            star = Instantiate(PrefabEmptyStar) as GameObject;
            star.SetActive(true);
            star.transform.SetParent(ListOfStars.transform, false);
            star.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("estrella amarilla " + i + " estrella vacia " + result);
        }
        for (int i = 0; i < result; i++)
        {
            star = Instantiate(PrefabStar) as GameObject;
            star.SetActive(true);
            star.transform.SetParent(ListOfStars.transform, false);
            star.transform.localPosition = new Vector3(0, 0, 0);
            Debug.Log("estrella amarilla " + i + " estrella vacia " + result);
        }
        // if (optionName == "Error")
        // {
        //     if (result > 1)
        //     {
        //         Title.text = "Vuelve a Intentarlo";
        //     }
        //     else
        //     {
        //         Title.text = "Has perdido :(";
        //         //Reload the same scene
        //     }
        //     Title.fontSize = 60;

        // }
        // else
        // {
        //     if (result == 3)
        //     {
        //         Title.text = "Excelente";
        //     }
        //     else if (result == 2)
        //     {
        //         Title.text = "Muy bien";
        //     }
        //     else
        //     {
        //         Title.text = "Bien";
        //     }
        //     for (int i = 0; i < result; i++)
        //     {
        //         var star = Instantiate(PrefabStar) as GameObject;
        //         star.SetActive(true);
        //         star.transform.SetParent(ListOfStars.transform, false);
        //         star.transform.localPosition = new Vector3(0, 0, 0);
        //     }
        //     //Exit to scene
        //     //Upload to Firestore
        //     //Unlock next level
        // }
        //insert result to database
    }
}
