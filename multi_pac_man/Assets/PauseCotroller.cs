using UnityEngine;
using System.Collections;

public class PauseCotroller : MonoBehaviour {

	public string BackLevel;
	public GameObject pauseCanvas;
	private bool isPause;

	// Use this for initialization
	void Start () {
		isPause = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPause == true) {
			pauseCanvas.SetActive (true);
		} else {
			pauseCanvas.SetActive(false);
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			isPause = ! isPause;
		}
	}

	public void resume(){
		isPause = false;
	}

	public void leave(){
		PhotonNetwork.Disconnect();
		Application.LoadLevel (BackLevel);

	}

}
