using Firebase.Firestore;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class LevelScene : MonoBehaviour
{
    FirebaseFirestore database;
    [HideInInspector]
    public TextMeshProUGUI Coins, Hearts, ModulesId, CourseId, GradeId;
    GameObject LevelPage;

    public GameObject PrefabUnlockedLevel, PrefabLockedLevel;
    void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        /*
        Canvas
            Life
                Text (Prefab)
        */
        Coins = transform.GetChild(3).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Coins
                Text (Prefab)
        */
        Hearts = transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Level Slider
                Level Page 1
        */
        LevelPage = transform.GetChild(4).GetChild(0).gameObject;
        /*
        Canvas
            ModulesId
        */
        ModulesId = transform.GetChild(11).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            CourseId
        */
        CourseId = transform.GetChild(12).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            CourseId
        */
        GradeId = transform.GetChild(13).GetComponent<TextMeshProUGUI>();

        gameObject.AddComponent<UserData>().GetCoins(Coins);
        gameObject.AddComponent<UserData>().GetHearts(Hearts);
        Debug.Log("hearts " + Hearts.text);
        ModulesId.text = PlayerPrefs.GetString("ModulesId");
        CourseId.text = PlayerPrefs.GetString("CourseId");
        GradeId.text = PlayerPrefs.GetString("GradeId");
        // GetLevelsSnapshot();
    }

    void Start()
    {
        GetLockedLevels();
    }

    protected async void GetLockedLevels()
    {
        User user = new User();
        Query allFinishedLessonsQuery = database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(GradeId.text + "_" + CourseId.text)
        .Collection("Modules").Document(ModulesId.text).Collection("Lessons").OrderBy("Index");
        QuerySnapshot allFinishedLessonsQuerySnapshot = await allFinishedLessonsQuery.GetSnapshotAsync();
        Int16 Index = 0;
        Int16 Stars = 0;
        bool Locked = true;
        GameObject level = gameObject;
        // string Description = "";

        foreach (DocumentSnapshot documentSnapshot in allFinishedLessonsQuerySnapshot.Documents)
        {
            Dictionary<string, object> lessons = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in lessons)
            {
                //Get index, save query and save LessonId
                if (pair.Key.Equals("Locked"))
                {
                    Locked = Convert.ToBoolean(pair.Value.ToString());
                }
                if (pair.Key.Equals("Index"))
                    Index = Convert.ToInt16(pair.Value);
                if (pair.Key.Equals("Stars"))
                    Stars = Convert.ToInt16(pair.Value);

                // if (pair.Key.Equals("Description"))
                //     Description = pair.Value.ToString();
            }
            if (Locked)
            {
                level = Instantiate(PrefabLockedLevel);
            }
            else
            {
                level = Instantiate(PrefabUnlockedLevel);
                //Setting data
                //documentSnapshot.Id is LessonId
                level.GetComponent<LevelButton>().SetData(Index, Stars, Hearts.text, GradeId.text, CourseId.text, ModulesId.text, documentSnapshot.Id);
            }
            level.SetActive(true);
            level.transform.SetParent(LevelPage.transform, false);
            level.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public async void GetLevelsSnapshot()
    {
        Query allFinishedLessonsQuery = database.Collection("Grade").Document(GradeId.text).Collection("Course").Document(CourseId.text)
        .Collection("Modules").Document(ModulesId.text).Collection("Lessons").OrderBy("Index");
        QuerySnapshot allFinishedLessonsQuerySnapshot = await allFinishedLessonsQuery.GetSnapshotAsync();
        foreach (DocumentSnapshot documentSnapshot in allFinishedLessonsQuerySnapshot.Documents)
        {
            Dictionary<string, object> lessons = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in lessons)
            {
                //Get index, save query and save LessonId
                // if (pair.Key.Equals("Description"))
                //     Description = pair.Value.ToString();
            }
            // Debug.Log("Index " + Index.ToString() + "\n" + "docId " + documentSnapshot.Id);

            GameObject level = Instantiate(PrefabUnlockedLevel);
            //Open Level
            level.SetActive(true);
            level.transform.SetParent(LevelPage.transform, false);
            level.transform.localPosition = new Vector3(0, 0, 0);
            // level.GetComponent<LevelButton>().SetData(Index, documentSnapshot.Id);
            // GetLockedLevels(level, documentSnapshot.Id);
        }
    }
    public async void GetLockedLevels(GameObject level, string CourseId)
    {
        User user = new User();
        DocumentReference docRef = database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(user.GetString("Grade") + "_" + CourseId).Collection("Modules").Document();
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Dictionary<string, object> city = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in city)
            {
                if (pair.Key.Equals("Locked"))
                {
                    level.GetComponent<SquareOption>().IsLocked(Convert.ToBoolean(pair.Value));
                }
            }
        }
    }


}


// Copyright (C) 2015-2021 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

// namespace Ricimi
// {
//     // This class manages the level scene of the demo. It handles the left and right
//     // selection buttons that are used to navigate across the available levels and their
//     // associated animations.
//     public class LevelScene : MonoBehaviour
//     {
//         public GameObject prevLevelButton;
//         public GameObject nextLevelButton;

//         public GameObject levelGroup;

//         public Text levelText;

//         private const int numLevelIndexes = 3;

//         private int m_currentLevelIndex = 0;

//         private Animator m_animator;

//         private void Awake()
//         {
//             m_animator = levelGroup.GetComponent<Animator>();
//         }

//         public void ShowPreviousLevels()
//         {
//             --m_currentLevelIndex;
//             if (m_currentLevelIndex < 0)
//                 m_currentLevelIndex = 0;

//             SetLevelText(m_currentLevelIndex + 1);
//             switch (m_currentLevelIndex)
//             {
//                 case 0:
//                     if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation4"))
//                         m_animator.Play("Animation4");
//                     DisablePrevLevelButton();
//                     break;

//                 case 1:
//                     if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation3"))
//                         m_animator.Play("Animation3");
//                     EnablePrevLevelButton();
//                     EnableNextLevelButton();
//                     break;

//                 default:
//                     break;
//             }
//         }

//         public void ShowNextLevels()
//         {
//             ++m_currentLevelIndex;
//             if (m_currentLevelIndex == numLevelIndexes)
//                 m_currentLevelIndex = numLevelIndexes - 1;

//             SetLevelText(m_currentLevelIndex + 1);
//             switch (m_currentLevelIndex)
//             {
//                 case 1:
//                     if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation1"))
//                         m_animator.Play("Animation1");
//                     EnablePrevLevelButton();
//                     EnableNextLevelButton();
//                     break;

//                 case 2:
//                     if (!m_animator.GetCurrentAnimatorStateInfo(0).IsName("Animation2"))
//                         m_animator.Play("Animation2");
//                     DisableNextLevelButton();
//                     break;

//                 default:
//                     break;
//             }
//         }

//         public void EnablePrevLevelButton()
//         {
//             var image = prevLevelButton.GetComponentsInChildren<Image>()[1];
//             var newColor = image.color;
//             newColor.a = 1.0f;
//             image.color = newColor;

//             var shadow = prevLevelButton.GetComponentsInChildren<Image>()[0];
//             var newShadowColor = shadow.color;
//             newShadowColor.a = 1.0f;
//             shadow.color = newShadowColor;

//             prevLevelButton.GetComponent<AnimatedButton>().interactable = true;
//         }

//         public void DisablePrevLevelButton()
//         {
//             var image = prevLevelButton.GetComponentsInChildren<Image>()[1];
//             var newColor = image.color;
//             newColor.a = 40 / 255.0f;
//             image.color = newColor;

//             var shadow = prevLevelButton.GetComponentsInChildren<Image>()[0];
//             var newShadowColor = shadow.color;
//             newShadowColor.a = 0.0f;
//             shadow.color = newShadowColor;

//             prevLevelButton.GetComponent<AnimatedButton>().interactable = false;
//         }

//         public void EnableNextLevelButton()
//         {
//             var image = nextLevelButton.GetComponentsInChildren<Image>()[1];
//             var newColor = image.color;
//             newColor.a = 1.0f;
//             image.color = newColor;

//             var shadow = nextLevelButton.GetComponentsInChildren<Image>()[0];
//             var newShadowColor = shadow.color;
//             newShadowColor.a = 1.0f;
//             shadow.color = newShadowColor;

//             nextLevelButton.GetComponent<AnimatedButton>().interactable = true;
//         }

//         public void DisableNextLevelButton()
//         {
//             var image = nextLevelButton.GetComponentsInChildren<Image>()[1];
//             var newColor = image.color;
//             newColor.a = 40 / 255.0f;
//             image.color = newColor;

//             var shadow = nextLevelButton.GetComponentsInChildren<Image>()[0];
//             var newShadowColor = shadow.color;
//             newShadowColor.a = 0.0f;
//             shadow.color = newShadowColor;

//             nextLevelButton.GetComponent<AnimatedButton>().interactable = false;
//         }

//         private void SetLevelText(int level)
//         {
//             levelText.text = level.ToString() + "/3";
//         }
//     }
// }
