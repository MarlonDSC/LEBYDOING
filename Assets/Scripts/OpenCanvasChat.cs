using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class OpenCanvasChat : MonoBehaviour
{
    [SerializeField] PhotonChatManager photoChatManager;
    public GameObject canvasSolicitud, canvasNoSePuede, canvasEspera, canvasAceptarchat;
    [SerializeField] private bool conectado,aceptar;

    public OpenCanvasChat esteEs;
    private void Start()
    {
        aceptar = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //photoChatManager.textCanvas.SetActive(true);
            photoChatManager.privateReceiver = other.gameObject.GetComponentInChildren<OpenCanvasChat>().photoChatManager.username;
            conectado = other.gameObject.GetComponentInChildren<OpenCanvasChat>().photoChatManager.estaOcupado;
            esteEs = other.gameObject.GetComponentInChildren<OpenCanvasChat>();
            Debug.Log("Entro en el player");
            other.gameObject.GetComponentInChildren<OpenCanvasChat>().canvasAceptarchat.SetActive(true);
        }
       
    }

  
   private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {

          
         
            
        }
    }
  

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {        
            photoChatManager.textCanvas.SetActive(false);
            other.gameObject.GetComponentInChildren<OpenCanvasChat>().canvasAceptarchat.SetActive(false);

        }     
    }
   
    public void EnviarSolicitud()
    {
        esteEs.canvasAceptarchat.SetActive(aceptar);
        if (conectado)
        {
            canvasNoSePuede.SetActive(true);
        }
        else
        {
            canvasEspera.SetActive(true);
            aceptar = !aceptar;       
            Debug.Log(aceptar);
        }
          
    }
}
