using System;
using System.Collections.Generic;
using Firebase.Firestore;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelModulos : MonoBehaviour
{
    // Start is called before the first frame update

    private FirebaseFirestore database;
    [HideInInspector]
    public TextMeshProUGUI Name, ShadowName, DocumentId, ModuleName, GradeId, CourseId, ModulesId;
    [HideInInspector]
    public GameObject ListOfOptions;

    public GameObject PrefabSquareOption, PrefabLockedSquareOption;


    public void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        /*
            Panel Modulos
                Popup - Clouds
                    Headline
                        Title Text
        */
        ModuleName = transform.GetChild(0).GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        /*
            Panel Modulos
                Popup - Clouds
                    Headline
                        Title Text
        */
        ListOfOptions = transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        /*
        Panel Modulos
            Database
                GradeId
        */
        GradeId = transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        Panel Modulos
            Database
                CourseId
        */
        CourseId = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Panel Modulos
            Database
                CourseId
        */
        ModulesId = transform.GetChild(2).GetChild(2).GetComponent<TextMeshProUGUI>();
        GradeId.text = PlayerPrefs.GetString("GradeId");
        CourseId.text = PlayerPrefs.GetString("CourseId");
        GetFinishedLessons();
    }

    public void SetData(string title)
    {
        ModuleName.text = title;
    }

    protected async void GetFinishedLessons()
    {
        User user = new User();
        Query capitalQuery = database.Collection("Users").Document(user.GetString("UID")).Collection("FinishedLessons").Document(GradeId.text + "_" + CourseId.text).Collection("Modules");
        QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
        bool locked = true;
        string name = "", image = "";
        GameObject squareOption = gameObject;
        foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
        {
            Dictionary<string, object> courses = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in courses)
            {
                if (pair.Key.Equals("Locked"))
                {
                    locked = Convert.ToBoolean(pair.Value.ToString());
                }
            }
            DocumentReference docRef = database.Collection("Grade").Document(GradeId.text).Collection("Course").Document(CourseId.text).Collection("Modules").Document(documentSnapshot.Id);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                Dictionary<string, object> course = snapshot.ToDictionary();
                foreach (KeyValuePair<string, object> pair in course)
                {
                    if (pair.Key.Equals("Name"))
                    {
                        name = pair.Value.ToString();
                    }
                    if (pair.Key.Equals("Image"))
                    {
                        image = pair.Value.ToString();
                    }
                }
                if (locked)
                {
                    squareOption = Instantiate(PrefabLockedSquareOption);
                    squareOption.GetComponent<SquareOption>().SetData(name);
                }
                else
                {
                    squareOption = Instantiate(PrefabSquareOption);
                    squareOption.GetComponent<SquareOption>().SetData("Modulos", name, image, snapshot.Id);
                    squareOption.GetComponent<SquareOption>().setAction("Modulos");
                }

                squareOption.SetActive(true);
                squareOption.transform.SetParent(ListOfOptions.transform, false);
                squareOption.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

}
