using UnityEngine;
using System.Collections;

public class navi : MonoBehaviour {

	public GameObject target;
	public float chaseSpeed;
	public Rigidbody2D player;
	public GameObject deathparticle;
	private Vector3 direction;
	private Vector3 dir;
	public float respawnDelay;

	public GameObject BirthPoint;
	private Vector3 graveLocation = new Vector3(-20f,-20f,0f);

	public lifeManager lifemanager;
	public PowerEatCotroller eatPower;
	public staminaControlloer staminaBar;

	// Use this for initialization
	void Start () {
		lifemanager = FindObjectOfType<lifeManager> ();
		eatPower = FindObjectOfType<PowerEatCotroller> ();
		staminaBar = FindObjectOfType<staminaControlloer> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		caculateDir ();
		updateAni ();
		if(valid((Vector2)dir))
			player.velocity = (Vector2)dir * chaseSpeed;

		GetComponent<Animator> ().SetBool ("isPower", eatPower.isPower);
	}

	void caculateDir(){
		direction = target.transform.position - gameObject.transform.position;
		if (Mathf.Abs (direction.x) >= Mathf.Abs (direction.y)) {
			if (direction.x > 0)
				dir = Vector3.right;
			if (direction.x < 0)
				dir = Vector3.left;
			if(!valid((Vector2)dir)){
				if(valid(Vector2.down))
					dir = Vector3.down;
				if(valid(Vector2.up))
					dir = Vector3.up;
			}

		} else {
			if (direction.y > 0)
				dir = Vector3.up;
			if (direction.y < 0)
				dir = Vector3.down;
			if(!valid((Vector2)dir)){
				if(valid(Vector2.right))
					dir = Vector3.right;
				if(valid(Vector2.left))
					dir = Vector3.left;
			}
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
