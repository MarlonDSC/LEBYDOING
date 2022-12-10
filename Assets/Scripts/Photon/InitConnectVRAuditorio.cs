using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InitConnectVRAuditorio : MonoBehaviourPunCallbacks
{


	InitConnectVRAuditorio[] clone;


	protected void Awake()
	{
		clone = FindObjectsOfType<InitConnectVRAuditorio>();
		if (clone.Length > 1)
		{
			Destroy(clone[0].gameObject);
		}
		DontDestroyOnLoad(this);

		ConectarPhoton();
	}

	protected void Start()
	{

	}

	void ConectarPhoton()
	{
		PhotonNetwork.ConnectUsingSettings();
	}

	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();

	}

	public override void OnJoinedLobby()
	{


	}

	public void cambioScena(int i)
	{

		PhotonNetwork.LoadLevel(i);
	}

	public void CreateAndJoinRoom()
	{
		RoomOptions roomOptions = new RoomOptions();
		roomOptions.IsVisible = true;
		roomOptions.MaxPlayers = 200;
		PhotonNetwork.JoinOrCreateRoom("Auditorio", roomOptions, TypedLobby.Default);
	}

	public override void OnJoinedRoom()
	{

	}

	public void Home()
	{
		PhotonNetwork.LeaveRoom();

	}

	public override void OnLeftRoom()
	{


	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		PhotonNetwork.LoadLevel(0);

	}
}
