using UnityEngine;
using System.Collections;

public class CrumblingPlatformBehavior : MonoBehaviour {

	private float lifetime;
	public float lifetimeMax = 2f;
	private Vector2 vibdir;

	bool dead = false;
	bool deathrow = false;

	// Use this for initialization
	void Start () {
		lifetime = lifetimeMax;
		vibdir = new Vector2 (1,0);
	}
	
	// Update is called once per frame
	void Update () {

		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer> ();
		ParticleSystem psys = gameObject.GetComponent<ParticleSystem> ();

		renderer.color = new Color (renderer.color.r, renderer.color.g * (lifetime/lifetimeMax), renderer.color.b * (lifetime/lifetimeMax));

		if (!dead) {

			if (lifetime > lifetimeMax / 7) {
				vibdir = vibdir.normalized * (lifetimeMax / lifetime) * 3;
				psys.emissionRate = 1 * (lifetimeMax / lifetime);
			}

			if(deathrow){
				lifetime -= Time.deltaTime;

				if (lifetime < lifetimeMax) {
					rigidbody2D.velocity = vibdir;
					vibdir *= -1;
				}
				
				if (lifetime <= 0.0f) {
					psys.startSpeed = 0;
					rigidbody2D.velocity = new Vector2 (0, 0);
					rigidbody2D.isKinematic = false;
					rigidbody2D.gravityScale = 1;
					dead = true;
				}
			}
		}
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (rigidbody2D.isKinematic == false) {
			Destroy (this.gameObject);
		} else if (coll.gameObject.tag == "Player" && coll.gameObject.transform.position.y > transform.position.y) {
			deathrow = true;
		}
	}	
}
