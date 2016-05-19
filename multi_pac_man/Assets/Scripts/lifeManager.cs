using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class lifeManager : MonoBehaviour {

	public int lifeStart;
	public Text countText;
	public GameObject Gameover;
	public string mainManu;

	private int lifeCounter;
	public float waitingTimeAfterOver;


	// Use this for initialization
	void Start () {
		countText = GameObject.Find("lifeCount").GetComponent<Text>();
		lifeCounter = lifeStart;
		//Gameover = GameObject.Find ("GameOver_canvas"); 
	}
	
	// Update is called once per frame
	void Update () {

		if (lifeCounter <= 0) {
			Gameover.SetActive(true);
		}


		countText.text = "x " + lifeCounter;

		if (Gameover.activeSelf) {
			waitingTimeAfterOver -= Time.deltaTime;
		}

		if (waitingTimeAfterOver <= 0) {
			Application.LoadLevel(mainManu);
			PhotonNetwork.Disconnect();
		}

	}

	public void takeLife(){
		lifeCounter--;
	}

	public void givenLife(){
		lifeCounter++;
	}
}
