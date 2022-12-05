using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarVfxColores : MonoBehaviour
{
	GameObject efectos;
	
	//accede a los hijos del gameobject y los activa mediante un id
	public void ActivarEfecto(int idEfecto)
	{
		DesactivarVfx();		
		efectos.transform.GetChild(idEfecto).gameObject.SetActive(true);
		
	}
	
	//desactiva todos los hijos del gameobject
	public void DesactivarVfx()
	{
		for (int index = 0; index < efectos.transform.childCount; index++)
		{
			efectos.transform.GetChild(index).gameObject.SetActive(false);
		}
	}

    // Update is called once per frame
    void Update()
    {
	    efectos = GameObject.FindGameObjectWithTag("Finish");
    }
}
