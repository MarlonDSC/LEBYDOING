using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComunicacionJoelRubros : MonoBehaviour
{
    public List<ListaRubros> cantidadDeRubros;

    public void MostrarRubros()
    {
        foreach(ListaRubros r in cantidadDeRubros)
        {
            Debug.Log(r.Categoria);
        }
    }
}
