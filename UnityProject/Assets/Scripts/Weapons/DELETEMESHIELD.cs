using UnityEngine;
using System.Collections;

public class DELETEMESHIELD : MonoBehaviour {

    public Transform owner;
    public float pushForce;

    private float lifetime;
    public float lifetimeMax;

	// Use this for initialization
	void Start () {
        lifetime = lifetimeMax;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(owner.position.x, owner.position.y, transform.position.z);

        lifetime -= Time.deltaTime;

        SpriteRenderer spr = GetComponent<SpriteRenderer>();

        if (lifetime > 0.0f)
            spr.color = new Color(spr.color.r, spr.color.g, spr.color.b, 1.0f * (lifetime / lifetimeMax));
        else
            Destroy(gameObject);
	}

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.GetComponent<Arrow>() != null)
        {
            if (coll.GetComponent<Arrow>().owner != this.gameObject)
            {
                coll.rigidbody2D.velocity += ((Vector2)(coll.transform.position - transform.position)).normalized * pushForce;
                coll.transform.up = coll.rigidbody2D.velocity.normalized;
            }
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.GetComponent<Arrow>() != null)
        {
            if (coll.GetComponent<Arrow>().owner != this.gameObject)
                coll.GetComponent<Arrow>().owner = this.gameObject;
        }
    }
}
