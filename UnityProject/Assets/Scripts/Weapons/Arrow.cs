using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {
	public float speed;
    public float stuckTimer = 5;
    private bool stuck = false;
    public int numCollisions = 0;

	public float Damage = 10;

	// Use this for initialization
	void Start () {
        rigidbody2D.velocity = new Vector2(1, 0) * speed;
	}
	
	// Update is called once per frame
	void Update () {
        if (stuck)
        {
            stuckTimer -= Time.deltaTime;

            if (stuckTimer <= 0)
                Destroy(this.gameObject);
        }

        rigidbody2D.position += rigidbody2D.velocity * Time.deltaTime;
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag != "Sphere" && coll.gameObject.tag != "RangedWeapon")
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            stuck = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Sphere" && other.gameObject.tag != "RangedWeapon")
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            stuck = true;
        }
    }
}
