﻿using UnityEngine;
using System.Collections;

public class BoostSphere : MonoBehaviour {
	
	public float DamageModifier = 10;
	public float VelocityModifier = 1.5f;
	public float AliveTimer = 7;
	public bool isAiming = false;
	public GameObject Sphere;
	public Player Owner;
	public AudioClip boostSound;
	
	void Start ()
	{
	
	}
	
	void Update ()
	{

		AliveTimer -= Time.deltaTime;
		if (AliveTimer <= 0) {
			Destroy (this.gameObject); 		
			if(Owner!=null && Owner.GetComponent<Player>() != null)
				Owner.GetComponent<Player>().RemoveBSphere();
			
		}
		
		if (AliveTimer<=2) {
			SpriteRenderer mySR = GetComponent<SpriteRenderer>();
			
			mySR.color = new Color(1,1,1,AliveTimer/2);
		}
		
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		//print("HEEEEEEYY");
		Arrow proj = other.GetComponent<Arrow> ();
		if (proj != null) {
			proj.Damage += DamageModifier;
			
		

		}
		if (other.rigidbody2D!=null) {
			if (other.rigidbody2D.velocity.magnitude < 32){
				other.rigidbody2D.velocity *= VelocityModifier;
				if(other.rigidbody2D.velocity.magnitude > 32){
					other.rigidbody2D.velocity = other.rigidbody2D.velocity.normalized * 32;
				}
			}
		}
		
	}
	
	
	public void SetOwner(Player owner)
	{
		Owner = owner;
		
	}
	
}