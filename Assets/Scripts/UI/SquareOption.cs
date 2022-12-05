using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase.Firestore;
using Ricimi;
using System;

public class SquareOption : MonoBehaviour
{
    private FirebaseFirestore database;
    [HideInInspector]
    public TextMeshProUGUI Name, ShadowName, CourseId, ModulesId, Locked;
    [HideInInspector]
    public Image Image, LockedOption;
    // [HideInInspector]
    // public bool active;
    [HideInInspector]
    public Button Option;
    [HideInInspector]
    public GameObject Canvas, ProgressBar;
    public GameObject PrefabSquareOption, PrefabPanelModules;
    private void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        /*
            Square Option
                Title
        */
        Name = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        /*
            Square Option
                shadow Text
        */
        ShadowName = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        /*
            Square Option
                Id
        */
        CourseId = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        /*
            Square Option
                Id
        */
        ModulesId = transform.GetChild(5).GetComponent<TextMeshProUGUI>();
        /*
            Square Option
                Frame
                    square white
                        Image
        */
        Image = transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>();
        /*
            Square Option
                Frame
                    square white
                        Progress Bar - Blue
        */
        // ProgressBar = transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        // Debug.Log(ProgressBar.name);
        /*
            Square Option
                Frame
                    square white
                        Image
        */
        Option = transform.GetChild(1).GetChild(0).GetComponent<Button>();
        /*
            Square Option
                Frame
                    Locked
        */
        ProgressBar = transform.GetChild(1).GetChild(1).gameObject;
        /*
            Square Option
                Frame
                    Locked
        */
        Locked = transform.GetChild(6).GetComponent<TextMeshProUGUI>();
        /*
            Canvas
        */
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
    }

    public void SetData(string name)
    {
        Name.text = name;
        ShadowName.text = name;
    }

    public void SetData(string panel, string name, string ImageURL, string Id)
    {
        StartCoroutine(DownloadImage(ImageURL, Image));
        Name.text = name;
        ShadowName.text = name;
        if (panel == "Temas")
            CourseId.text = Id;
        else if (panel == "Modulos")
            ModulesId.text = Id;
    }

    public void setAction(string panel)
    {
        if (panel == "Temas")
        {
            Option.onClick.AddListener(() =>
            {
                GameObject popup = gameObject;
                popup = Instantiate(PrefabPanelModules);
                popup.SetActive(true);
                popup.transform.SetParent(Canvas.transform, false);
                PlayerPrefs.SetString("CourseId", CourseId.text);
                popup.transform.localPosition = new Vector3(0, 0, 0);
                popup.GetComponent<Popup>().Open();
                popup.GetComponent<PanelModulos>().SetData(Name.text);
            });
        }
        else if (panel == "Modulos")
        {
            Option.onClick.AddListener(() =>
            {
                Color colour;
                ColorUtility.TryParseHtmlString("#106A8F", out colour);
                PlayerPrefs.SetString("ModulesId", ModulesId.text);
                Option.GetComponent<SceneTransition>().PerformTransition("Level", 1.0f, colour);
            });
        }
    }

    public async void GetModuleSnapshot()
    {
        GameObject popup = Instantiate(PrefabPanelModules);
        OpenModuleLevel(popup);
        // Debug.Log("GradeId " + GradeId.text);
        Debug.Log("CourseId " + CourseId.text);
        Query allModulesQuery = database.Collection("Grade").Document("GradeId").Collection("Course").Document(CourseId.text).Collection("Modules");
        QuerySnapshot allModulesQuerySnapshot = await allModulesQuery.GetSnapshotAsync();
        string ModuleName = "", ModuleImage = "";
        foreach (DocumentSnapshot documentSnapshot in allModulesQuerySnapshot.Documents)
        {
            Debug.Log("Children in Modules " + documentSnapshot.Id);
            Dictionary<string, object> modules = documentSnapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in modules)
            {
                if (pair.Key.Equals("Name"))
                    ModuleName = pair.Value.ToString();
                if (pair.Key.Equals("Image"))
                    ModuleImage = pair.Value.ToString();
            }
            GameObject option = Instantiate(PrefabSquareOption);
            //Name.text gives us the title
            OpenSquareOption(popup, option, ModuleName, ModuleImage, CourseId.text, documentSnapshot.Id);
            // GetLockedLevels(option, option.GetComponent<SquareOption>().GradeId.text, documentSnapshot.Id);
        }
    }

    public async void GetLockedLevels(GameObject popup, string CourseId, string ModuleId)
    {
        User user = new User();
        DocumentReference docRef = database.Collection("Users").Document(user.GetString("UID"))
        .Collection("FinishedLessons").Document(user.GetString("Grade") + "_" + CourseId).Collection("Modules").Document(ModuleId);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Dictionary<string, object> city = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in city)
            {
                if (pair.Key.Equals("Locked"))
                {
                    popup.GetComponent<SquareOption>().IsLocked(Convert.ToBoolean(pair.Value));
                }
            }
        }
    }

    public void OpenModuleLevel(GameObject popup)
    {
        popup.SetActive(true);
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
        popup.transform.SetParent(Canvas.transform, false);
        popup.transform.localPosition = new Vector3(0, 0, 0);
        popup.GetComponent<PanelModulos>().SetData(Name.text);
        popup.GetComponent<Popup>().Open();
        // popup.GetComponent<SquareOption>().SetData(Name, Image, GradeId, Id);
    }

    public void OpenSquareOption(GameObject popup, GameObject option, string Name, string Image, string GradeId, string Id)
    {
        // Debug.Log("Nombre " + Name + "\n" + "Image " + Image);
        // var option = Instantiate(PrefabSquareOption) as GameObject;
        GameObject ListOfOptions = popup.GetComponent<PanelModulos>().ListOfOptions;
        // Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
        option.SetActive(true);
        option.transform.SetParent(ListOfOptions.transform, false);
        option.transform.localPosition = new Vector3(0, 0, 0);
        // option.GetComponent<SquareOption>().SetData(Name, Image, GradeId, Id);
    }

    public void IsLocked(bool isLocked)
    {
        Locked.text = isLocked.ToString();
        LockedOption.gameObject.SetActive(isLocked);
        // Debug.Log(Name.text + " " + Locked.text);
    }

    IEnumerator DownloadImage(string MediaUrl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        request.SendWebRequest();
        // yield return request.SendWebRequest();
        while (!request.isDone)
        {
            // Debug.Log("download Progress " + request.downloadProgress * 100);
            if (request.downloadProgress == 1)
            {
                ProgressBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (request.downloadProgress * 100).ToString("0") + "%";
            }
            else
            {
                ProgressBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (request.downloadProgress * 100).ToString("0.00") + "%";

            }
            ProgressBar.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = request.downloadProgress;
            // ProgressBar.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = (request.downloadProgress * 100).ToString("0.00") + "%";
            // ProgressBar.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Image>().fillAmount = request.downloadProgress;
            // yield return new WaitForSeconds(0.2f);
            yield return null;
        }
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else if (request.isDone)
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            image.sprite = SpriteFromTexture2D(webTexture);
            image.color = new Color32(255, 255, 225, 255);
            Destroy(ProgressBar);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
