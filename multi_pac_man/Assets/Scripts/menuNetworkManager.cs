using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class menuNetworkManager : MonoBehaviour {

	private string localRoomName;
	private RoomInfo[] roomList;
	private UnityEngine.UI.Text joinroomName;
	public GameObject joinRoomButton;
	public GameObject roomName;

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("0.1");
		joinRoomButton.SetActive (false);
	}


	//for createroom botton
	public void createRoom(){
		if (!PhotonNetwork.connected) {
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		} else if (PhotonNetwork.room == null) {
			localRoomName = roomName.GetComponent<Text>().text.ToString();
			PhotonNetwork.CreateRoom(localRoomName);
		}
	}
	

	public void joinRoom(){

		PhotonNetwork.JoinRoom (roomList [0].name);// what about several room 
	}

	void OnJoinedLobby()
	{
		Debug.Log ("loby name is: " + PhotonNetwork.lobby.Name.ToString () + " typey is : " + PhotonNetwork.lobby.Type.ToString ());
	}

	void OnReceivedRoomListUpdate()//only after join the lobby, can update roomlist
	{
		roomList = PhotonNetwork.GetRoomList();
		Debug.Log(" Roomlist is updated ");
		Debug.Log ("mast client ?" + PhotonNetwork.isMasterClient);
		Debug.Log ("roomList" + PhotonNetwork.countOfRooms);
		showJoinButton ();
	}

	void OnJoinedRoom()
	{
		Debug.Log ("connect to Room " + PhotonNetwork.room.name.ToString ());
		Debug.Log ("current player in the room " + PhotonNetwork.room.playerCount.ToString ());

	}

	//master is not the room hodler, no need check master
	public void showJoinButton(){
		if (/*!PhotonNetwork.isMasterClient &&*/ PhotonNetwork.countOfRooms != 0) {
			joinRoomButton.SetActive (true);
			joinroomName = joinRoomButton.GetComponentInChildren<UnityEngine.UI.Text> ();
			joinroomName.text = roomList [0].name.ToString();
		}
	}

}
