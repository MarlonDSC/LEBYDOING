using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ManagerVR : MonoBehaviourPunCallbacks
{
    InitConnectVRAuditorio connectAuditorio;
    void Start()
    {
        connectAuditorio = FindObjectOfType<InitConnectVRAuditorio>();
        //connectAuditorio.CreateAndJoinRoom();
    }

    public override void OnJoinedLobby()
    {
        connectAuditorio.CreateAndJoinRoom();
    }
    public override void OnJoinedRoom()
    {
        connectAuditorio.cambioScena(1);
    }
    
}
