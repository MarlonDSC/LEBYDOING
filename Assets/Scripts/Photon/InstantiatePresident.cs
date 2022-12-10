using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class InstantiatePresident : MonoBehaviourPunCallbacks, IPunObservable
{

	public GameObject playerVr;
	public Transform SpawnPoints;
	PhotonView view;

	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		view = GetComponent<PhotonView>();
		PhotonNetwork.Instantiate(this.playerVr.name, SpawnPoints.position, Quaternion.identity);

		/*	if(view.IsMine){
		PhotonNetwork.Instantiate(this.MiPlayer.name,SpawnPoints.position, Quaternion.identity);
		}
		if(!view.IsMine){
			PhotonNetwork.Instantiate(this.MiPlayer.name,SpawnPoints.position, Quaternion.identity);
		} */
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{

		if (stream.IsWriting)
		{


		}
		else if (stream.IsReading)
		{

		}
	}

}
