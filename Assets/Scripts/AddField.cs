using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddField : MonoBehaviour
{
    public GameObject panelParent;
    private GameObject fieldToAdd;
    public void AgregarCampo()
    {
        fieldToAdd = Instantiate(gameObject);
        fieldToAdd.transform.SetParent(panelParent.transform);
        fieldToAdd.transform.SetSiblingIndex(5);
    }
}
