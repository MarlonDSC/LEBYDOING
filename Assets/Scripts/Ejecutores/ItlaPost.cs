using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
public class ItlaPost : MonoBehaviour
{
    ModeloPostItla modelo = new ModeloPostItla();
    string uri = "https://metaverso-back-dev.azurewebsites.net/api/solicitantes/enviar-aplicacion";
    private void Start()
    {
        modelo.idEmpresa = 80;
        modelo.idSolicitante = 158;
        modelo.idVacante = 5;
        var json = JsonConvert.SerializeObject(modelo, Formatting.Indented);
        StartCoroutine(PostSolicitantesVacantes(uri, json));

    }
    IEnumerator PostSolicitantesVacantes(string url, string bodyJasonString)
    {
        
      
        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        Debug.Log(bodyJasonString);
        byte[] rawData = Encoding.UTF8.GetBytes(bodyJasonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(rawData);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        switch(request.result)
        {
            case UnityWebRequest.Result.InProgress:
                break;
            case UnityWebRequest.Result.Success:
                string response = request.downloadHandler.text;
                JObject dataScript = JObject.Parse(response);
                Debug.Log("Paso");
                Debug.Log(dataScript);
                break;
            case UnityWebRequest.Result.ConnectionError:            
                Debug.Log("Error conexion");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error Protocolos");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error proceso de data");
                break;
            default:
                throw new ArgumentOutOfRangeException();

        }
        
    }
}
