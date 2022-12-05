using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public FirebaseFirestore database;
    public GameObject PrefabEndGameWin, PrefabEndGameLose, PrefabStartGame;
    GameObject Canvas;
    [HideInInspector]
    public GameObject Opciones;
    TextMeshProUGUI Tries, Result, GradeId, CourseId, ModulesId, LessonId, LessonIndex;
    bool pressedWrongAnswer;
    Toggle selectedToggle;
    int errorIndex = 0;
    int correctIndex = 0;
    public Sprite[] SquareSprites;
    // Start is called before the first frame update

    void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        Opciones = transform.GetChild(5).gameObject;
        Tries = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        Result = transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                GradeId
        */
        GradeId = transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            CourseId
        */
        CourseId = transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                LessonId
        */
        ModulesId = transform.GetChild(8).GetChild(2).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                ModulesId
        */
        LessonId = transform.GetChild(8).GetChild(3).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                LessonIndex
        */
        LessonIndex = transform.GetChild(8).GetChild(4).GetComponent<TextMeshProUGUI>();
        GradeId.text = PlayerPrefs.GetString("GradeId");
        CourseId.text = PlayerPrefs.GetString("CourseId");
        ModulesId.text = PlayerPrefs.GetString("ModulesId");
        LessonId.text = PlayerPrefs.GetString("LessonId");
        LessonIndex.text = PlayerPrefs.GetString("LessonIndex");
    }

    private void Start()
    {
        Tries.text = "0";
        Result.text = ((Int16)(3 - Convert.ToInt16(Tries.text))).ToString();
        pressedWrongAnswer = false;
        SetActions();
        PosicionDeLosBotonesRandom();
    }

    public void SetData(Sprite images)
    {
        for (int i = 0; i < Opciones.transform.childCount; i++)
        {
            if (Opciones.transform.GetChild(i).name != "Error" && Opciones.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Image>().sprite == null)
            {
                //Add shadow
                Opciones.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Image>().sprite = images;
                Opciones.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Image>().sprite = images;
            }
        }

    }

    public void SetData(GameObject listOfButtons)
    {
        Debug.Log("listOfButtons " + listOfButtons.transform.childCount + "\n Opciones" + Opciones.transform.childCount);
        for (int i = 0; i < listOfButtons.transform.childCount; i++)
        {
            Debug.Log(listOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite);
            Debug.Log(listOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().gameObject.name);
            for (int j = 0; j < Opciones.transform.childCount; j++)
            {
                if (Opciones.transform.GetChild(i).name != "Error" && Opciones.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Image>().sprite == null)
                {
                    //Add shadow
                    Opciones.transform.GetChild(i).GetChild(1).GetChild(2).GetComponent<Image>().sprite = listOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite;
                    Opciones.transform.GetChild(i).GetChild(1).GetChild(3).GetComponent<Image>().sprite = listOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite;
                    Debug.Log("i and j" + i + j);
                }
            }
        }
    }

    private void PosicionDeLosBotonesRandom()
    {
        for (int i = 0; i < Opciones.transform.childCount - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, Opciones.transform.childCount);
            Opciones.transform.GetChild(i).transform.SetSiblingIndex(randomIndex);
        }
    }

    public void SetActions()
    {
        for (int i = 0; i < Opciones.transform.childCount; i++)
        {
            Toggle toggle = Opciones.transform.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener((onValueChanged) =>
           {
               var popup = gameObject as GameObject;
               int activeToggles = 0;
               for (int i = 0; i < Opciones.transform.childCount; i++)
               {
                   selectedToggle = Opciones.transform.GetChild(i).GetComponent<Toggle>();
                   if (selectedToggle.isOn)
                   {
                       if (selectedToggle.name == "Error")
                       {
                           errorIndex = i;
                           Opciones.transform.GetChild(errorIndex).GetComponent<Animator>().Play("SquareError");
                           pressedWrongAnswer = true;
                       }
                       else if (selectedToggle.name != "Error")
                       {
                           correctIndex = i;
                       }
                       activeToggles++;
                   }
               }

               if (activeToggles == 2 && pressedWrongAnswer)
               {
                   Opciones.transform.GetChild(errorIndex).GetComponent<Animator>().Play("SquareError");
                   Opciones.transform.GetChild(correctIndex).GetComponent<Animator>().Play("SquareCorrect");
                   Tries.text = (Convert.ToInt16(Tries.text) + 1).ToString();
                   Result.text = (Convert.ToInt16(Result.text) - 1).ToString();
                   Debug.Log("Result " + Result.text + "\n" + "Tries " + Tries.text);
                   StartCoroutine(
                   Animate(
                        Opciones.transform.GetChild(errorIndex).GetComponent<Animator>(),
                        Opciones.transform.GetChild(correctIndex).GetComponent<Animator>()
                        )
                    );
                   Opciones.transform.GetChild(errorIndex).GetComponent<Toggle>().isOn = false;
                   Opciones.transform.GetChild(correctIndex).GetComponent<Toggle>().isOn = false;
                   if (Convert.ToInt16(Result.text) == 0)
                   {
                       popup = Instantiate(PrefabEndGameLose) as GameObject;
                       //    popup = Instantiate(PrefabEndGameWin) as GameObject;
                       popup.GetComponent<EndGameLose>().SetData("Inténtalo de nuevo", Convert.ToInt16(Result.text));
                       Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
                       popup.SetActive(true);
                       popup.transform.SetParent(Canvas.transform, false);
                       popup.transform.localPosition = new Vector3(0, 0, 0);
                       popup.gameObject.GetComponent<Popup>().Open();
                       //Exit to scene
                       //Upload to Firestore
                       //Unlock next level
                   }
                   activeToggles = 0;
                   pressedWrongAnswer = false;
               }
               else if (activeToggles == 2 && !pressedWrongAnswer)
               {
                   popup = Instantiate(PrefabEndGameWin) as GameObject;
                   if (Convert.ToInt16(Result.text) == 3)
                   {
                       popup.GetComponent<EndGameWin>().SetData("Excelente", Convert.ToInt16(Result.text));
                   }
                   else if (Convert.ToInt16(Result.text) == 2)
                   {
                       popup.GetComponent<EndGameWin>().SetData("Muy bien", Convert.ToInt16(Result.text));
                   }
                   else if (Convert.ToInt16(Result.text) == 1)
                   {
                       popup.GetComponent<EndGameWin>().SetData("Bien", Convert.ToInt16(Result.text));
                   }

                   UpdateStars();

                   CheckIfNextLevelExists(popup);

                   Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
                   popup.SetActive(true);
                   popup.transform.SetParent(Canvas.transform, false);
                   popup.transform.localPosition = new Vector3(0, 0, 0);
                   popup.gameObject.GetComponent<Popup>().Open();
                   activeToggles = 0;
               }
           });
        }
    }

    private async void UpdateStars()
    {
        User user = new User();
        DocumentReference lessonReference =
        database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(GradeId.text + "_" + CourseId.text)
        .Collection("Modules").Document(ModulesId.text).Collection("Lessons").Document(LessonId.text);
        await lessonReference.UpdateAsync("Stars", Convert.ToInt16(Result.text));
    }

    private async void CheckIfNextLevelExists(GameObject popup)
    {
        //Get the index

        //Sum the index and search if there is a level with that level
        User user = new User();
        int nextLessonIndex = (Convert.ToInt16(LessonIndex.text) + 1);
        Query capitalQuery = database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(GradeId.text + "_" + CourseId.text)
        .Collection("Modules").Document(ModulesId.text).Collection("Lessons").WhereEqualTo("Index", nextLessonIndex);
        QuerySnapshot lessonQuerySnapshot = await capitalQuery.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in lessonQuerySnapshot.Documents)
        {
            Debug.Log("Document data for document:" + documentSnapshot.Id);
            if (documentSnapshot.Id.Equals("") || documentSnapshot.Id.Equals(null))
            {
                popup.GetComponent<EndGameWin>().Forward.gameObject.SetActive(false);
            }
            else
            {
                popup.GetComponent<EndGameWin>().LessonId.text = documentSnapshot.Id;
                UnlockNextLevel(popup.GetComponent<EndGameWin>().LessonId.text);
                // popup.GetComponent<EndGameWin>().Forward.onClick.AddListener(() =>
                // {
                //     GameObject startGame = Instantiate(PrefabStartGame);
                //     Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
                //     startGame.SetActive(true);
                //     startGame.transform.SetParent(Canvas.transform, false);
                //     startGame.transform.localPosition = new Vector3(0, 0, 0);
                //     startGame.gameObject.GetComponent<Popup>().Open();
                //     startGame.GetComponent<StartGame>().SetData((Int16)nextLessonIndex, popup.GetComponent<EndGameWin>().LessonId.text);
                // });
            }
            // Dictionary<string, object> city = documentSnapshot.ToDictionary();
            // foreach (KeyValuePair<string, object> pair in city)
            // {
            //     Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            // }
        }
        //Access and change Locked = true
    }

    private async void UnlockNextLevel(string lessonId)
    {
        User user = new User();
        DocumentReference lessonReference =
        database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(GradeId.text + "_" + CourseId.text)
        .Collection("Modules").Document(ModulesId.text).Collection("Lessons").Document(lessonId);
        await lessonReference.UpdateAsync("Locked", false);
    }

    private IEnumerator Animate(params Animator[] animator)
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < animator.Length; i++)
        {
            animator[i].Play("Normal");
        }
    }
}
