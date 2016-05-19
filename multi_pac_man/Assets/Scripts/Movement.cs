using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Movement : Photon.MonoBehaviour {

	public NetworkManager networkManager;
	public float speed = 10f; 
	public Rigidbody2D player;
	public GameObject BirthPoint;
	public GameObject deathparticle;
	public float respawnDelay;
	public bool localTest;
	public float ration;
	public float farRange;
	public float nearRange;
	public float timeOffset;
	public float waitforRemoteDelay;

	public lifeManager lifemanager;
	public PowerEatCotroller eatPower;
	public Sprite sickGhost;
	public staminaControlloer staminaBar;
	private bool localIsRun;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition ;
	private Vector3 syncEndPosition;
	private Vector3 velocity;
	private Vector3 syncVelocity;
	private float sprint = 2;
	private Vector3 graveLocation = new Vector3(-20f,-20f,0f);
	void Start() {
		//dest = transform.position;
		//player = GetComponent<Rigidbody2D> ();
		syncEndPosition = BirthPoint.transform.position;
		syncStartPosition = BirthPoint.transform.position;
		lifemanager = FindObjectOfType<lifeManager> ();
		eatPower = FindObjectOfType<PowerEatCotroller> ();
		staminaBar = FindObjectOfType<staminaControlloer> ();
		//crazyPacSign = GameObject.Find ("PowerEaten_canvas");
	}
	
	void FixedUpdate() {

		velocity = Vector3.zero;//reset velocity 
		//syncVelocity = Vector3.zero;
		if (localTest) {
			// for local test
			inputMove ();
			updateAni ();
		} else {
			if (photonView.isMine) { //make sure the control client own spawn
				inputMove ();
				updateAni ();
				if(valid((Vector2)velocity))
					player.velocity = (Vector2)velocity;
			} else {
				synedMove ();
				synedAni ();

			}
		}

			
			if (gameObject.tag == "ghost_online") {
			//gameObject.GetComponent<SpriteRenderer>().sprite = sickGhost;
			GetComponent<Animator> ().SetBool ("isPower", eatPower.isPower);
		}

		//staminaBar.isrun = localIsRun;
	}

	void synedAni(){
		Vector2 dir = syncEndPosition - syncStartPosition;

		if (Mathf.Abs (dir.x) > Mathf.Abs (dir.y))
			dir.y = 0f;
		if (Mathf.Abs (dir.x) < Mathf.Abs (dir.y))
			dir.x = 0f;

		GetComponent<Animator> ().SetFloat ("DirX", dir.x);
		GetComponent<Animator> ().SetFloat ("DirY", dir.y);	

		}


	void synedMove(){
		/*
		syncTime += Time.deltaTime;
		//Vector3 dest = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
		Vector3 dest = Vector3.MoveTowards (syncStartPosition, syncEndPosition, speed);
		if(valid((Vector2)(dest-syncStartPosition)))
			player.MovePosition(dest);
			*/
		Vector3.SmoothDamp(syncStartPosition, syncEndPosition,ref syncVelocity, syncDelay,ration * speed,Time.deltaTime);
		if (Vector3.Distance (player.position, syncEndPosition) < nearRange)
			syncVelocity = Vector3.zero;
		if (Vector3.Distance (player.position, syncEndPosition) > farRange)
			player.position = syncEndPosition;
		if(valid((Vector2)syncVelocity))
			player.velocity = syncVelocity;

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

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			Vector3 pos = player.position;
			//stream.SendNext(ref pos);
			stream.Serialize (ref pos);
			stream.Serialize (ref velocity);
		} else {
			Vector3 pos = new Vector3 (14, 14, 0);
			Vector3 velo = Vector3.zero ;
			//transform.position = (Vector3)stream.ReceiveNext ();
			stream.Serialize(ref pos);
			stream.Serialize(ref velo); //try to do prediction
			syncStartPosition = player.position;
			//syncTime = 0.0f; //star time up
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			Vector3 dir = (pos - syncStartPosition).normalized;
			syncEndPosition = pos + dir * Time.fixedDeltaTime *speed* timeOffset;// //predictinposition


		}
	}

	void OnCollisionEnter2D(Collision2D Coll)//collision with ghost, synDelay will miss this collision
	{
		if (eatPower.isPower == false) {
			if (Coll.gameObject.tag == "ghost_online" || Coll.gameObject.tag == "ghost_local") {
				Debug.Log ("I hit ghost");
				Debug.Log ("apple count ? "+ eatPower.appleCount);
				Debug.Log ("is power ? "+ eatPower.isPower);
				//Respawn();//it's better to move back position, but destroy
				GetComponent<PhotonView> ().RPC ("Respawn", PhotonTargets.All, null);
			}
		} else {
			if (Coll.gameObject.tag == "pac_1") {
				Debug.Log ("I hit crazyPac");
				//Respawn();//it's better to move back position, but destroy
				GetComponent<PhotonView> ().RPC ("Respawn", PhotonTargets.All, null);
			}
		}
	}
	//respawn player
	[PunRPC]
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
		syncEndPosition = dest;
		syncStartPosition = dest;
	}
	/* this is not a good way to handle stamina bar, no distinguish
	[PunRPC]
	public void syncStaminaBar(bool newValue){
		///if(gameObject.tag == "pac_1" || gameObject.tag == "pac_2")
		//	localIsRun = newValue;
	}
	*/

}
