using UnityEngine;
using System.Collections;

public class PlayerChoose : MonoBehaviour {


	public waitingStart Waiting;
	private GameObject pacimage;
	private GameObject ghostimage;

	void Start(){
		pacimage = GameObject.Find("pacImage");
		ghostimage = GameObject.Find("ghostImage");
		pacimage.SetActive (false);
		ghostimage.SetActive (false);
	}

	void Update(){
		if (Waiting.roleIsChosen == false) {
			pacimage.SetActive (false);
			ghostimage.SetActive (false);
		}
	}

	public void choosePac(){
		PlayerPrefs.SetString ("playerName", "pacman");
		Debug.Log ("you have choose pacman");
		Waiting.roleIsChosen = true;
		Debug.Log ("roleischos " + Waiting.roleIsChosen.ToString());
		pacimage.SetActive (true);

	}
	
	public void chooseGhost(){
		PlayerPrefs.SetString ("playerName", "ghost");
		Debug.Log ("you have choose ghost");
		Waiting.roleIsChosen = true;
		Debug.Log ("roleischos " + Waiting.roleIsChosen.ToString());
		ghostimage.SetActive (true);
	}
	
}
