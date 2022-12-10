using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
public class LogIn : MonoBehaviour
{

    public TMP_InputField correoInput;
    public TextMeshProUGUI emergencyText;
    public GameObject emergencyGO, logInGO;
    public Button logInButton;

    List<CorreosUsuarios> correosUsuarios;
    InitialConnect initConect;
    void Start()
    {
        //initConect = FindObjectOfType<InitialConnect>();
        StartCoroutine(GetCorreosSolicitantes());
        logInButton.onClick.AddListener(() =>
        {
            LogInScene();
        });
    }

    IEnumerator GetCorreosSolicitantes()
    {

        string uri = "https://metaverso-back-dev.azurewebsites.net/api/Solicitantes/solicitudes-correo";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                UsuariosRegistrados usuariosRegistrados = JsonConvert.DeserializeObject<UsuariosRegistrados>(request.downloadHandler.text);
                var json = JsonConvert.SerializeObject(usuariosRegistrados.DataList, Formatting.Indented);

                // List<DataList> dataLists = JsonConvert.DeserializeObject<List<DataList>>(json);
                correosUsuarios = JsonConvert.DeserializeObject<List<CorreosUsuarios>>(json);
            
            }
        }
    }

    void LogInScene()
    {
        foreach (CorreosUsuarios r in correosUsuarios)
        {
            if (r.CorreoElectronico == correoInput.text)
            {
                SceneManager.LoadScene(1);
                Debug.Log("Correo es igual");
            }
            else
            {
                logInGO.SetActive(false);
                emergencyGO.SetActive(true);
                emergencyText.text = "El correo electrónico no es válido, por favor intentar de nuevo";
            }
        }
    }
}
