using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ricimi;
using UnityEngine.UI;
using System;

public class AyudaAR : MonoBehaviour
{

    [SerializeField, TextArea(2, 6)] private string[] arrayTexto;
    //[SerializeField, TextArea(2, 6)] private string[] arrayTexto2;
    [SerializeField] private TMP_Text dialogoTexto;
    [SerializeField] private GameObject panelDialogo;
    [SerializeField] private GameObject ayudaImg;
    [SerializeField] private GameObject cerrarImg;
    [SerializeField] private GameObject btnAyudaGrande;
    [SerializeField] private GameObject[] objetosDeLaEscena;
    [SerializeField] public bool nivel1;

    public ExamenAR botonTrue;
    public float typingTime = 0.01f;
    private bool didDialaogueStart;
    private int lineIndex;
    public GameObject[] manos;
    public GameObject[] manosPintura;
    public GameObject emoji;
    public GameObject botonContinuar;
    public GameObject[] manosRotarYEscalar;


    // esta variable me sirve para controlar la aparicion del texto al inicio de la animacion
    int velocidadDetextoNormal = 0;

    public void RemoveAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length - 1);
    }

    public void AddAt<T>(ref T[] arr, int index)
    {
        for (int a = index; a < arr.Length - 1; a++)
        {
            // moving elements downwards, to fill the gap at [index]
            arr[a] = arr[a + 1];
        }
        // finally, let's decrement Array's size by one
        Array.Resize(ref arr, arr.Length + 1);
    }

    // Update is called every frame, if the MonoBehaviour is enabled.
    protected void Update()
    {
        if (nivel1)
        {

            if (btnAyudaGrande.activeSelf)
            {

                ApagarObjetosDeLaEscena(false);
            }

            else
            {
                ApagarObjetosDeLaEscena(true);
            }
        }
        else
        {
            btnAyudaGrande.SetActive(false);
            ApagarObjetosDeLaEscena(true);
            botonContinuar.SetActive(false);
        }

        if (panelDialogo.activeSelf)
        {
            ayudaImg.SetActive(false);
        }

        if (!nivel1 && arrayTexto.Length > 2)
        {
            RemoveAt(ref arrayTexto, 6);
            RemoveAt(ref arrayTexto, 5);
            RemoveAt(ref arrayTexto, 4);
            RemoveAt(ref arrayTexto, 3);
            RemoveAt(ref arrayTexto, 2);


        }

        else if (nivel1 && arrayTexto.Length < 6)
        {
            AddAt(ref arrayTexto, 6);
            AddAt(ref arrayTexto, 5);
            AddAt(ref arrayTexto, 4);
            AddAt(ref arrayTexto, 3);
            AddAt(ref arrayTexto, 2);

            arrayTexto[0] = "Presiona el boton del arbol";
            arrayTexto[1] = "Presiona el boton de la araña";
            arrayTexto[2] = "Puedes rotar o escalar el modelo con los dedos";
            arrayTexto[3] = "Ahora presiona el boton de pintar";
            arrayTexto[4] = "Presiona uno de los botones señalados para pintar";
            arrayTexto[5] = "Ahora presiona el boton del examen";
            arrayTexto[6] = "¡Muy Bien!";

            panelDialogo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            panelDialogo.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        }
    }

    private void EncenderManos(bool SioNo, GameObject[] modelo)
    {
        for (int objectIndex = 0; objectIndex < manos.Length; objectIndex++)
        {
            modelo[objectIndex].SetActive(SioNo);
        }
    }

    private void EncenderManosPintura(bool SioNo)
    {
        for (int objectIndex = 0; objectIndex < manos.Length; objectIndex++)
        {
            manosPintura[objectIndex].SetActive(SioNo);
        }
    }

    private void ApagarObjetosDeLaEscena(bool SioNo)
    {
        for (int objectIndex = 0; objectIndex < objetosDeLaEscena.Length; objectIndex++)
        {
            objetosDeLaEscena[objectIndex].SetActive(SioNo);
        }
    }

    public void StartDialogue()
    {
        if (manosPintura.Length > 0)
        {
            if (nivel1)
            {
                didDialaogueStart = true;
                cerrarImg.SetActive(false);
                ayudaImg.SetActive(false);
                btnAyudaGrande.SetActive(false);
                manos[0].SetActive(true);
                emoji.SetActive(false);
                panelDialogo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                panelDialogo.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                panelDialogo.SetActive(true);
                lineIndex = 0;
                EncenderManos(false, manosRotarYEscalar);
                botonContinuar.SetActive(false);
                StartCoroutine(ShowLine());
            }

            else
            {


                arrayTexto[0] = "Presiona los botones señalados para ver los modelos";
                arrayTexto[1] = "¡Muy bien!";

                panelDialogo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
                panelDialogo.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
                didDialaogueStart = true;
                cerrarImg.SetActive(false);
                ayudaImg.SetActive(false);
                btnAyudaGrande.SetActive(false);
                EncenderManos(true, manos);
                emoji.SetActive(false);
                panelDialogo.SetActive(true);
                lineIndex = 0;
                StartCoroutine(ShowLine());
            }

        }
        else
        {
            didDialaogueStart = true;
            cerrarImg.SetActive(false);
            ayudaImg.SetActive(false);
            btnAyudaGrande.SetActive(false);
            EncenderManos(true, manos);
            emoji.SetActive(false);
            panelDialogo.SetActive(true);
            lineIndex = 0;
            StartCoroutine(ShowLine());
        }

    }




    public void NextPhrase()
    {

        if (nivel1)
        {

            SistemaDeAyudaNivel1();
        }

        else
        {

            if (dialogoTexto.text == arrayTexto[lineIndex])
            {
                EncenderManos(false, manos);
                cerrarImg.SetActive(true);
                emoji.SetActive(true);
                SiguienteDialogo();

            }
        }



    }

    private void SistemaDeAyudaNivel1()
    {

        if (dialogoTexto.text == arrayTexto[0] && botonTrue.primerBoton)
        {
            //panelDialogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1520, 266);
            //EncenderManos(false);
            manos[0].SetActive(false);
            manos[1].SetActive(true);
            //manosPintura[0].SetActive(true);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto0");
        }

        else if (dialogoTexto.text == arrayTexto[1] && botonTrue.segundoBoton)
        {
            EncenderManos(false, manos);
            EncenderManos(true, manosRotarYEscalar);
            botonContinuar.SetActive(true);
            //manosPintura[0].SetActive(true);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto1");
        }

        else if (dialogoTexto.text == arrayTexto[2])
        {
            panelDialogo.GetComponent<RectTransform>().anchorMin = new Vector2(0.65f, 0);
            panelDialogo.GetComponent<RectTransform>().anchorMax = new Vector2(0.65f, 0);
            EncenderManos(false, manosRotarYEscalar);
            botonContinuar.SetActive(false);
            manosPintura[0].SetActive(true);
            manosPintura[1].SetActive(false);
            manosPintura[2].SetActive(false);
            manosPintura[3].SetActive(false);
            manosPintura[4].SetActive(false);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto3");
        }

        else if (dialogoTexto.text == arrayTexto[3])
        {
            EncenderManos(false, manos);
            manosPintura[0].SetActive(false);
            manosPintura[1].SetActive(true);
            manosPintura[2].SetActive(true);
            manosPintura[3].SetActive(true);
            manosPintura[4].SetActive(true);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto2");
        }

        else if (dialogoTexto.text == arrayTexto[4])
        {
            //panelDialogo.GetComponent<RectTransform>().anchoredPosition = new Vector2(1520, 266);
            manosPintura[1].SetActive(false);
            manosPintura[2].SetActive(false);
            manosPintura[3].SetActive(false);
            manosPintura[4].SetActive(false);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto3");
        }

        else if (dialogoTexto.text == arrayTexto[5])
        {
            //EncenderManosPintura(true);

            //EncenderManos(false);
            //EncenderManosPintura(false);
            cerrarImg.SetActive(true);
            emoji.SetActive(true);
            SiguienteDialogo();
            Debug.LogWarning("arrayTExto4");
        }

        else if (dialogoTexto.text == arrayTexto[6])
        {
            //StartCoroutine(CerrarPopUp());
            panelDialogo.SetActive(false);
            Debug.LogWarning("arrayTExto5");
            Debug.LogWarning("arrayTExto4");

        }


    }

    public void SiguienteDialogo()
    {
        lineIndex++;

        if (lineIndex < arrayTexto.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialaogueStart = false;
            ayudaImg.SetActive(true);
            panelDialogo.SetActive(false);
            lineIndex = 0;
        }
    }

    private IEnumerator ShowLine()
    {
        dialogoTexto.text = string.Empty;


        while (velocidadDetextoNormal == 0)
        {
            yield return new WaitForSeconds(0.5f);
            velocidadDetextoNormal++;
        }

        foreach (char character in arrayTexto[lineIndex])
        {
            dialogoTexto.text += character;
            yield return new WaitForSeconds(typingTime);
        }
    }

    public bool nivel()
    {

        return nivel1;

    }


}
