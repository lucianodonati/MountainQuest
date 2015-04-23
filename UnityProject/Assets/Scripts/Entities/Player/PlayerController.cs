using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// calculated for rigidbody2d
	// mass 5
	// linear drag 0
	// angular drag 0.05
	// gravity scale 4

	// MOVEMENT VARS
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


	//SWORD VARS
	public GameObject Sword = null;
	bool swinging = false;
	bool halfswung = false;

	//SWITCH BOOL
	bool usingSword = false;

	//target for looking
	public Vector3 looktarget;

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
		Look (looktarget);

		Walk ();

		if (grounded) {
			jumpCooldownTimer -= Time.deltaTime;
		}

		if (Input.GetAxisRaw ("Vertical") > 0 && jumpCooldownTimer <= 0.0f && jumpTimer > 0.0f && !jumplock) {
			Jump ();
		}

		if(Input.GetAxisRaw("Vertical") == 0 && !grounded && !jumplock){
			jumplock = true;
		}

		this.transform.up = preserveUp;

		if(Input.GetKeyDown(KeyCode.Space)){
			usingSword = !usingSword;
			SwitchWeapon();
		}

		Attack ();
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

	void Look(Vector3 lookat){

		Vector3 targpos;

		if (lookat == Vector3.zero)
			targpos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		else
			targpos = lookat;

		if (!swinging) {
			if (targpos.x > transform.position.x)
				facingRight = true;
			else if (targpos.x < transform.position.x)
				facingRight = false;
		}

		if (usingSword) {
			if (facingRight) {
				transform.GetChild (0).position = Vector3.Lerp (transform.GetChild (0).position,
				                                                transform.GetChild (0).parent.position + new Vector3 (0.25f, 0, -0.5f),
				                                                (8 * Time.deltaTime));
			} else {
				transform.GetChild (0).position = Vector3.Lerp (transform.GetChild (0).position,
				                                                transform.GetChild (0).parent.position + new Vector3 (-0.25f, 0, -0.5f),
				                                                (8 * Time.deltaTime));
			}
		} else {
			targpos -= transform.position;
			targpos.z = -0.5f;

			transform.GetChild (0).position =
				Vector3.Lerp (transform.GetChild (0).position,
				              transform.GetChild (0).parent.position + (targpos.normalized * 0.25f),
				             (8 * Time.deltaTime));
		}
		
	}

	void Walk(){
		rigidbody2D.velocity = new Vector2 (Input.GetAxisRaw ("Horizontal") * movementSpeed, this.gameObject.rigidbody2D.velocity.y);
	}

	void Jump(){
		if(jumpTimer == jumpTimerMax && grounded){
			rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x,jumpSpeed * firstJumpModifier);
		}else if(jumpTimer < jumpTimerMax/5){
			rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpSpeed * firstJumpModifier/3);
			jumplock = true;
		}
		
		jumpTimer-=Time.deltaTime;
	}

	void SwitchWeapon(){
		if(usingSword){
			GameObject sord = (GameObject)Instantiate(Sword,transform.position,Quaternion.Euler(0,0,-45));
			sord.transform.parent = transform;
			sord.transform.position = sord.transform.parent.position + new Vector3(0.5f,0.5f,-1f);
		}else{
			Destroy(transform.GetChild(1).gameObject);
		}
	}

	void Attack(){
		if (!usingSword) {

			//FIRING ARROWS
			if (arrowCooldownTimer > 0.0f)
				arrowCooldownTimer -= Time.deltaTime;
			
			if (Input.GetMouseButton (0) && arrowCooldownTimer <= 0.0f) {
				
				Vector3 mousepos = GameObject.FindGameObjectWithTag ("MainCamera").camera.ScreenToWorldPoint (Input.mousePosition);
				mousepos -= transform.position;
				mousepos.z = 0;
				
				GameObject currArrow = (GameObject)Instantiate (Arrow,
				                                                gameObject.transform.position + Vector3.back,
				                                                Quaternion.FromToRotation (preserveUp, mousepos));
				
				currArrow.rigidbody2D.velocity = mousepos.normalized * 7.5f;
				
				arrowCooldownTimer = arrowCooldownTimerMax;
			}
		} else {

			//SWORD CODE
			if(!swinging){
				if(!facingRight && transform.GetChild(1).rotation != Quaternion.Euler(0,0,45)){
					transform.GetChild(1).rotation = Quaternion.Slerp(transform.GetChild(1).rotation,
					                                                  Quaternion.Euler(0,0,45),
					                                                  (8*Time.deltaTime));
					transform.GetChild(1).position = Vector3.Lerp(transform.GetChild(1).position,
					                                              transform.GetChild(1).parent.position + new Vector3(-0.5f,0.5f,-1f),
					                                              (8*Time.deltaTime));
				}else if(facingRight && transform.GetChild(1).rotation != Quaternion.Euler(0,0,-45)){
					transform.GetChild(1).rotation = Quaternion.Slerp(transform.GetChild(1).rotation,
					                                                  Quaternion.Euler(0,0,-45),
					                                                  (8*Time.deltaTime));
					transform.GetChild(1).position = Vector3.Lerp(transform.GetChild(1).position,
					                                              transform.GetChild(1).parent.position + new Vector3(0.5f,0.5f,-1f),
					                                              (8*Time.deltaTime));
				}
				
				if(Input.GetMouseButton(0) && !swinging){
					swinging = true;
				}
			}else{
				Quaternion toRot;
				Vector3 toPos;
				if(!halfswung){
					if(facingRight){
						toRot = Quaternion.Euler(0,0,-90);
						toPos = new Vector3(1f,-0.3f,-1f);
					}else{
						toRot = Quaternion.Euler(0,0,90);
						toPos = new Vector3(-1f,-0.3f,-1f);
						
					}
				}else{
					if(facingRight){
						toRot = Quaternion.Euler(0,0,0);
						toPos = new Vector3(0.5f,0.5f,-1f);
						
					}else{
						toRot = Quaternion.Euler(0,0,0);
						toPos = new Vector3(-0.5f,0.5f,-1f);
						
					}
				}
				
				transform.GetChild(1).rotation = Quaternion.Slerp(transform.GetChild(1).rotation,
				                                                  toRot,
				                                                  (16*Time.deltaTime));
				transform.GetChild(1).position = Vector3.Lerp(transform.GetChild(1).position,
				                                              transform.GetChild(1).parent.position + toPos,
				                                              (16*Time.deltaTime));
				
				if(Quaternion.Angle(transform.GetChild(1).rotation,toRot) < 0.1f && !halfswung){
					halfswung = true;
				}else if (Quaternion.Angle(transform.GetChild(1).rotation,toRot) < 0.1f && halfswung){
					halfswung = false;
					swinging = false;
				}
			}
		}
	}
}
