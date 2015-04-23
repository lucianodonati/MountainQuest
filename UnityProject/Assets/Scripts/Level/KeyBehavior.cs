using UnityEngine;
using System.Collections;

public class KeyBehavior : MonoBehaviour {

	/*
	 * PARTICLE SYSTEM SPECIFICATIONS:
	 * 
	 * ---Basic---
	 * Duration:				5.00
	 * Looping:					True
	 * Prewarm:					False
	 * Start Delay:				0
	 * Start Lifetime:			5
	 * Start Speed:				5
	 * Start Size:				0.3
	 * Start Rotation:			0
	 * Start Color:				R:255, G:255, B:255
	 * Gravity Multiplier:		0
	 * Inherent Velocity:		0
	 * Play On Awake:			True
	 * Max Particles:			1000
	 * --------------------------------------------
	 * 
	 * ---Emission---
	 * Rate:					1
	 * Bursts:					N/A
	 * ----------------------------
	 * 
	 * ---Shape---
	 * Shape:					Cone
	 * Angle:					0.8094972
	 * Radius:					0.1078041
	 * Length:					5
	 * Emit from:				Base
	 * Random Direction:		False
	 * ----------------------------------
	 * 
	 * ---Color Over Lifetime---
	 * Color:					Gradient
	 * 							Color 1:
	 * 								Location: 0%
	 * 								R:255, G:255, B:0
	 * 							Color 2:
	 * 								Location:100.0%
	 * 								R:255, G:255, B:255
	 * ------------------------------------------------
	 * 
	 * ---Size Over Lifetime---
	 * Size:					Curve
	 * 							Node 1:
	 * 								0.000, 0.00
	 * 							Node 2:
	 * 								0.366, 1.00
	 * 							Node 3:
	 * 								1.000, 0.00
	 * ----------------------------------------
	 * 
	 * ---Renderer---
	 * Render Mode:				Billboard
	 * Normal Direction:		1
	 * Material:				Sprites-Default
	 * Sort Mode:				None
	 * Sorting Fudge:			0
	 * Cast Shadows:			True
	 * Recieve Shadows:			True
	 * Max Particle Size:		0.5
	 * ----------------------------------------
	 */

	public GameObject attachedDoor;

	ParticleSystem psys;
	ParticleSystem.Particle[] finalparticles;
	int numparticles;

	public int finalBurst;

	bool touched = false;

	float dieTimer;
	public float dieTimerMax = 5f;

	// Use this for initialization
	void Start () {
		psys = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

		if (touched) {
			dieTimer -= Time.deltaTime;
			SpriteRenderer sprndr = gameObject.GetComponent<SpriteRenderer>();
			sprndr.color = new Color(sprndr.color.r,
			                         sprndr.color.g,
			                         sprndr.color.b,
			                         Mathf.SmoothStep(sprndr.color.a,0,1));

			if(dieTimer <= 0.0f)
				Destroy(gameObject);
		}
	}

	void LateUpdate(){
		if (touched) {
			numparticles = psys.GetParticles(finalparticles);
			
			if(dieTimer <= 2* dieTimerMax/3){
				Gather ();
			}else{
				for (int i = 0; i < numparticles; ++i) {
					finalparticles [i].velocity =
						new Vector3(Mathf.SmoothStep(finalparticles[i].velocity.x,0,Time.deltaTime),
						            Mathf.SmoothStep(finalparticles[i].velocity.y,0,Time.deltaTime),
						            psys.transform.position.z);
				}
			}

			psys.SetParticles(finalparticles,numparticles);
			
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player" && !touched) {
			CollectKey();
		}
	}

	void CollectKey(){
		
		Destroy (gameObject.collider2D);
		Destroy (gameObject.rigidbody2D);
		
		psys.startSpeed = 10;
		psys.Emit (finalBurst);
		psys.simulationSpace = ParticleSystemSimulationSpace.World;
		
		dieTimer = dieTimerMax;
		touched = true;
		
		finalparticles = new ParticleSystem.Particle[psys.maxParticles];
		
		numparticles = psys.GetParticles (finalparticles);
		
		for (int i = 0; i < numparticles; ++i) {
			
			int modif = Random.Range (10, 20);
			
			finalparticles [i].velocity = 
				new Vector3 (finalparticles [i].velocity.x * modif, finalparticles [i].velocity.y * modif, finalparticles [i].velocity.z);
		}
		psys.SetParticles (finalparticles, numparticles);
	}

	void Gather(){
		for(int i = 0; i < numparticles; ++i){
			Vector3 to =
				(attachedDoor.transform.position - finalparticles[i].position);
			
			to.z = 0;
			
			if(to.magnitude > 1){
				to = to.normalized * (dieTimerMax/dieTimer); 
				
				finalparticles[i].velocity =
					new Vector3(finalparticles[i].velocity.x + to.x,
					            finalparticles[i].velocity.y + to.y,
					            psys.transform.position.z);
			}else{
				finalparticles[i].position = attachedDoor.transform.position;
				finalparticles[i].velocity = Vector3.zero;
			}
		}
	}
}
