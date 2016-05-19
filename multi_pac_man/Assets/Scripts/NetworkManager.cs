using UnityEngine;
using System.Collections;
//using System;


//network connection and spawn connection
public class NetworkManager : MonoBehaviour {


	private const string roomName = "Q_pac_man";
	private RoomInfo[] roomList;

	// Use this for initialization
	public GameObject playerPrefab_1;
	public GameObject playerPrefab_2;
	public GameObject ghost;
	public GameObject BirthPoint;
	public GameObject ghostBirthPoint;


	private string role;


	
	void Start () {
		//host and join room based on apple Id
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	

	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		} 
		else if (PhotonNetwork.room == null)
		{	
			// Create Room
			if (GUI.Button(new Rect(140, 50, 150, 80), "create a room"))
				PhotonNetwork.CreateRoom(roomName);//, true, true, 5);
			// Join Room
			if (roomList != null)
			{
				for (int i = 0; i < roomList.Length; i++)
				{
					if (GUI.Button(new Rect(140, 140 + (90 * i), 150, 80), "Join " + roomList[i].name))
					{
						PhotonNetwork.JoinRoom(roomList[i].name);

					}
				}
			}


			foreach(RoomInfo game in PhotonNetwork.GetRoomList())
			{
				GUILayout.Label(game.name + " " + game.playerCount + "/" + game.maxPlayers);
			}

		}
		/*
		else if(PhotonNetwork.inRoom && PhotonNetwork.isNonMasterClientInRoom && PhotonNetwork.room.maxPlayers < 3){//chose player
			
		Debug.Log("room joined, choose player");
			if(GUI.Button(new Rect(300, 230,150,80), "ghost") ){
				role = "ghost";
				createPlayer(role);
			}
			if(GUI.Button(new Rect(300, 320,150,80), "pacman") ){
				role = "pacman";
				createPlayer(role);
			}
		}
*/
	}
	
	void OnReceivedRoomListUpdate()//only after join the lobby, can update roomlist
	{
		roomList = PhotonNetwork.GetRoomList();
		Debug.Log(" Roomlist is updated ");

	}

	void OnJoinedRoom()
	{
		Debug.Log("connect to Room " + PhotonNetwork.room.name.ToString());
		Debug.Log ("current player in the room " + PhotonNetwork.room.playerCount.ToString ());

		if (PhotonNetwork.room.playerCount == 1) {
			PhotonNetwork.Instantiate (playerPrefab_1.name, BirthPoint.transform.position, Quaternion.identity, 0);
			Debug.Log ("the first spawn born: " + playerPrefab_1.name);
		} 
		if (PhotonNetwork.room.playerCount == 2) {
			PhotonNetwork.Instantiate (playerPrefab_1.name, BirthPoint.transform.position, Quaternion.identity, 0);
			Debug.Log ("the first spawn born: " + playerPrefab_1.name);
		} 
		if (PhotonNetwork.room.playerCount == 3) {
			PhotonNetwork.Instantiate (ghost.name, ghostBirthPoint.transform.position, Quaternion.identity, 0);
			Debug.Log ("the first spawn born: " + ghost.name);
		} 
		//createPlayer ();

	}

	void OnJoinedLobby()
	{
		Debug.Log ("loby name is: " + PhotonNetwork.lobby.Name.ToString () + " typey is : " + PhotonNetwork.lobby.Type.ToString ());
	}

	void createPlayer(){
		string localName = "";
		Vector3 localBirth =Vector3.zero;
		if (PlayerPrefs.GetString("playerName") == "pacman"){
			localName = playerPrefab_2.name.ToString();
			localBirth = BirthPoint.transform.position;
		}
		if (PlayerPrefs.GetString("playerName") == "ghost") {
			localName = ghost.name.ToString ();
			localBirth = ghostBirthPoint.transform.position;
		}
		if (PhotonNetwork.room.playerCount < 3) {
			PhotonNetwork.Instantiate (localName,localBirth , Quaternion.identity, 0);
			Debug.Log ("the new spawn born: " + localName+ " the total player is "+ PhotonNetwork.room.playerCount.ToString() );
		}
	}

}


