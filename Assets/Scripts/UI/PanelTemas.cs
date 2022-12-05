using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using System;
using Ricimi;

public class PanelTemas : MonoBehaviour
{
    private GameObject ListOfOptions;
    private FirebaseFirestore database;
    public GameObject PrefabSquareOption, PrefabLockedSquareOption, PrefabPanelModules;
    private void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        ListOfOptions = transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        // GetGradeSnapshot();
        GetFinishedLessons();
    }

    protected async void GetFinishedLessons()
    {
        User user = new User();
        Query capitalQuery = database.Collection("Users").Document(user.GetString("UID")).Collection("FinishedLessons");
        QuerySnapshot capitalQuerySnapshot = await capitalQuery.GetSnapshotAsync();
        string[] fullName;
        string gradeId = "";
        bool locked = true;
        string name = "", image = "";
        GameObject squareOption = gameObject;
        foreach (DocumentSnapshot documentSnapshot in capitalQuerySnapshot.Documents)
        {
            // Debug.Log("DocumentSnapshot " + documentSnapshot.Id);
            fullName = documentSnapshot.Id.Split('_');
            gradeId = fullName[0];
            Dictionary<string, object> courses = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in courses)
            {
                if (pair.Key.Equals("Locked"))
                {
                    locked = Convert.ToBoolean(pair.Value.ToString());
                }
            }
            DocumentReference docRef = database.Collection("Grade").Document(fullName[0]).Collection("Course").Document(fullName[1]);
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
                    squareOption.GetComponent<SquareOption>().SetData("Temas", name, image, snapshot.Id);
                    squareOption.GetComponent<SquareOption>().setAction("Temas");
                }

                squareOption.SetActive(true);
                squareOption.transform.SetParent(ListOfOptions.transform, false);
                squareOption.transform.localPosition = new Vector3(0, 0, 0);
                PlayerPrefs.SetString("GradeId", gradeId);
            }
        }
    }

    protected async void InstantiateThemes(string GradeSnapshot)
    {
        // UPwVxtvgrmddkR6crG8j
        Query allFinishedLessonsQuery = database.Collection("Grade").Document(GradeSnapshot).Collection("Course");
        QuerySnapshot allFinishedLessonsQuerySnapshot = await allFinishedLessonsQuery.GetSnapshotAsync();
        string Name = "";
        string Image = "";
        foreach (DocumentSnapshot documentSnapshot in allFinishedLessonsQuerySnapshot.Documents)
        {
            Dictionary<string, object> finishedLessons = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in finishedLessons)
            {
                if (pair.Key.Equals("Name"))
                    Name = pair.Value.ToString();
                if (pair.Key.Equals("Image"))
                    Image = pair.Value.ToString();
            }
            GameObject popup = Instantiate(PrefabSquareOption);
            //Open Level
            popup.SetActive(true);
            popup.transform.SetParent(ListOfOptions.transform, false);
            popup.transform.localPosition = new Vector3(0, 0, 0);
            popup.GetComponent<Popup>().Open();
            // popup.GetComponent<SquareOption>().SetData(Name, Image, GradeSnapshot, documentSnapshot.Id);
            // GetLockedLevels(popup, documentSnapshot.Id);
        }
        PlayerPrefs.SetString("GradeId", GradeSnapshot);
    }
    public async void GetLockedLevels(string CourseId)
    {
        User user = new User();
        DocumentReference docRef = database.Collection("Users").Document(user.GetString("UID")).Collection("FinishedLessons").Document(user.GetString("Grade") + "_" + CourseId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        GameObject SquareOption = gameObject;
        bool locked = true;
        if (snapshot.Exists)
        {
            Dictionary<string, object> courses = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in courses)
            {
                if (pair.Key.Equals("Locked"))
                {
                    locked = Convert.ToBoolean(pair.Value.ToString());
                    // SquareOption.GetComponent<SquareOption>().IsLocked(Convert.ToBoolean(pair.Value));
                }
                else
                {
                    // SquareOption = Instantiate(PrefabSquareOption);
                    // SquareOption.GetComponent<SquareOption>().SetData();
                }

                SquareOption.SetActive(true);
                SquareOption.transform.SetParent(ListOfOptions.transform, false);
                SquareOption.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    // protected async void GetLockedLevels()
    // {
    //     // User user = new User();
    //     // Query allFinishedLessonsQuery = database.Collection("Users").Document(user.GetString("UID"))
    //     // .Collection("FinishedLessons").Document(user.GetString("Grade") + "_" + CourseId.text)
    //     // .Collection("Modules").Document(ModulesId.text).Collection("Lessons").OrderBy("Index");

    //     User user = new User();
    //     Query allFinishedLessonsQuery = database.Collection("Users").Document(user.GetString("UID")).Collection("FinishedLessons").WhereEqualTo("Grade", user.GetString("Grade"));
    //     QuerySnapshot allFinishedLessonsQuerySnapshot = await allFinishedLessonsQuery.GetSnapshotAsync();
    //     GameObject squareOption = gameObject;
    //     // string Description = "";
    //     string GradeSnapshot = "";
    //     string Name = "";
    //     string Image = "";
    //     bool Locked = true;
    //     foreach (DocumentSnapshot documentSnapshot in allFinishedLessonsQuerySnapshot.Documents)
    //     {
    //         Dictionary<string, object> lessons = documentSnapshot.ToDictionary();
    //         foreach (KeyValuePair<string, object> pair in lessons)
    //         {
    //             //Get index, save query and save LessonId
    //             if (pair.Key.Equals("Locked"))
    //             {
    //                 Locked = Convert.ToBoolean(pair.Value.ToString());
    //             }
    //             else
    //             {
    //                 // UPwVxtvgrmddkR6crG8j
    //                 Query allGradesQuery = database.Collection("Grade").Document(GradeSnapshot).Collection("Course");
    //                 QuerySnapshot allGradesQuerySnapshot = await allGradesQuery.GetSnapshotAsync();

    //                 foreach (DocumentSnapshot gradedocumentSnapshot in allGradesQuerySnapshot.Documents)
    //                 {
    //                     Dictionary<string, object> GradeLessons = gradedocumentSnapshot.ToDictionary();
    //                     foreach (KeyValuePair<string, object> grade in GradeLessons)
    //                     {
    //                         if (grade.Key.Equals("Name"))
    //                             Name = grade.Value.ToString();
    //                         if (grade.Key.Equals("Image"))
    //                             Image = grade.Value.ToString();
    //                     }
    //                     GameObject popup = Instantiate(PrefabSquareOption);
    //                     //Open Level
    //                     popup.SetActive(true);
    //                     popup.transform.SetParent(ListOfOptions.transform, false);
    //                     popup.transform.localPosition = new Vector3(0, 0, 0);
    //                     // popup.GetComponent<SquareOption>().SetData(Name, Image, GradeSnapshot, documentSnapshot.Id);
    //                     GetLockedLevels(popup, documentSnapshot.Id);
    //                 }
    //                 PlayerPrefs.SetString("GradeId", GradeSnapshot);
    //             }
    //         }
    //         if (Locked)
    //         {
    //             squareOption = Instantiate(PrefabLockedSquareOption);
    //         }
    //         else
    //         {
    //             squareOption = Instantiate(PrefabSquareOption);
    //             //Setting data
    //             //documentSnapshot.Id is LessonId
    //             // squareOption.GetComponent<LevelButton>().SetData(Index, Stars, Hearts.text, GradeId.text, CourseId.text, ModulesId.text, documentSnapshot.Id);
    //             squareOption.GetComponent<SquareOption>().SetData(Name, Image, GradeSnapshot, documentSnapshot.Id);
    //         }
    //         squareOption.SetActive(true);
    //         squareOption.transform.SetParent(ListOfOptions.transform, false);
    //         squareOption.transform.localPosition = new Vector3(0, 0, 0);
    //     }
    // }
}
