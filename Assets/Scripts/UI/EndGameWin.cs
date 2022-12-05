using System;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameWin : MonoBehaviour
{
    GameObject ListOfStars;
    [HideInInspector]
    public TextMeshProUGUI Title, LessonId, LessonIndex;
    [HideInInspector]
    public Button Level, Forward;
    public GameObject PrefabStar, PrefabStartGame;

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
        /*
        End Game win
            Buttons
                Button Circle - Purple
        */
        Level = transform.GetChild(10).GetChild(0).GetComponent<Button>();
        /*
        End Game win
            Buttons
                Button circle - Green
        */
        Forward = transform.GetChild(10).GetChild(2).GetComponent<Button>();
        /*
        End Game win
            Database
                LevelId
        */
        LessonId = transform.GetChild(13).GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        End Game win
            Database
            `   LessonIndex
        */
        LessonIndex = transform.GetChild(13).GetChild(1).GetComponent<TextMeshProUGUI>();
        LessonIndex.text = PlayerPrefs.GetString("LessonIndex");
        Level.onClick.AddListener(() =>
        {
            Color colour;
            ColorUtility.TryParseHtmlString("#106A8F", out colour);
            Level.GetComponent<SceneTransition>().PerformTransition("Level", 1.0f, colour);
            // SceneManager.LoadScene("Level");
        });
        Forward.onClick.AddListener(() =>
        {
            Debug.Log("Implement next scene");
            GameObject popup = gameObject;
            GameObject Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
            popup = Instantiate(PrefabStartGame);
            popup.SetActive(true);
            popup.transform.SetParent(Canvas.transform, false);
            popup.transform.localPosition = new Vector3(0, 0, 0);
            popup.GetComponent<StartGame>().SetData((Int16)(Convert.ToInt16(LessonIndex.text) + 1), LessonId.text);
        });
        Debug.Log("EndGameWin");
    }

    public void SetData(string title, Int16 result)
    {
        Title.text = title;
        for (int i = 0; i < result; i++)
        {
            var star = Instantiate(PrefabStar) as GameObject;
            star.SetActive(true);
            star.transform.SetParent(ListOfStars.transform, false);
            star.transform.localPosition = new Vector3(0, 0, 0);
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
