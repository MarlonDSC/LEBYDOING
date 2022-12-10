using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ChatEnabler : MonoBehaviourPunCallbacks 
{

    Ray ray;
    RaycastHit hit;
    private PhotonView PV;
	public GameObject go;

    private void Start()
    {
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        PV = GetComponent<PhotonView>();
    }
    void Update()
    {


        UpdateCanvasAccept();

    }

    public void UpdateCanvasAccept()
    {
        if (PV.IsMine)
        {
            PV.RPC("OpenChatInvitation", RpcTarget.AllBuffered, true);
        }
    }
    [PunRPC]
    void OpenChatInvitation(bool activated)
    {
        if (Camera.main != null)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {

                if (hit.collider.CompareTag("Player"))
                {
                    go = hit.collider.gameObject.GetComponentInChildren<OpenCanvasChat>().canvasAceptarchat;
                    hit.collider.gameObject.GetComponentInChildren<OpenCanvasChat>().canvasAceptarchat.SetActive(activated);
                    hit.collider.gameObject.GetComponent<OpenCanvasChat>().canvasAceptarchat.SetActive(activated);
                    if (go != null)
                    {
                        go.SetActive(activated);
                    }


                    gameObject.GetComponentInChildren<OpenCanvasChat>().canvasSolicitud.SetActive(activated);
                    Debug.Log("Seleccionaste al player");
                }
            }

        }

       
       
       
    }
}
