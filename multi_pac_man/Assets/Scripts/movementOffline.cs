using UnityEngine;
using System.Collections;

public class movementOffline : MonoBehaviour {

	public float speed = 10f; 
	public Rigidbody2D player;
	public GameObject BirthPoint;
	public GameObject deathparticle;
	public float respawnDelay;


	public lifeManager lifemanager;
	public PowerEatCotroller eatPower;
	public staminaControlloer staminaBar;
	

	private Vector3 velocity;

	private float sprint = 2;
	private Vector3 graveLocation = new Vector3(-20f,-20f,0f);

	// Use this for initialization
	void Start () {
	
		lifemanager = FindObjectOfType<lifeManager> ();
		eatPower = FindObjectOfType<PowerEatCotroller> ();
		staminaBar = FindObjectOfType<staminaControlloer> ();
		//crazyPacSign = GameObject.Find ("PowerEaten_canvas");
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		velocity = Vector3.zero;//reset velocity 
		inputMove ();
		updateAni ();
		if(valid((Vector2)velocity))
			player.velocity = (Vector2)velocity;
	}

	void inputMove(){
		// Check for Input if not moving, how to solve the smooth move and collision
		// when hit the conner rotation appear
		
		if(Input.GetKey(KeyCode.UpArrow))
			velocity = Vector3.up * speed;
		if(Input.GetKey(KeyCode.DownArrow))
			velocity = Vector3.down * speed;
		if(Input.GetKey(KeyCode.LeftArrow))
			velocity = Vector3.left * speed;
		if(Input.GetKey(KeyCode.RightArrow))
			velocity = Vector3.right * speed;
		/*	if (Input.GetKey (KeyCode.Space))//sprint
			velocity*=sprint;*/
		if (Input.GetButton ("Jump")) { 
			//GetComponent<PhotonView> ().RPC ("syncStaminaBar", PhotonTargets.All, new object[]{ true });
			staminaBar.isrun = true;
			if (staminaBar.stopRun == false)
				velocity *= sprint; 
		} else {
			staminaBar.isrun = false;
			//GetComponent<PhotonView> ().RPC ("syncStaminaBar", PhotonTargets.All,  new object[]{ false });
		}
	}

	void updateAni(){
		//animation 
		//when |dir| <= 0.1 no animation
		//Vector2 dir = dest - (Vector2)player.position;
		Vector2 dir = player.velocity;
		// make date more pure
		if (Mathf.Abs (dir.x) > Mathf.Abs (dir.y))
			dir.y = 0f;
		if (Mathf.Abs (dir.x) < Mathf.Abs (dir.y))
			dir.x = 0f;
		
		GetComponent<Animator> ().SetFloat ("DirX", dir.x);
		GetComponent<Animator> ().SetFloat ("DirY", dir.y);
		
	}

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		// avoid collision
		// this raycast is too simple!!!
		// only care before move, what about on moving??
		Vector2 dir_sub_1 = dir.normalized, dir_sub_2 = dir.normalized;//with more raycast, much better
		float offset = 0.7f;
		
		if (dir.x == 0) {
			dir_sub_1.x = offset;
			dir_sub_2.x = -offset;
		}
		if(dir.y == 0){
			dir_sub_1.y = offset;
			dir_sub_2.y = -offset;
		}
		
		Vector2 pos = player.position;
		//Debug.DrawLine (pos + dir.normalized, pos, Color.red);
		//Debug.DrawLine (pos + dir_sub_1, pos, Color.red);
		//Debug.DrawLine (pos + dir_sub_2, pos, Color.red);
		RaycastHit2D hit = Physics2D.Linecast(pos + dir.normalized, pos);// set the lincast little longer
		RaycastHit2D hit_sub_1 = Physics2D.Linecast(pos + dir_sub_1, pos);
		RaycastHit2D hit_sub_2 = Physics2D.Linecast(pos + dir_sub_2, pos);
		/*if (hit.collider== GetComponent<CircleCollider2D> ())// || hit.collider.attachedRigidbody == GetComponent<Rigidbody2D> ())
			return true;
		else 
			return false;
		*/
		return( !(hit.collider.tag == "wall") && !(hit_sub_1.collider.tag == "wall") && !(hit_sub_2.collider.tag == "wall"));
		
	}

	void OnCollisionEnter2D(Collision2D Coll)//collision with ghost, synDelay will miss this collision
	{
		if (eatPower.isPower == false) {
			if (Coll.gameObject.tag == "ghost_online" || Coll.gameObject.tag == "ghost") {
				Debug.Log ("I hit ghost");
				Debug.Log ("apple count ? "+ eatPower.appleCount);
				Debug.Log ("is power ? "+ eatPower.isPower);
				Respawn();//it's better to move back position, but destroy
			
			}
		} else {
			if (Coll.gameObject.tag == "pac_1") {
				Debug.Log ("I hit crazyPac");
				Respawn();//it's better to move back position, but destroy

			}
		}
	}
	//respawn player

	public void Respawn(){
		StartCoroutine ("RespawnCo");
	}
	
	public IEnumerator RespawnCo(){//when reset position, collider is still there, how to hide it 
		Instantiate(deathparticle,player.position,Quaternion.identity);//particle effect
		if (gameObject.tag == "pac_1")
			lifemanager.takeLife();
		player.GetComponent<Renderer> ().enabled = false;
		moveToPosition(graveLocation);
		yield return new WaitForSeconds (respawnDelay);
		moveToPosition (BirthPoint.transform.position);
		Debug.Log ("Respawn player");
		player.GetComponent<Renderer> ().enabled = true;
		
	}
	
	public void moveToPosition(Vector3 dest){
		player.position = dest;

	}

}
