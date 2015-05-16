using UnityEngine;
using System.Collections;

public class CrumblingPlatformBehavior : MonoBehaviour {

	private float lifetime;
	public float lifetimeMax = 2f;
	private Vector2 vibdir;
    private Vector3 posSave;
    ParticleSystem psys;
    Color colorSave;

	public bool dead = false;
	bool deathrow = false;

    float refreshTimer;
    public float refreshTimerMax = 3.0f;

	// Use this for initialization
	void Start () {
        psys = gameObject.GetComponent<ParticleSystem>();
		lifetime = lifetimeMax;
		vibdir = new Vector2 (1,0);
        posSave = transform.position;
        colorSave = GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {

		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer> ();

		renderer.color = new Color (renderer.color.r, renderer.color.g * (lifetime/lifetimeMax), renderer.color.b * (lifetime/lifetimeMax));

		if (!dead) {
			if (lifetime > lifetimeMax / 7) {
				vibdir = 0.1f * vibdir.normalized * (lifetimeMax / lifetime) * 3;
				psys.emissionRate = 1 * (lifetimeMax / lifetime);
			}

			if(deathrow){
				lifetime -= Time.deltaTime;

				if (lifetime < lifetimeMax) {
                    transform.position = posSave + (Vector3)vibdir;
					vibdir *= -1;
				}
				
				if (lifetime <= 0.0f) {
					psys.startSpeed = 0;
                    transform.position = posSave;
					rigidbody2D.velocity = new Vector2 (0, 0);
					rigidbody2D.isKinematic = false;
					rigidbody2D.gravityScale = 1;
					dead = true;
				}
			}
		}
        else
        {
            if (refreshTimer > 0.0f && !renderer.enabled)
                refreshTimer -= Time.deltaTime;
            else if (refreshTimer <= 0.0f && !renderer.enabled)
            {
                renderer.enabled = true;
                psys.renderer.enabled = true;
                lifetime = lifetimeMax;
                GetComponent<Collider2D>().isTrigger = false;
                dead = false;
                deathrow = false;
                GetComponent<SpriteRenderer>().color = colorSave;
            }
        }
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (rigidbody2D.isKinematic == false && 
		    !(coll.gameObject.tag == "Player" && coll.gameObject.transform.position.y > transform.position.y))
        {
			//Destroy (this.gameObject);
            rigidbody2D.isKinematic = true;
            renderer.enabled = false;
            psys.renderer.enabled = false;
            vibdir = new Vector2(1, 0);
            GetComponent<Collider2D>().isTrigger = true;
            transform.position = posSave;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            refreshTimer = refreshTimerMax;
            psys.emissionRate = 0;

		} else if (coll.gameObject.tag == "Player" && coll.gameObject.transform.position.y > transform.position.y)
        {
			Crumble ();
		}
	}

	void Crumble(){
		deathrow = true;
	}
}
