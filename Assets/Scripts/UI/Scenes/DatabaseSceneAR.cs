using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using Ricimi;
using System;

public class DatabaseSceneAR : MonoBehaviour
{
    FirebaseFirestore database;

    TextMeshProUGUI GradeId, CourseId, ModulesId, LessonId, LessonIndex;
    public GameObject ImageTarget, TestPrefab, AyudaManager;
    GameObject ListOfButtons, ARPrefab, Canvas, LevelName;
    Button BookButton;
    void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        /*
        Canvas
            PanelAR
                Buttons-Objects
        */
        /*
        Canvas
            Database
                GradeId
        */
        GradeId = transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                CourseId
        */
        CourseId = transform.GetChild(4).GetChild(1).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                ModulesId
        */
        ModulesId = transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                LessonId
        */
        LessonId = transform.GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            Database
                ModulesId
        */
        LessonIndex = transform.GetChild(4).GetChild(4).GetComponent<TextMeshProUGUI>();
        /*
        Canvas
            PanelAR
                Button - Objets
        */
        ListOfButtons = transform.GetChild(1).GetChild(1).gameObject;
        /*
        Canvas
            PanelAR
                Book
                    BotonLibro
        */
        BookButton = transform.GetChild(1).GetChild(3).GetChild(2).GetComponent<Button>();
        /*
        Canvas
            PanelAR
                Letra
        */
        LevelName = transform.GetChild(1).GetChild(2).gameObject;
        /*
        This prefab will be loaded from the resources and get it's ReferenceName from Firestore
        */
        ARPrefab = gameObject;
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
        GradeId.text = PlayerPrefs.GetString("GradeId");
        CourseId.text = PlayerPrefs.GetString("CourseId");
        ModulesId.text = PlayerPrefs.GetString("ModulesId");
        LessonId.text = PlayerPrefs.GetString("LessonId");
        LessonIndex.text = PlayerPrefs.GetString("LessonIndex");
        Debug.Log("LessonIndex " + LessonIndex.text);
        GetLessonSnapshot(GradeId.text, CourseId.text, ModulesId.text, LessonId.text);
        SetActionsToButtons();
        BookButton.onClick.AddListener(() =>
        {
            InstantiateTest();
        });
    }

    public void InstantiateTest()
    {
        var popup = GameObject.Instantiate(TestPrefab);
        popup.transform.SetParent(Canvas.transform, false);
        popup.transform.localPosition = new Vector3(0, 0, 0);
        popup.SetActive(true);
        popup.GetComponent<Popup>().Open();
        popup.GetComponent<Test>().SetData(ListOfButtons);
        // for (int i = 0; i < ListOfButtons.transform.childCount; i++)
        // {
        //     popup.GetComponent<Test>().SetData(ListOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite);
        //     Debug.Log(ListOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>().gameObject.name);
        // }
    }

    public async void GetLessonSnapshot(string gradeId, string courseId, string moduleId, string lessonId)
    {
        User user = new User();
        List<object> Object = new List<object>();
        string ImageReference = "";
        string ModelReference = "";
        string Name = "";
        GameObject FBX = gameObject;
        DocumentReference docRef = database.Collection("Grade").Document(gradeId).Collection("Course")
        .Document(courseId).Collection("Modules").Document(moduleId)
        .Collection("Lessons").Document(lessonId);
        Dictionary<string, object> dataReference = new Dictionary<string, object> { };
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Dictionary<string, object> lesson = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in lesson)
            {
                if (pair.Key.Equals("Objects"))
                    Object = pair.Value as List<object>;
                if (pair.Key.Equals("Name"))
                    Name = pair.Value.ToString();
            }
            //Shadow TextMeshProUGUI
            LevelName.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Name;
            // Text TextMeshProUGUI
            LevelName.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Name;
            for (int i = 0; i < Object.Count; i++)
            {
                dataReference = (Dictionary<string, object>)Object[i];
                foreach (var key in dataReference)
                {
                    if (key.Key.Equals("ImageReference"))
                        ImageReference = key.Value.ToString();
                    if (key.Key.Equals("ModelReference"))
                        ModelReference = key.Value.ToString();
                }
                /*
                The first parameter is the image URL to download from Firestore
                The second parameter is an array of Images where I put the images that will get the same object, in this case the shadow and the background get the same image
                */
                StartCoroutine(DownloadImage(ImageReference,
                ListOfButtons.transform.GetChild(i).GetChild(1).GetComponent<Image>(),
                ListOfButtons.transform.GetChild(i).GetChild(0).GetComponent<Image>()
                ));
                //Prefabs have to be inside Resources folder, also there is no need to specify the file extension
                ARPrefab = Resources.Load<GameObject>($"Prefabs/{Name}/{ModelReference}");
                if (Convert.ToInt16(LessonIndex.text) == 1)
                {
                    AyudaManager.GetComponent<AyudaAR>().nivel1 = true;
                }
                else
                {
                    AyudaManager.GetComponent<AyudaAR>().nivel1 = false;
                }
                FBX = Instantiate(ARPrefab);
                FBX.SetActive(false);
                FBX.transform.SetParent(ImageTarget.transform, false);
                // FBX.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    public void SetActionsToButtons()
    {
        for (int i = 0; i < ListOfButtons.transform.childCount; i++)
        {
            var save = i;
            ListOfButtons.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                for (int index = 0; index < ImageTarget.transform.childCount; index++)
                {
                    var child = ImageTarget.transform.GetChild(index).gameObject;
                    if (child != null)
                        child.SetActive(false);
                    ImageTarget.transform.GetChild(save).gameObject.SetActive(true);
                    // Debug.Log(ImageTarget.transform.GetChild(save).gameObject.name);
                }


            });
        }
    }

    IEnumerator DownloadImage(string MediaUrl, params Image[] image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            for (int i = 0; i < image.Length; i++)
            {
                Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
                image[i].sprite = SpriteFromTexture2D(webTexture);
            }
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
