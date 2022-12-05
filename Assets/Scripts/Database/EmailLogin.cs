using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ricimi;
using Firebase.Auth;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmailLogin : MonoBehaviour
{
    [HideInInspector]
    public TMP_InputField emailInput;
    [HideInInspector]
    public TMP_InputField passwordInput;
    public GameObject MessagePrefab;
    GameObject Canvas;
    private FirebaseAuth auth;
    private bool fetchingToken = false;
    protected FirebaseAuth otherAuth;
    protected Dictionary<string, FirebaseUser> userByAuth =
    new Dictionary<string, FirebaseUser>();

    protected string email = "";
    protected string password = "";
    protected string displayName = "";

    private string logText = "";
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    bool UIEnabled = true;
    const int kMaxLogSize = 16382;
    protected bool signInAndFetchProfile = false;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    private void Awake()
    {
        /*We don't want to init Firebase token on Sign Up and we don't want 
        to get errors about email and password input for screens that don't require these elements*/
        if (gameObject.name.Contains("Register - Sign Up"))
        {
            /*
            Register - Sign Up
                InputFields
                    InputField (1)
            */
            emailInput = transform.GetChild(3).GetChild(1).gameObject.GetComponent<TMP_InputField>();
            /*
            Register - Sign Up
                InputFields
                    InputField (2)
            */
            passwordInput = transform.GetChild(3).GetChild(2).gameObject.GetComponent<TMP_InputField>();
        }
        else if (gameObject.name.Contains("Register - Log In"))
        {
            /*
            Register - Login
                    InputField (TMP) (1)
            */
            Debug.Log("Register - Log in");
            emailInput = transform.GetChild(1).gameObject.GetComponent<TMP_InputField>();
            Debug.Log("email " + emailInput.text);
            /*
            Register - Login
                    InputField (TMP) (2)
            */
            passwordInput = transform.GetChild(2).gameObject.GetComponent<TMP_InputField>();
            Debug.Log("password " + passwordInput.text);
        }
        Debug.Log("gameobject.name " + gameObject.name);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private Firebase.AppOptions otherAuthOptions = new Firebase.AppOptions
    {
        ApiKey = "AIzaSyAIgfze5L0NmhKeMFE-DQaBLPnwro-VHBA",
        AppId = "468739861409",
        ProjectId = "lebydoar1",
        DatabaseUrl = new Uri("https://lebydoar1.firebaseio.com")
    };

    // Handle initialization of the necessary firebase modules:
    protected void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        auth.IdTokenChanged += IdTokenChanged;
        // Specify valid options to construct a secondary authentication object.
        if (otherAuthOptions != null &&
            !(String.IsNullOrEmpty(otherAuthOptions.ApiKey) ||
              String.IsNullOrEmpty(otherAuthOptions.AppId) ||
              String.IsNullOrEmpty(otherAuthOptions.ProjectId)))
        {
            try
            {
                otherAuth = Firebase.Auth.FirebaseAuth.GetAuth(Firebase.FirebaseApp.Create(
                  otherAuthOptions, "Secondary"));
                otherAuth.StateChanged += AuthStateChanged;
                otherAuth.IdTokenChanged += IdTokenChanged;
            }
            catch (Exception)
            {
                Debug.Log("ERROR: Failed to initialize secondary authentication object.");
            }
        }
        AuthStateChanged(this, null);
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;
        if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;
            if (signedIn)
            {
                Debug.Log("AuthStateChanged Signed in " + user.UserId);
                displayName = user.DisplayName ?? "";
                DisplayDetailedUserInfo(user, 1);
            }
        }
    }

    public void SignOut()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
        UserData userData = new UserData();
        userData.SetUID("");
    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWithOnMainThread(
              task => Debug.Log(string.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
        }
    }

    // Display user information.
    protected void DisplayUserInfo(IUserInfo userInfo, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);
        var userProperties = new Dictionary<string, string> {
        {"Display Name", userInfo.DisplayName},
        {"Email", userInfo.Email},
        {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
        {"Provider ID", userInfo.ProviderId},
        {"User ID", userInfo.UserId}
      };
        foreach (var property in userProperties)
        {
            if (!string.IsNullOrEmpty(property.Value))
            {
                Debug.Log(string.Format("{0}{1}: {2}", indent, property.Key, property.Value));
            }
        }
    }

    // Display a more detailed view of a FirebaseUser.
    protected void DisplayDetailedUserInfo(FirebaseUser user, int indentLevel)
    {
        string indent = new string(' ', indentLevel * 2);
        DisplayUserInfo(user, indentLevel);
        Debug.Log(string.Format("{0}Anonymous: {1}", indent, user.IsAnonymous));
        Debug.Log(string.Format("{0}Email Verified: {1}", indent, user.IsEmailVerified));
        Debug.Log(string.Format("{0}Phone Number: {1}", indent, user.PhoneNumber));
        var providerDataList = new List<IUserInfo>(user.ProviderData);
        var numberOfProviders = providerDataList.Count;
        if (numberOfProviders > 0)
        {
            for (int i = 0; i < numberOfProviders; ++i)
            {
                Debug.Log(string.Format("{0}Provider Data: {1}", indent, i));
                DisplayUserInfo(providerDataList[i], indentLevel + 2);
            }
        }
    }

    public void CallCreateUserWithEmailAsync()
    {
        Debug.Log(emailInput.text + " " + passwordInput.text);
        if (emailInput.text.Equals("") || passwordInput.text.Equals(""))
        {
            Popup("Error", "Ingresa un correo y una contraseña", "Ok", 7, 7);
        }
        else
        {
            if (passwordInput.text.Length >= 8)
            {
                CreateUserWithEmailAsync();
            }
            else
            {
                Popup("Advertencia", "", "Ok", 7, 7);
            }

        }
    }

    public Task CreateUserWithEmailAsync()
    {
        email = emailInput.text;
        password = passwordInput.text;
        DebugLog(String.Format("Attempting to create user {0}...", email));

        // This passes the current displayName through to HandleCreateUserAsync
        // so that it can be passed to UpdateUserProfile().  displayName will be
        // reset by AuthStateChanged() when the new user is created and signed in.
        string newDisplayName = displayName;
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
          .ContinueWithOnMainThread((task) =>
          {
              if (LogTaskCompletion(task, "User Creation"))
              {
                  UserData userData = new UserData();
                  var user = task.Result;
                  DisplayDetailedUserInfo(user, 1);
                  userData.CreateUser(user);
                  Popup("Bienvenido", user.Email + " se ha creado tu cuenta exitosamente, ve a modificar tu perfil en el botón de la persona", "Ok", 1, 1);
                  return UpdateUserProfileAsync(newDisplayName: newDisplayName);
              }
              return task;
          }).Unwrap();
    }

    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            DebugLog(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            DebugLog(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    authErrorCode = String.Format("AuthError.{0}: ",
                      ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                }
                DebugLog(authErrorCode + exception.ToString());
                // Check out more exceptions at https://firebase.google.com/docs/reference/unity/namespace/firebase/auth
                if (authErrorCode.Contains("EmailAlreadyInUse"))
                {
                    Popup("Error", "Ya existe una cuenta con este correo, inicia sesión y vuelve a intentar", "Ok", 7, 7);
                }

                if (authErrorCode.Contains("EmailAlreadyInUse"))
                {
                    Popup("Error", "La contraseña es incorrecta o el usuario no existe", "Ok", 7, 7);
                }
                if (authErrorCode.Contains("WrongPassword"))
                {
                    Popup("Contraseña incorrecta", "Vuelve a intentarlo nuevamente o contacta con LEBYDO para restaurarla", "Ok", 7, 7);
                }

                if (authErrorCode.Contains("UserDisabled"))
                {
                    Popup("Error", "El usuario está deshabilitado, contacta con LEBYDO para más información", "Ok", 7, 7);
                }
            }
        }
        else if (task.IsCompleted)
        {
            DebugLog(operation + " completed");
            complete = true;
        }
        return complete;
    }

    // Update the user's display name with the currently selected display name.
    public Task UpdateUserProfileAsync(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            DebugLog("Not signed in, unable to update user profile");
            return Task.FromResult(0);
        }
        displayName = newDisplayName ?? displayName;
        DebugLog("Updating user profile");
        return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName,
            PhotoUrl = auth.CurrentUser.PhotoUrl,
        }).ContinueWithOnMainThread(task =>
        {
            if (LogTaskCompletion(task, "User profile"))
            {
                DisplayDetailedUserInfo(auth.CurrentUser, 1);
            }
        });
    }

    public void DebugLog(string s)
    {
        Debug.Log(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }
        scrollViewVector.y = int.MaxValue;
    }

    public void CallSignInWithEmailCredentialAsync()
    {
        Debug.Log(emailInput.text + " " + passwordInput.text);
        if (emailInput.text.Equals("") || passwordInput.text.Equals(""))
        {
            Popup("Error", "Ingresa un correo y una contraseña", "Ok", 7, 7);
        }
        else
        {
            if (passwordInput.text.Length >= 8)
            {
                SigninWithEmailCredentialAsync();
            }
            else
            {
                Popup("Advertencia", "Por tu seguridad te recomendamos ingresar una contraseña mayor a 7 dígitos, intenta nuevamente", "Ok", 7, 7);
            }

        }
    }

    public void Popup(string title, string message, string action, int borderColour, int buttonColour)
    {
        var popup = Instantiate(MessagePrefab) as GameObject;
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
        popup.SetActive(true);
        popup.transform.SetParent(Canvas.transform, false);
        popup.transform.localPosition = new Vector3(0, 0, 0);
        popup.gameObject.GetComponent<Popup>().Open();
        popup.gameObject.GetComponent<ModalMessage>().textTitle = title;
        popup.gameObject.GetComponent<ModalMessage>().textMessage = message;
        popup.gameObject.GetComponent<ModalMessage>().textAction = action;
        popup.gameObject.GetComponent<ModalMessage>().spriteIndexBorderColour = borderColour;
        popup.gameObject.GetComponent<ModalMessage>().spriteIndexButtonColour = borderColour;
    }

    public void Popup(GameObject MessagePrefab, string title, string message, string action, int borderColour, int buttonColour)
    {
        var popup = Instantiate(MessagePrefab);
        Canvas = GameObject.Find("Canvas").GetComponent<Canvas>().gameObject;
        popup.SetActive(true);
        popup.transform.SetParent(Canvas.transform, false);
        popup.transform.localPosition = new Vector3(0, 0, 0);
        popup.gameObject.GetComponent<Popup>().Open();
        popup.gameObject.GetComponent<ModalMessage>().textTitle = title;
        popup.gameObject.GetComponent<ModalMessage>().textMessage = message;
        popup.gameObject.GetComponent<ModalMessage>().textAction = action;
        popup.gameObject.GetComponent<ModalMessage>().spriteIndexBorderColour = borderColour;
        popup.gameObject.GetComponent<ModalMessage>().spriteIndexButtonColour = borderColour;
    }

    // This is functionally equivalent to the Signin() function.  However, it
    // illustrates the use of Credentials, which can be aquired from many
    // different sources of authentication.
    public Task SigninWithEmailCredentialAsync()
    {
        email = emailInput.text;
        password = passwordInput.text;
        DebugLog(String.Format("Attempting to sign in as {0}...", email));
        if (signInAndFetchProfile)
        {
            return auth.SignInAndRetrieveDataWithCredentialAsync(
              Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
                HandleSignInWithSignInResult);
        }
        else
        {
            return auth.SignInWithCredentialAsync(
              Firebase.Auth.EmailAuthProvider.GetCredential(email, password)).ContinueWithOnMainThread(
                HandleSignInWithUser);
        }
    }
    void HandleSignInWithSignInResult(Task<Firebase.Auth.SignInResult> task)
    {
        if (LogTaskCompletion(task, "Sign-in"))
        {
            DisplaySignInResult(task.Result, 1);
        }
    }

    // Called when a sign-in without fetching profile data completes.
    void HandleSignInWithUser(Task<Firebase.Auth.FirebaseUser> task)
    {
        if (LogTaskCompletion(task, "Sign-in"))
        {
            UserData userData = new UserData();
            userData.SetUID(task.Result);
            DebugLog(String.Format("{0} signed in", task.Result.DisplayName));
            DebugLog(String.Format("{0} signed in", task.Result.UserId));
            Popup("Bienvenido", task.Result.Email, "Ok", 1, 1);
            Canvas.gameObject.GetComponent<HomeScene>().InitData();
        }
    }

    // Display user information reported
    protected void DisplaySignInResult(Firebase.Auth.SignInResult result, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        DisplayDetailedUserInfo(result.User, indentLevel);
        var metadata = result.Meta;
        if (metadata != null)
        {
            DebugLog(String.Format("{0}Created: {1}", indent, metadata.CreationTimestamp));
            DebugLog(String.Format("{0}Last Sign-in: {1}", indent, metadata.LastSignInTimestamp));
        }
        var info = result.Info;
        if (info != null)
        {
            DebugLog(String.Format("{0}Additional User Info:", indent));
            DebugLog(String.Format("{0}  User Name: {1}", indent, info.UserName));
            DebugLog(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
            DebugLog(String.Format("{0}  Provider ID: {1}", indent, info.ProviderId));
            DisplayProfile<string>(info.Profile, indentLevel + 1);
        }
    }

    // Display additional user profile information.
    protected void DisplayProfile<T>(IDictionary<T, object> profile, int indentLevel)
    {
        string indent = new String(' ', indentLevel * 2);
        foreach (var kv in profile)
        {
            var valueDictionary = kv.Value as IDictionary<object, object>;
            if (valueDictionary != null)
            {
                DebugLog(String.Format("{0}{1}:", indent, kv.Key));
                DisplayProfile<object>(valueDictionary, indentLevel + 1);
            }
            else
            {
                DebugLog(String.Format("{0}{1}: {2}", indent, kv.Key, kv.Value));
            }
        }
    }
}
