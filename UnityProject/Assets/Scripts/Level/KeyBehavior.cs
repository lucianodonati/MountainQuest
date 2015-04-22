using UnityEngine;
using System.Collections;

public class KeyBehavior : MonoBehaviour {

	public GameObject attachedDoor;

	ParticleSystem psys;
	ParticleSystem.Particle[] finalparticles;
	int numparticles;

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
			sprndr.color = new Color(sprndr.color.r,sprndr.color.g,sprndr.color.b,(dieTimer/dieTimerMax));

			if(dieTimer <= 0.0f)
				Destroy(gameObject);
		}
	}

	void LateUpdate(){
		if (touched) {
			numparticles = psys.GetParticles(finalparticles);
			
			if(dieTimer <= 2* dieTimerMax/3){
				for(int i = 0; i < numparticles; ++i){
					Vector3 to =
						(attachedDoor.transform.position - finalparticles[i].position).normalized * (dieTimerMax/dieTimer);

					finalparticles[i].velocity =
						new Vector3(finalparticles[i].velocity.x + to.x,
						            finalparticles[i].velocity.y + to.y,
						            psys.transform.position.z);
				}
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

			Destroy (gameObject.collider2D);
			Destroy (gameObject.rigidbody2D);

			psys.startSpeed = 10;
			psys.Emit (100);
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
	}
}
