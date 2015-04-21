using UnityEngine;
using System.Collections;

public class KeyBehavior : MonoBehaviour {

	ParticleSystem psys;

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

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			psys.Emit (100);
			dieTimer = dieTimerMax;
			touched = true;
		}
	}
}
