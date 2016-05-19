using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class staminaControlloer : MonoBehaviour {

	public RectTransform staminaBar;
	public bool isrun;
	public bool stopRun;
	public float maxLength;
	private float localLength;
	public float decaySpeed;
	public float increaseSpeed;

	// Use this for initialization
	void Start () {
		localLength = maxLength;
		stopRun = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (localLength <= maxLength && localLength >= 0) {
			staminaBar.localScale =new Vector3 (localLength / maxLength, 1f, 1f);

			if (isrun) {
				decrease ();
			} else {
				increase ();
			}
		}

		if (localLength > maxLength)
			resetBar ();

		// if stamina is used up, stop run
		if (localLength < 0) {
			localLength = 0;
			stopRun = true;
		} else
			stopRun = false;
	}


	void decrease(){
		localLength -= decaySpeed * Time.deltaTime;
	}

	void increase(){
		localLength += increaseSpeed * Time.deltaTime;
	}

	void resetBar(){
		localLength = maxLength;
	}
}
