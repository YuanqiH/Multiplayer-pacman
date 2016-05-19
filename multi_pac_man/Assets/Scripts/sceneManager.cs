using UnityEngine;
using System.Collections;

public class sceneManager : MonoBehaviour {


	// Use this for initialization
	public GameObject playerPrefab_1;
	//public GameObject playerPrefab_2;
	public GameObject ghost;
	public GameObject BirthPoint;
	public GameObject ghostBirthPoint;
	public int maxPlayer;

	// Use this for initialization
/*	void Start () {	
		if(!PhotonNetwork.isMasterClient)//any client create their own first
			createPlayer ();
	}*/
	// on the other client screen there is only local !!
	public void createPlayer(){
		string localName = "";
		Vector3 localBirth =Vector3.zero;
		if (PlayerPrefs.GetString("playerName") == "pacman"){
			localName = playerPrefab_1.name.ToString();
			localBirth = BirthPoint.transform.position;
		}
		if (PlayerPrefs.GetString("playerName") == "ghost") {
			localName = ghost.name.ToString ();
			localBirth = ghostBirthPoint.transform.position;
		}
		if (PhotonNetwork.room.playerCount <= maxPlayer) {
			PhotonNetwork.Instantiate (localName,localBirth , Quaternion.identity, 0);
			Debug.Log ("the new spawn born: " + localName+ " the total player is "+ PhotonNetwork.room.playerCount.ToString() );
		}
	}
}
