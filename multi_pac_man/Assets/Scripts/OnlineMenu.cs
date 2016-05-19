using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnlineMenu : MonoBehaviour {
	
	Text roomName;
	public GameObject chooseMenuCanvas;
	public GameObject createButton;
	private bool hasRoomName;

	void Start(){
		roomName = GameObject.Find("RoomNameText").GetComponent<Text>();//find by itself
		hasRoomName = false;
	}

	void Update(){
		if (roomName.text.Length == 0)
			hasRoomName = false;
		else
			hasRoomName = true;
		showCreateButton ();
		//Debug.Log ("hasroomName" + hasRoomName);
	}

	public void inputRoomName(string input){
		roomName.text = input; 
	}

	public void moveToChooseMenu(){
			chooseMenuCanvas.SetActive (true);
	}

	public void showCreateButton(){
		if (hasRoomName == true)
			createButton.SetActive(true);
		else
			createButton.SetActive(false);

	}

}
