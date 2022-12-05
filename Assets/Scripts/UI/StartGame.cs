using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using System.Collections.Generic;
using UnityEngine.UI;
using Ricimi;

public class StartGame : MonoBehaviour
{
    FirebaseFirestore database;
    TextMeshProUGUI levelNumber, LessonId, Hearts;
    GameObject Canvas;
    Button ButtonGo;
    private void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        /*
        Start Game
            Headline
                Title Text
        */
        levelNumber = transform.GetChild(1).GetChild(2).GetComponent<TextMeshProUGUI>();
        /*
        Start Game
            Database
                LessonId
        */
        LessonId = transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        Start Game
            Database
                Hearts
        */
        Hearts = transform.GetChild(8).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Start Game
            Button - Blue (Teal)
        */
        ButtonGo = transform.GetChild(7).GetComponent<Button>();
    }

    public void SetData(Int16 index, string lessonId)
    {
        levelNumber.text = levelNumber.text + " " + index.ToString();
        gameObject.AddComponent<UserData>().GetHearts(Hearts);
        ButtonGo.onClick.AddListener(() =>
        {
            if (Convert.ToInt16(Hearts.text) > 0)
            {
                Color colour;
                ColorUtility.TryParseHtmlString("#106A8F", out colour);
                Debug.Log("LessonId " + lessonId);
                PlayerPrefs.SetString("LessonId", lessonId);
                PlayerPrefs.SetString("LessonIndex", index.ToString());
                gameObject.GetComponent<UserData>().TakeHeart(Hearts);
                ButtonGo.GetComponent<SceneTransition>().PerformTransition("EscenaBaseDeDatos", 1.0f, colour);
                // SceneManager.LoadScene("DatabaseSceneAR");
            }
            else
            {
                Debug.Log("Te hacen falta corazones, compra más");
            }
        });
    }
    public async void GetLessonSnapshot(string gradeId, string courseId, string moduleId, string lessonId)
    {
        User user = new User();
        List<object> Object = new List<object>();
        DocumentReference docRef = database.Collection("Grade").Document(gradeId).Collection("Course").Document(courseId).Collection("Modules")
        .Document(moduleId).Collection("Lessons").Document(lessonId);
        Dictionary<string, object> dataReference = new Dictionary<string, object> { };
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Dictionary<string, object> lesson = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in lesson)
            {
                if (pair.Key.Equals("Objects"))
                    Object = pair.Value as List<object>;
            }
            for (int i = 0; i < Object.Count; i++)
            {
                dataReference = (Dictionary<string, object>)Object[i];
                foreach (var key in dataReference)
                {
                    if (key.Key.Equals("ImageReference"))
                        Debug.Log(key.Key + "\t\t" + key.Value);
                }
            }
        }
    }
}
