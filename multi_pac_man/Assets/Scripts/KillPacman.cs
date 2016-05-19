using UnityEngine;
using System.Collections;

public class KillPacman : MonoBehaviour {
	//public NetworkManager networkManager;
	public float respawnDelay;
	public GameObject prefab;
	public GameObject BirthPoint;
	//public Movement movement;
	// Use this for initialization
	void Start () {
		//networkManager = FindObjectOfType<NetworkManager> ();
		//movement = FindObjectOfType<Movement> ();
	}
	
	void OnCollisionEnter2D(Collision2D Coll)//collision with ghost
	{
		if (Coll.gameObject.tag == "pac_1"||Coll.gameObject.tag == "pac_2") {
			Debug.Log ("I kill pacman");
			Destroy(Coll.gameObject);
			//networkManager.Respawn();
			Respawn();
			//player.position.Set(Mathf.Ceil(player.position.x),Mathf.Ceil(player.position.y));
			//dest = player.position;
		}
	}

	public void Respawn(){
		StartCoroutine ("RespawnCo");
	}
	
	public IEnumerator RespawnCo(){
		
		//player.GetComponent<Renderer> ().enabled = false;
		yield return new WaitForSeconds (respawnDelay);
		Debug.Log ("Respawn pac_1");
		PhotonNetwork.Instantiate (prefab.name, BirthPoint.transform.position, Quaternion.identity, 0);
		
	}

}
