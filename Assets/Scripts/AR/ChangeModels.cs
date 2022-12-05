using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeModels : MonoBehaviour
{
	public GameObject[] modelos;
	public GameObject[] modelosPrefab;

	public ChangeColorModel prefabs;
	
	//public ColorOriginalDeLosPrefabs colorOriginal;

	//Colores
    public Color colorOriginalSpider;
	public Color colorOriginalArbol;

	//llamo otro script con una variable publica que recibe un objeto con el otro script 
	public ActivarVfxColores activarVfx;

	//Activa los modelos mediante un id
    public void activarModelos(int iD)
    {
	    ApagarGameObjects(modelos);
	    
	    switch(iD)
	    {
	    	case 0:
		    	modelos[0].SetActive(true);
		    	activarVfx.DesactivarVfx();
		    	ObtenerColor2(colorOriginalSpider);
		    	break;
		    	
	    	case 1:
		    	modelos[1].SetActive(true);
		    	activarVfx.DesactivarVfx();
		    	ObtenerColor2(colorOriginalArbol);
		    	break;
	    }
	  
    }
    
	public void ColorOriginal()
	{
		
		StartCoroutine(Esperar2());

	}
    
	
	private void ObtenerColor2(Color colores)
	{
		
		StartCoroutine(Esperar(colores));
		
	}
	
	
	IEnumerator Esperar(Color colores){
		
		yield return new WaitForSeconds(0.00001f);
		prefabs.ObtenerColor(colores);
		
	}
	
	IEnumerator Esperar2(){
		
		yield return new WaitForSeconds(0.0000001f);
		for (int gameobjectIndex = 0; gameobjectIndex < modelosPrefab.Length; gameobjectIndex++)
		{
			ObtenerColor2(modelosPrefab[gameobjectIndex].GetComponent<ColorOriginalDeLosPrefabs>().colorDelModelo);
		}
		activarVfx.DesactivarVfx();		
	}
		


	private void ApagarGameObjects(GameObject[] objeto)
	{
		//Apaaga todos los gameobjects en el array
		for (int gameobjectIndex = 0; gameobjectIndex < objeto.Length; gameobjectIndex++)
		{
			objeto[gameobjectIndex].SetActive(false);
		}
	}

	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		activarVfx = GameObject.FindObjectOfType(typeof(ActivarVfxColores)) as ActivarVfxColores;
	}
	
	// Update is called every frame, if the MonoBehaviour is enabled.
	protected void Update()
	{
		modelosPrefab = GameObject.FindGameObjectsWithTag("Modelo");
	}
}

 
