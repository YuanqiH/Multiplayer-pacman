using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class waitingStart : MonoBehaviour {

	public string startLevel;
	public GameObject waitingStartCanvas;
	public bool roleIsChosen;
	private Text playerNO;
	private Text RoomName;

	void Start(){
		waitingStartCanvas.SetActive (false);
	}

	void Update(){
		if (roleIsChosen == true) {
			waitingStartCanvas.SetActive(true);
		}
		if (roleIsChosen == false) {
			waitingStartCanvas.SetActive(false);
		}

		InforText ();

	}

	public void rechoose(){

		roleIsChosen = false;
	}

	void InforText(){
		if (roleIsChosen == true) {
			playerNO = GameObject.Find ("playerNo").GetComponent<Text> ();
			RoomName = GameObject.Find ("roomName").GetComponent<Text> ();
			RoomName.text = PhotonNetwork.room.name.ToString ();
			playerNO.text = PhotonNetwork.room.playerCount.ToString ();

		}
	}

	public void onReady(){
		Application.LoadLevel (startLevel);
	}


}
