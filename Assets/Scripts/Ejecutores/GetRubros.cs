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

public class GetRubros : MonoBehaviour
{
    public Transform parentButtonsCompanies;
    public GameObject botonesCompanies;
    public List<ListaRubros> listaRubros;
    public List<VacantesYEmpresas> listaVacantes;
    //public ComunicacionJoelRubros joelRubros;
    private void Start()
    {
        StartCoroutine(GetRubrosFromApi());
        StartCoroutine(GetVacantesYEmpresas());
        //joelRubros = FindObjectOfType<ComunicacionJoelRubros>();
    }
  
    IEnumerator GetRubrosFromApi()
    {

        string uri = "https://metaverso-back-dev.azurewebsites.net/api/Solicitantes/GetCategoriaEmpresa?IdCategoria=0";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                RubrosFiltrados usuariosRegistrados = JsonConvert.DeserializeObject<RubrosFiltrados>(request.downloadHandler.text);
                var json = JsonConvert.SerializeObject(usuariosRegistrados.DataList, Formatting.Indented);

                // List<DataList> dataLists = JsonConvert.DeserializeObject<List<DataList>>(json);
                listaRubros = JsonConvert.DeserializeObject<List<ListaRubros>>(json);
                foreach (ListaRubros r in listaRubros)
                {
                    // Debug.Log(r.Categoria);
                    GameObject go = Instantiate(botonesCompanies, parentButtonsCompanies);
                    go.GetComponent<textoEmpresas>().textCompaniesButton.text = r.Categoria;
                }
                //joelRubros.cantidadDeRubros = listaRubros;

            }
        }
    }

    IEnumerator GetVacantesYEmpresas()
    {
        string uri = "https://metaverso-back-dev.azurewebsites.net/api/Solicitantes/vacantes-metaverso?Id=0&IdEmpresa=0";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                VacantesAPI usuariosRegistrados = JsonConvert.DeserializeObject<VacantesAPI>(request.downloadHandler.text);
                var json = JsonConvert.SerializeObject(usuariosRegistrados.DataList, Formatting.Indented);

                // List<DataList> dataLists = JsonConvert.DeserializeObject<List<DataList>>(json);
                listaVacantes = JsonConvert.DeserializeObject<List<VacantesYEmpresas>>(json);
                foreach (VacantesYEmpresas r in listaVacantes)
                {
                    Debug.Log(r.Categoria);
                    
                }
                //joelRubros.cantidadDeRubros = listaRubros;

            }
        }
    }
}
