using UnityEngine;
using System.Collections;

public class RedirectSphere : MonoBehaviour
{
	
	public float RotationDirection;
	public Vector3 Direction;
	public float DamageModifier = 5;
	public float AliveTimer = 7;
	public Player Owner;
	
	void Start ()
	{
		Direction = new Vector3 (0, 1, 0);
	}
	
	void Update ()
	{
		
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, RotationDirection));
		
		AliveTimer -= Time.deltaTime;
		if (AliveTimer <= 0) {
			Destroy (this.gameObject);
			if (Owner.GetComponent<Player> ())
				Owner.GetComponent<Player> ().RemoveRSphere ();
			
		}
		if (AliveTimer <= 2) {
			SpriteRenderer mySR = GetComponent<SpriteRenderer> ();
			mySR.color = new Color (1, 1, 1, AliveTimer / 2);
		}
		
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		Arrow proj = other.GetComponent<Arrow> ();
		
		if (proj != null) {
			other.rigidbody2D.position = this.transform.position;
			Direction.Normalize ();
			Direction *= other.rigidbody2D.velocity.magnitude;
			other.rigidbody2D.velocity = Direction;
			other.rigidbody2D.rotation = RotationDirection + 0;
			
			proj.Damage += DamageModifier;
			
			proj.particleSystem.Play();
			

		}
		
	}
	
	public void ChangeDirection (float rdir, Vector3 dir, float timer)
	{
		Direction = dir;
		RotationDirection = rdir;
		AliveTimer = timer;
	}
	
	public void SetOwner (Player owner)
	{
		Owner = owner;
	}
	
}

