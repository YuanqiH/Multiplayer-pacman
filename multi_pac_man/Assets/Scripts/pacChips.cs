using UnityEngine;
using System.Collections;


public class pacChips : MonoBehaviour {


	public int pointToAdd;
	public GameObject crazyPacSign;
	public PowerEatCotroller powerEAT;

	//private GameObject[] ghosts;

	void Start(){
		powerEAT = FindObjectOfType<PowerEatCotroller> ();
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<Rigidbody2D> () == null)
			return;

		if (other.tag == "pac_1") {
			ScoreManager.addPoints (pointToAdd, "pac_1");
			if (gameObject.tag == "apple") {
				//showEatenSign();
				powerEAT.takeApple();
			}
			Destroy (gameObject);
		}


		//Debug.Log("Dot has been eaten");

		if (other.tag == "pac_2") {
			ScoreManager.addPoints (pointToAdd, "pac_2");
			Destroy (gameObject);
		}

	}

	public void showEatenSign(){
		crazyPacSign.SetActive (true);
	}



}


