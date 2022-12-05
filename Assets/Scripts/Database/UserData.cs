using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class UserData : MonoBehaviour
{
    FirebaseFirestore database;
    //Profile
    Image ProfilePicture;
    TMP_InputField Username, FullName;
    TextMeshProUGUI Hearts, Coins, Grade;
    GameObject editUsernameButton, editNameButton;
    Toggle Sex;
    //Statistics
    TextMeshProUGUI Wins, Losses, Rank, Gifts;

    private void Awake()
    {
        database = FirebaseFirestore.DefaultInstance;
        if (gameObject.name.Contains("Profile"))
        {
            //Profile
            /*
            Profile
                Profile
                    Avatar
                        Mask
                            Image
            */
            ProfilePicture = transform.GetChild(3).GetChild(5).GetChild(2).GetChild(0).gameObject.GetComponent<Image>();
            /*
            Profile
                Profile
                    Input Fields
                        InputField (TMP)
            */
            Username = transform.GetChild(3).GetChild(1).GetChild(0).gameObject.GetComponent<TMP_InputField>();
            /*
            Profile
                Profile
                    Input Fields
                        InputField (TMP)
            */
            FullName = transform.GetChild(3).GetChild(1).GetChild(1).gameObject.GetComponent<TMP_InputField>();
            /*
            Profile
                Profile
                    Credits
                        Life
                            Text - Pink
            */
            Hearts = transform.GetChild(3).GetChild(2).GetChild(0).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Profile
                    Credits
                        Life
                            Text
            */
            Coins = transform.GetChild(3).GetChild(2).GetChild(1).GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Profile
                    Input Fields
                        InputField (TMP)
                            Button Circle - Teal
            */
            Grade = transform.GetChild(3).GetChild(4).GetChild(3).GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Profile
                    Input Fields
                        InputField (TMP)
                            Button Circle - Teal
            */
            editUsernameButton = Username.transform.GetChild(2).gameObject;
            /*
            Profile
                Profile
                    Input Fields
                        InputField (TMP)
                            Button Circle - Teal
            */
            editNameButton = FullName.transform.GetChild(2).gameObject;

            //Statistics
            /*
            Profile
                Statistics
                    Items
                        Item (0)
                            Text
            */
            Wins = transform.GetChild(4).GetChild(1).GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Statistics
                    Items
                        Item (1)
                            Text
            */
            Losses = transform.GetChild(4).GetChild(1).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Statistics
                    Items
                        Item (2)
                            Text
            */
            Rank = transform.GetChild(4).GetChild(1).GetChild(2).GetChild(3).GetComponent<TextMeshProUGUI>();
            /*
            Profile
                Statistics
                    Items
                        Item (3)
                            Text
            */
            Gifts = transform.GetChild(4).GetChild(1).GetChild(3).GetChild(3).GetComponent<TextMeshProUGUI>();
            GetUserData();
        }
    }

    public void SetUID(FirebaseUser NewUser)
    {
        PlayerPrefs.SetString("UID", NewUser.UserId);
    }

    public async void SetGrade()
    {
        database = FirebaseFirestore.DefaultInstance;
        string Grade = "";
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        // TextMeshProUGUI CreatedAt = transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            Grade = User.Grade;
            Query query = database.Collection("Grade").WhereEqualTo("Name", User.Grade);
            ListenerRegistration listener = query.Listen(snapshot =>
            {
                foreach (DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    Debug.Log(documentSnapshot.Id);
                    PlayerPrefs.SetString("GradeId", documentSnapshot.Id);
                    // InstantiateThemes(documentSnapshot.Id);
                }
            });
            // PlayerPrefs.SetString("GradeId", Grade);
        }
    }

    public void SetGrade(string Grade)
    {
        PlayerPrefs.SetString("GradeId", Grade);
    }
    public void SetUID(string UserId)
    {
        PlayerPrefs.SetString("UID", UserId);
    }

    public string GetGrade()
    {
        User user = new User();
        Debug.Log("Printing " + user.GetString("Grade"));
        return user.GetString("Grade");
    }

    public string GetUID()
    {
        User user = new User();
        return user.GetString("UID");
    }
    public void CreateUser(FirebaseUser NewUser)
    {
        database = FirebaseFirestore.DefaultInstance;
        string[] fullName = NewUser.DisplayName.Split(' ');

        DocumentReference docRef = database.Collection("Users").Document(NewUser.UserId);
        User user = new User
        {
            Username = "",
            Name = "",
            LastName = "",
            Hearts = 20,
            Coins = 500,
            Sex = "Not Selected",
            ProfilePicture = "https://firebasestorage.googleapis.com/v0/b/lebydoar1.appspot.com/o/Project%2FAvatars%2FNone.png?alt=media&token=4152ac72-b540-4009-ab31-094e641b5bd3",
            PhoneNumber = NewUser.PhoneNumber,
            UID = NewUser.UserId,
            Email = NewUser.Email,
            CreatedAt = DateTime.Now,
            Grade = "Not specified",
            Statistics = new Dictionary<string, dynamic>
            {
                { "Gifts", 0 },
                { "Losses", 0 },
                { "Rank", 0 },
                { "Wins", 0 }
            },
            // Statistics = new List<object>() { 0, 0, 0, 0 }

        };
        docRef.SetAsync(user);
        Debug.Log("Data taken");
        // PlayerPrefs.SetString("UID", NewUser.UserId);
        SetUID(NewUser);
        SetGrade(user.Grade);
    }

    public void UpdateUserData(FirebaseUser User)
    {

    }

    public async void UpdateUserName()
    {
        DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
        await UsersRef.UpdateAsync("Username", Username.text);
    }
    public async void UpdateFullName()
    {
        string[] fullName = FullName.text.Split(' ');
        DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
        Dictionary<string, object> name = new Dictionary<string, object>
        {
            { "Name", fullName[0] },
            { "LastName", fullName[1] },
        };
        await UsersRef.UpdateAsync(name);
    }

    public async void UpdateSex(Toggle Sex)
    {
        ToggleGroup SexGroup = transform.GetChild(3).GetChild(3).GetComponent<ToggleGroup>();
        int numberOfActive = 0;
        foreach (var item in SexGroup.ActiveToggles())
        {
            numberOfActive++;
        }

        if (numberOfActive != 0)
        {
            DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
            await UsersRef.UpdateAsync("Sex", Sex.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
        }
        // if (numberOfActive == 0)
        // {
        //     database = FirebaseFirestore.DefaultInstance;
        //     DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
        //     await UsersRef.UpdateAsync("Sex", "Not selected");
        // }
    }

    public void GetUserData(FirebaseUser User)
    {
        DownloadImageProfile();
    }



    public void GetUserData()
    {
        DownloadImageProfile();
        GetUserDataInBulk();
        // GetStatistics();
    }

    public void DeleteUserData(FirebaseUser User)
    {

    }

    public async void DownloadImageProfile()
    {
        Query docQuery = database.Collection("Users").WhereEqualTo("UID", GetUID());

        QuerySnapshot dataQuerySnapshot = await docQuery.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in dataQuerySnapshot.Documents)
        {
            Dictionary<string, object> experiences = documentSnapshot.ToDictionary();

            foreach (KeyValuePair<string, object> pair in experiences)
            {
                if (pair.Key.Equals("ProfilePicture"))
                {
                    StartCoroutine(DownloadImage(pair.Value.ToString(), ProfilePicture));
                }
            }
        }
    }

    public async void DownloadImageProfile(Image ProfilePicture)
    {
        database = FirebaseFirestore.DefaultInstance;
        Query docQuery = database.Collection("Users").WhereEqualTo("UID", GetUID());

        QuerySnapshot dataQuerySnapshot = await docQuery.GetSnapshotAsync();

        foreach (DocumentSnapshot documentSnapshot in dataQuerySnapshot.Documents)
        {
            Dictionary<string, object> experiences = documentSnapshot.ToDictionary();

            foreach (KeyValuePair<string, object> pair in experiences)
            {
                if (pair.Key.Equals("ProfilePicture"))
                {
                    StartCoroutine(DownloadImage(pair.Value.ToString(), ProfilePicture));
                }
            }
        }
    }

    public async void GetUsername()
    {
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            Username.text = User.Username;
        }
    }

    public async void GetCoins(TextMeshProUGUI Coins)
    {
        // database = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            Coins.text = User.Coins.ToString();
        }
    }

    public async void SubtractCoins()
    {
        database = FirebaseFirestore.DefaultInstance;
        int coins = 0;
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        Debug.Log(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            coins = User.Coins;
        }
        coins--;
        DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
        await UsersRef.UpdateAsync("Coins", coins);

    }

    public async void SubtractHearts()
    {
        database = FirebaseFirestore.DefaultInstance;
        int hearts = 0;
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        Debug.Log(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            hearts = User.Hearts;
        }
        hearts--;
        DocumentReference UsersRef = database.Collection("Users").Document(GetUID());
        await UsersRef.UpdateAsync("Hearts", hearts);
    }


    public async void GetHearts(TextMeshProUGUI Hearts)
    {
        database = FirebaseFirestore.DefaultInstance;
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            Hearts.text = User.Hearts.ToString();
        }
    }

    public async void TakeHeart(TextMeshProUGUI Hearts)
    {
        int hearts = Convert.ToInt16(Hearts.text);
        database = FirebaseFirestore.DefaultInstance;
        DocumentReference cityRef = database.Collection("Users").Document(GetUID());
        await cityRef.UpdateAsync("Hearts", (hearts - 1));
    }

    public async void GetUserDataInBulk()
    {
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        TextMeshProUGUI CreatedAt = transform.GetChild(4).GetChild(2).GetComponent<TextMeshProUGUI>();
        if (snapshot.Exists)
        {
            User User = snapshot.ConvertTo<User>();
            Username.text = User.Username;
            FullName.text = User.Name + " " + User.LastName;
            Hearts.text = User.Hearts.ToString();
            Coins.text = User.Coins.ToString();
            Grade.text = User.Grade;
            if (User.Sex == "Niño")
            {
                Sex = transform.GetChild(3).GetChild(3).GetChild(1).gameObject.GetComponent<Toggle>();
                Sex.isOn = true;
            }
            else if (User.Sex == "Niña")
            {
                Sex = transform.GetChild(3).GetChild(3).GetChild(2).gameObject.GetComponent<Toggle>();
                Sex.isOn = true;
            }
            else if (User.Sex == "Not selected")
            {
                Sex = transform.GetChild(3).GetChild(3).GetChild(2).gameObject.GetComponent<Toggle>();
                Sex.isOn = false;
                Sex = transform.GetChild(3).GetChild(3).GetChild(1).gameObject.GetComponent<Toggle>();
                Sex.isOn = false;
            }
            CreatedAt.text = "Miembro desde " + User.CreatedAt.ToShortDateString();
            Dictionary<string, object> user = snapshot.ToDictionary();

            Dictionary<string, object> statistics = new Dictionary<string, object> { };
            statistics = (Dictionary<string, object>)user["Statistics"];

            foreach (var key in statistics)
            {
                if (key.Key.Equals("Wins"))
                {
                    Wins.text = key.Value.ToString();
                }
                else if (key.Key.Equals("Losses"))
                {
                    Losses.text = key.Value.ToString();
                }
                else if (key.Key.Equals("Rank"))
                {
                    Rank.text = key.Value.ToString();
                }
                else if (key.Key.Equals("Gifts"))
                {
                    Gifts.text = key.Value.ToString();
                }
            }
            SetGrade(Grade.text);
        }

    }

    public async void GetStatistics()
    {
        DocumentReference docRef = database.Collection("Users").Document(GetUID());
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            Console.WriteLine("Document data for {0} document:", snapshot.Id);
            Dictionary<string, object> user
             = snapshot.ToDictionary();
            foreach (KeyValuePair<string, object> pair in user
            )
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
            }
        }
        else
        {
            Console.WriteLine("Document {0} does not exist!", snapshot.Id);
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        while (!request.isDone)
        {
            Debug.Log("request " + request.downloadProgress * 100);
            yield return null;

        }

        // yield return null;
        // yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            Sprite webSprite = SpriteFromTexture2D(webTexture);
        }
    }

    IEnumerator DownloadImage(string MediaUrl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        while (!request.isDone)
        {
            //TODO not tested yet
            Debug.Log("request " + request.downloadProgress * 100);
            yield return request.SendWebRequest();
        }
        yield return null;


        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            Texture2D webTexture = ((DownloadHandlerTexture)request.downloadHandler).texture as Texture2D;
            image.sprite = SpriteFromTexture2D(webTexture);
        }
    }

    Sprite SpriteFromTexture2D(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}
