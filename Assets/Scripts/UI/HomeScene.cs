using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeScene : MonoBehaviour
{
    Image profilePicture;
    GameObject Canvas;

    Button LoginButton;
    Button SignOutButton;
    // public GameObject MessagePrefab;

    void Awake()
    {
        /*
        Canvas
            Buttons - Register
                Button - Login
        */
        LoginButton = transform.GetChild(5).GetChild(0).GetComponent<Button>();
        /*
        Canvas
            Buttons - Register
                Button - Sign Out
        */
        SignOutButton = transform.GetChild(5).GetChild(1).GetComponent<Button>();
        InitData();
    }

    public void InitData()
    {
        profilePicture = transform.GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>();
        gameObject.AddComponent<UserData>().DownloadImageProfile(profilePicture);
        //SetGrade without parameters reads the grade of the user based on the SetUID() which is the DocumentId
        gameObject.GetComponent<UserData>().SetGrade();
        ShowSignInOrSignUp();
    }

    public void SignOut(GameObject MessagePrefab)
    {
        gameObject.AddComponent<EmailLogin>().SignOut();
        gameObject.GetComponent<EmailLogin>().Popup(MessagePrefab, "Sesión Cerrada", "Se ha cerrado la sesión", "Ok", 3, 3);
        StartCoroutine(ReloadScene());
    }

    public IEnumerator ReloadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowSignInOrSignUp()
    {
        if (PlayerPrefs.GetString("UID").Equals(""))
        {
            //Turn on sign in button
            LoginButton.gameObject.SetActive(true);
            SignOutButton.gameObject.SetActive(false);
        }
        else
        {
            //Turn on Sign out button
            LoginButton.gameObject.SetActive(false);
            SignOutButton.gameObject.SetActive(true);
        }
    }
}
