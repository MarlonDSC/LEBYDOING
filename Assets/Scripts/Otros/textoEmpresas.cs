using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class textoEmpresas : MonoBehaviour
{
    public TextMeshProUGUI textCompaniesButton;

    Button butonApretar;
    GetRubros losrubrositos;
    private void Start()
    {
        losrubrositos = FindObjectOfType<GetRubros>();
        butonApretar = GetComponent<Button>();
        butonApretar.onClick.AddListener(() => ListaEmpresas());
    }

    public void ListaEmpresas()
    {
        foreach (VacantesYEmpresas r in losrubrositos.listaVacantes)
        {
            if (textCompaniesButton.text == r.Categoria)
            {
                Debug.Log(r.Empresa);
            }
        }
    }
}
