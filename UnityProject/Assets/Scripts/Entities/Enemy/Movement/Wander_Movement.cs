using UnityEngine;
using System.Collections;

public class Wander_Movement : MonoBehaviour {

	public bool direction;
	public float moveSpeed;

	private Vector3 preserveUp;

	bool grounded;

	// Use this for initialization
	void Start () {

		RandomizeDirection ();

		preserveUp = this.transform.up;
	}
	
	// Update is called once per frame
	void Update () {

		if (grounded) {
			Vector2 movevec = new Vector2(moveSpeed,0);

			if(!direction)
				movevec *= -1;

			rigidbody2D.velocity = movevec;
		}

		this.transform.up = preserveUp;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform" && !grounded) {
			grounded = true;
			RandomizeDirection();
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform") {
			grounded = false;
				rigidbody2D.velocity = Vector2.zero;
		}
	}

	void RandomizeDirection(){
		if (Random.Range (1, 6) > 3)
			direction = true; //right
		else
			direction = false; //left
	}
}
