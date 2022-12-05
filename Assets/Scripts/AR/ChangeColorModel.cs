using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorModel : MonoBehaviour
{
	//Modelos
	[HideInInspector]
	public GameObject[] modelos;

    //llamo otro script con una variable publica que recibe un objeto con el otro script 
    public ActivarVfxColores activarVfx;

    //Colores
    public Color colorAzul;
    public Color colorRojo;
    public Color colorVerde;
    public Color colorAmarillo;

    //Cambia el color de los modelos con el tag "player"
    //Cada id es un tipo de color
    public void CambiarColor(int colorId)
    {
        switch (colorId)
        {
            case 0:
                ObtenerColor(colorAzul);
                activarVfx.ActivarEfecto(0);
                break;

            case 1:
                ObtenerColor(colorRojo);
                activarVfx.ActivarEfecto(1);
                break;

            case 2:
                ObtenerColor(colorVerde);
                activarVfx.ActivarEfecto(2);
                break;

            case 3:
                ObtenerColor(colorAmarillo);
                activarVfx.ActivarEfecto(3);
                break;
        }
    }

    //Toma una variable de tipo Color y cambia el material del modelo por dicho color
	public void ObtenerColor(Color colores)
    {
        for (int objectIndex = 0; objectIndex < modelos.Length; objectIndex++)
        {
            if (modelos[objectIndex].GetComponent<Renderer>().material.HasProperty("Color_4B9AABFA"))
            {
                modelos[objectIndex].GetComponent<Renderer>().material.SetColor("Color_4B9AABFA", colores);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        modelos = GameObject.FindGameObjectsWithTag("Player");

    }

    // Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
    protected void Start()
    {
        //la variable accede al script en los componentes y me permite usar las funciones del otro script
        activarVfx = GameObject.FindObjectOfType(typeof(ActivarVfxColores)) as ActivarVfxColores;
    }
    

}
