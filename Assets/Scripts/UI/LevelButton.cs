using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ricimi;
using System;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    TextMeshProUGUI Index, LessonId;
    GameObject ListOfStars;
    public GameObject PrefabStartGame, PrefabLevelStar;
    private void Awake()
    {
        /*
        Level Button
            Text (TMP)
        */
        Index = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        /*
        Level Button
            Text (TMP)
        */
        LessonId = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        /*
        Level Button
            Stars
        */
        ListOfStars = transform.GetChild(2).gameObject;
    }

    public void SetData(Int16 index, Int16 stars, string hearts, string gradeId, string courseId, string moduleId, string lessonId)
    {
        GameObject star = gameObject;
        Index.text = index.ToString();
        LessonId.text = lessonId;
        for (int i = 0; i < 3; i++)
        {
            star = Instantiate(PrefabLevelStar);
            star.SetActive(true);
            if (i < stars)
            {
                star.GetComponent<LevelStar>().SetData("Yelow");
            }
            else
            {
                star.GetComponent<LevelStar>().SetData("Gray");
            }
            star.transform.SetParent(ListOfStars.transform, false);
            star.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (Convert.ToInt16(hearts) > 0)
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                GameObject Canvas = GameObject.Find("Canvas");
                GameObject popup = Instantiate(PrefabStartGame);
                //Open Level
                popup.SetActive(true);
                popup.transform.SetParent(Canvas.transform, false);
                popup.transform.localPosition = new Vector3(0, 0, 0);
                popup.GetComponent<StartGame>().SetData(index, lessonId);
                // popup.GetComponent<StartGame>().GetLessonSnapshot(gradeId, courseId, moduleId, lessonId);
                popup.GetComponent<Popup>().Open();
                // Debug.Log("LevelButton " + lessonId);
            });
        }
        else
        {
            //TODO unimplemented buy hearts
        }
    }
}
