using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerEatCotroller : MonoBehaviour {
	
	public Text appleText;
	public int appleStart;
	public int appleCount;
	public bool isPower;
	public float PowerTime;
	private float localTime;
	public RectTransform powerBar;

	// Use this for initialization
	void Start(){
		appleText = GameObject.Find ("appleCount").GetComponent<Text> ();
		//powerBar = GameObject.Find ("powerBar").GetComponent<RectTransform> ();
		appleCount = appleStart;
		localTime = PowerTime;
		isPower = false;
	}
	
	// Update is called once per frame
	void Update(){

		appleText.text = "" + appleCount;

		if (isPower == true) {
			localTime -= Time.deltaTime;
			powerBar.localScale = new Vector3(localTime/PowerTime,1f,1f);
		}

		if (localTime <= 0) {
			isPower = false;
			resetTime();
			powerBar.gameObject.GetComponent<Image>().enabled = false;
		}


	}

	public void takeApple(){
		appleCount--;
		isPower = true;
		powerBar.gameObject.GetComponent<Image>().enabled = true;
		powerBar.localScale = Vector3.one;
		resetTime ();
		Debug.Log ("ispower" + isPower);
	}

	public void resetTime(){
		localTime = PowerTime;
	}
}
