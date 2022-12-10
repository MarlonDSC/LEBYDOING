using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPlayer : MonoBehaviour
{
	
	/// <Joel>
	DataHolder data; 
	/// </Joel> 
	
    public Material[] colorPielHombre, colorPielMujer, ojos;
    public SkinnedMeshRenderer[] pielCabeza;
    public SkinnedMeshRenderer[] pielBrazosHombre, pielBrazosMujer;
    public Button pielButton1, pielButton2, ojosButton1, ojosButton2;

    private Material[] materialesCabeza, materialesBrazos, materialesCabezaMujer, materialesBrazosMujer;
    private int pielInt = 0, ojosInt = 0;

    void Start()
	{ 
		data = FindObjectOfType<DataHolder>();
    	
        //aqui estan los  botones para cambiar la piel
        pielButton1.onClick.AddListener(() => CambiarPiel(true));
        pielButton2.onClick.AddListener(() => CambiarPiel(false));
        ojosButton1.onClick.AddListener(() => CambiarOjos(true));
        ojosButton2.onClick.AddListener(() => CambiarOjos(false));
       
		materialesCabeza = pielCabeza[0].materials;
        materialesBrazos = pielBrazosHombre[0].materials;
        materialesCabezaMujer = pielCabeza[1].materials;
        materialesBrazosMujer = pielBrazosMujer[0].materials;

    }

    void CambiarPiel(bool sentido)
    {
        if (sentido)
            pielInt++;

        else
            pielInt--;

        //Estos if es para que no se salgan del array de manera que puedas darle la vuelta a los skins sin problema
        if (pielInt > colorPielHombre.Length - 1)
        {
            pielInt = 0;
        }
        else if (pielInt < 0)
        {
            pielInt = colorPielHombre.Length - 1;
        }
        materialesCabeza[1] = colorPielHombre[pielInt];
        materialesBrazos[0] = colorPielHombre[pielInt];
        materialesCabezaMujer[0] = colorPielMujer[pielInt];
        materialesBrazosMujer[0] = colorPielMujer[pielInt];
        pielCabeza[0].materials = materialesCabeza;
        pielCabeza[1].material = materialesCabezaMujer[0];
        //esta funcion es para cambiar la piel en todos los tshirts y sueters ya que estos tienen el mismo material
        for (int i = 0; i < pielBrazosHombre.Length; i++)
        {
            pielBrazosHombre[i].material = materialesBrazos[0];
            pielBrazosMujer[i].material = materialesBrazosMujer[0];
        }
        
	    data.LPiel = pielInt; 
    }

    void CambiarOjos(bool sentido)
    {
        if (sentido)
            ojosInt++;

        else
            ojosInt--;

        //Estos if es para que no se salgan del array de manera que puedas darle la vuelta a los skins sin problema
        if (ojosInt > ojos.Length - 1)
        {
            ojosInt = 0;
        }
        else if (ojosInt < 0)
        {
            ojosInt = ojos.Length - 1;
        }

        materialesCabeza[0] = ojos[ojosInt];
        pielCabeza[0].materials = materialesCabeza;
        materialesCabezaMujer[3] = ojos[ojosInt];
        pielCabeza[1].materials = materialesCabezaMujer;
    }

    
}
