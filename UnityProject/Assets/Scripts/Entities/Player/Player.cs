using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : Entity {

	public List<Sphere> Spheres;
	public List<Arrow> Arrows;
	public List<Sword> Swords;

	Sword ActiveSword;
	Arrow ActiveArrow;
	Sphere ActiveSphere;

	private Vector3 spawnpos;

	public int lives = 3;

	// Use this for initialization
	void Start () {
		spawnpos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public override void die(){
		if (lives <= 0) {
			health.currentHP = health.maxHP;
			transform.position = spawnpos;
			lives--;
		} else {
			//Recode when proper game over has been made
			Application.LoadLevel("MainMenu");
		}
	}
}
