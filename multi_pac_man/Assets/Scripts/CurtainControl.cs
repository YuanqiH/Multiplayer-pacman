using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurtainControl : Photon.MonoBehaviour {

	private GameObject curtain_canvas;
	public GameObject beginButton;
	public GameObject createButton;

	private sceneManager scenemana;
	private GameObject toggle;
	private Text oneready;
	private Text twoready;
	private Text waitingState;
	private int playerCount = 0;
	private bool isShowButton;
	// Use this for initialization
	void Start () {
		curtain_canvas = GameObject.Find ("curtain_canvas");
	//	oneready = GameObject.Find ("oneReady").GetComponent<Text> ();
	//	twoready = GameObject.Find ("twoReady").GetComponent<Text> ();
		waitingState = GameObject.Find ("waitingState").GetComponent<Text> ();
		scenemana = FindObjectOfType<sceneManager> ();
		beginButton.SetActive (false);
		createButton.SetActive (false);
		if(!PhotonNetwork.isMasterClient){//if not master,tell master you r in
			GetComponent<PhotonView>().RPC ("playerIn", PhotonTargets.MasterClient, null);
			createButton.SetActive(true);
			waitingState.text=("You have to wait room holder to begin the game");
		}
	}

	void Update(){

		//ReadyState ();

		BeginState ();
		if (PhotonNetwork.isMasterClient) {
			showBeginButton ();
		}
	}

	public void onBegin(){
		if(PhotonNetwork.isMasterClient)
			GetComponent<PhotonView>().RPC ("onpenCurtain", PhotonTargets.All, null);
		scenemana.createPlayer ();//create master's charactor
	}

	public void showBeginButton(){
		if (isShowButton == true) {
			beginButton.SetActive (true);
			waitingState.text = "";
		} else {
			beginButton.SetActive (false);
			waitingState.text = "Waiting for " + (PhotonNetwork.room.playerCount - 1 - playerCount).ToString () + " player ready ....";
		}
	}


	/*//ignore this ready State,it's bad, can be improved
	public void ReadyState(){
		if (PhotonNetwork.isMasterClient) {//can be improved consider other player
			if (playerCount == 1){
				GetComponent<PhotonView>().RPC ("checkToggle_1", PhotonTargets.AllBuffered, null);
			}
			if (playerCount == 2){
				GetComponent<PhotonView>().RPC ("checkToggle_2", PhotonTargets.AllBuffered, null);
			}
		}
	}
*/
	public void BeginState(){
		if (playerCount + 1 == PhotonNetwork.room.playerCount) {
			isShowButton = true;
		} else {
			isShowButton = false;
		}
	}

	[PunRPC]
	void onpenCurtain(){
		curtain_canvas.SetActive (false);
	}


	[PunRPC]
	void playerIn(){
		playerCount += 1;
		Debug.Log ("I have send playerCount " + playerCount.ToString ());
	}
	/*
	[PunRPC]
	public void showButton(bool value){
		if (!PhotonNetwork.isMasterClient) {
			createButton.SetActive(value);
		}
	}
*/
}
