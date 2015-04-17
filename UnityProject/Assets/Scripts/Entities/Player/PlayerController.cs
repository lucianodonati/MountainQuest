using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// MOVEMENT VARS
	public float movementSpeed = 2;

	public float jumpSpeed = 0.2f;
	private float jumpTimer;
	public float jumpTimerMax = 0.1f;
	public float firstJumpModifier = 10.0f;

	public bool grounded = false;

	public bool facingRight = true;

	private float jumpCooldownTimer;
	public float jumpCooldownTimerMax = 0.1f;

	private Vector2 preserveUp;


	//ARROW VARS
	public GameObject Arrow = null;

	private float arrowCooldownTimer;
	public float arrowCooldownTimerMax = 0.5f;

	// Use this for initialization
	void Start () {
		preserveUp = this.transform.up;

		jumpTimer = jumpTimerMax;
		jumpCooldownTimer = jumpCooldownTimerMax;
		arrowCooldownTimer = arrowCooldownTimerMax;
	}
	
	// Update is called once per frame
	void Update () {

		//MOVEMENT
		float xMove = Input.GetAxisRaw ("Horizontal");

		if (xMove > 0)
			facingRight = true;
		else if (xMove < 0)
			facingRight = false;

		this.gameObject.rigidbody2D.velocity = new Vector2 (xMove * movementSpeed, this.gameObject.rigidbody2D.velocity.y);

		if (grounded) {
			jumpCooldownTimer -= Time.deltaTime;
		}

		if (Input.GetAxisRaw ("Vertical") > 0 && jumpCooldownTimer <= 0.0f && jumpTimer > 0.0f) {
			if(jumpTimer == jumpTimerMax && grounded)
				rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x,jumpSpeed * firstJumpModifier);
			else
				rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpSpeed);

			jumpTimer-=Time.deltaTime;
		}

		this.transform.up = preserveUp;


		//FIRING ARROWS
		if (arrowCooldownTimer > 0.0f)
			arrowCooldownTimer -= Time.deltaTime;

		if (Input.GetMouseButton (0) && arrowCooldownTimer <= 0.0f) {

			Vector3 mousepos = GameObject.FindGameObjectWithTag("MainCamera").camera.ScreenToWorldPoint(Input.mousePosition);
			mousepos -= transform.position;
			mousepos.z = 0;

			GameObject currArrow = (GameObject)Instantiate(Arrow,gameObject.transform.position,Quaternion.FromToRotation(preserveUp,mousepos));

			currArrow.rigidbody2D.velocity = mousepos.normalized * 2.0f;

			arrowCooldownTimer = arrowCooldownTimerMax;
		}

	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform") {
			grounded = true;
			jumpCooldownTimer = jumpCooldownTimerMax;
		}
	}

	void OnCollisionStay2D(Collision2D coll){
		if (coll.gameObject.tag == "Platform") {
			grounded = true;
			jumpTimer = jumpTimerMax;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		grounded = false;
	}
}
