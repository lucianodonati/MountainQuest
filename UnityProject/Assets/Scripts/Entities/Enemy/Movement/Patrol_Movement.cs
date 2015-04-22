using UnityEngine;
using System.Collections;

public class Patrol_Movement : MonoBehaviour {

	public bool direction;
	public float moveSpeed = 4;
	
	private Vector3 preserveUp;
	
	public GameObject ground;

	//if true, does old patrol
	//if false, requires two points to move between
	public bool LimitToPlatform = true;

	public GameObject leftBound, rightBound;

	// Use this for initialization
	void Start () {

		if (Random.Range (1, 6) > 3)
			direction = true; //right
		else
			direction = false; //left

		preserveUp = this.transform.up;
	}
	
	// Update is called once per frame
	void Update () {

		Vector2 movevec = new Vector2 (moveSpeed, 0);
			
		if (!direction)
			movevec *= -1;
			
		rigidbody2D.velocity = new Vector2(movevec.x,rigidbody2D.velocity.y);

		if (LimitToPlatform && ground != null) {
			if (collider2D.bounds.min.x + (rigidbody2D.velocity.x * (2 * Time.deltaTime)) < ground.collider2D.bounds.min.x ||
				collider2D.bounds.max.x + (rigidbody2D.velocity.x * (2 * Time.deltaTime)) > ground.collider2D.bounds.max.x)
				direction = !direction;
		} else {
			if (collider2D.bounds.min.x + (rigidbody2D.velocity.x * (2 * Time.deltaTime)) < leftBound.transform.position.x ||
				collider2D.bounds.max.x + (rigidbody2D.velocity.x * (2 * Time.deltaTime)) > rightBound.transform.position.x)
				direction = !direction;
		}
		
		
		this.transform.up = preserveUp;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform" && ground == null) {
			ground = coll.gameObject;
		}
	}

	void OnCollisionStay2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform" && ground == null) {
			ground = coll.gameObject;
		}
	}
	
	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform") {
			ground = null;
		}
	}
}
