using UnityEngine;
using System.Collections;

public class Patrol_Movement : MonoBehaviour {

	public bool direction;
	public float moveSpeed = 4;
	
	private Vector3 preserveUp;
	
	public GameObject ground;

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
		if (ground != null) {
			Vector2 movevec = new Vector2(moveSpeed,0);
			
			if(!direction)
				movevec *= -1;
			
			rigidbody2D.velocity = movevec;

			if(collider2D.bounds.min.x + (rigidbody2D.velocity.x * Time.deltaTime) < ground.collider2D.bounds.min.x ||
			   collider2D.bounds.max.x + (rigidbody2D.velocity.x * Time.deltaTime) > ground.collider2D.bounds.max.x)
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
