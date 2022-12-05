using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExamenAR : MonoBehaviour
{
	[SerializeField] GameObject examenLibro;
	[SerializeField] AyudaAR ayudaAR;
	[HideInInspector] public bool primerBoton = false;
	[HideInInspector] public bool segundoBoton = false;
	[HideInInspector] public bool tercerBoton = false;
	
	// Awake is called when the script instance is being loaded.
	protected void Start()
	{
		examenLibro.SetActive(false);
		ayudaAR = FindObjectOfType<AyudaAR>();
	}
	
	
	public void PrimerBotonTrue()
	{
		
		primerBoton = true;
		Debug.LogWarning("primerBoton");
	}
	
	public void SegundoBotonTrue()
	{
		
		segundoBoton = true;
		Debug.LogWarning("SegundoBoton");

	}
	
	public void TercerBotonTrue()
	{
		tercerBoton = true;
		Debug.LogWarning("TercerBoton");
		
	}
	

    // Update is called once per frame
    void Update()
    {
	    if(ayudaAR.nivel()){
	    	if(primerBoton && segundoBoton && tercerBoton){
		    	examenLibro.SetActive(true);
	    	}
	    }
	    else{
	    	
	    	if(primerBoton && segundoBoton){
		    	examenLibro.SetActive(true);
	    	}
	    }
	    
	 
	}
    

 
}

