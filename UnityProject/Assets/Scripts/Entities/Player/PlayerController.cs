using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// MOVEMENT VARS
	//calculated for rigidbody2d
	// mass 5
	// linear drag 0
	// angular drag 0.05
	// gravity scale 4

	public float movementSpeed = 10;

	public float jumpSpeed = 1f;
	private float jumpTimer;
	public float jumpTimerMax = 0.1f;
	public float firstJumpModifier = 20.0f;
	bool jumplock = false;

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

		if (Input.GetAxisRaw ("Vertical") > 0 && jumpCooldownTimer <= 0.0f && jumpTimer > 0.0f && !jumplock) {
			if(jumpTimer == jumpTimerMax && grounded)
				rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x,jumpSpeed * firstJumpModifier);
			else
				rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpSpeed);

			jumpTimer-=Time.deltaTime;
		}

		if(Input.GetAxisRaw("Vertical") == 0 && !grounded && !jumplock){
			jumplock = true;
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

			currArrow.rigidbody2D.velocity = mousepos.normalized * 7.5f;

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
			jumplock = false;
			jumpTimer = jumpTimerMax;
		}
	}

	void OnCollisionExit2D(Collision2D coll){
		grounded = false;
	}
}
