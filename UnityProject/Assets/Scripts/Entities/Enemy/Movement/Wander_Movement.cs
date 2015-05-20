using UnityEngine;
using System.Collections;

public class Wander_Movement : Enemy_Movement {

	//public bool direction;
	public float moveSpeed = 4;

	//private Vector3 preserveUp;

    //public bool grounded = false;
    public GameObject ground;

    public float clearance = 0.1f;
    private bool chosen;

	// Use this for initialization
	protected override void Start () {
        base.Start();
		//preserveUp = this.transform.up;
	}
	
	// Update is called once per frame
	protected override void Update () {

        if (ground != null) {
            Vector2 movevec = new Vector2(moveSpeed,0);

            if(!direction)
                movevec *= -1;

            rigidbody2D.velocity = movevec;

            if (ground.collider2D.bounds.min.x > collider2D.bounds.max.x + clearance ||
                ground.collider2D.bounds.max.x < collider2D.bounds.min.x - clearance)
            {
                ground = null;
                chosen = false;
                GetComponent<Animator>().SetBool("falling", true);
            }
        }else{
            rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
        }
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.collider.bounds.max.y <= collider2D.bounds.min.y && !chosen)
        {
            direction = !direction;
            ground = coll.gameObject;

            chosen = true;
            GetComponent<Animator>().SetBool("falling", false);
        }
    }
}
