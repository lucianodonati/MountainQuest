using UnityEngine;
using System.Collections;

public class Wander_Movement : Enemy_Movement {

	//public bool direction;
	public float moveSpeed = 4;

	//private Vector3 preserveUp;

    private GameObject ground;

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

            if(collider2D.bounds.max.x < ground.collider2D.bounds.min.x ||
                collider2D.bounds.min.x > ground.collider2D.bounds.max.x) {
                    ground = null;
                }
        }else{
            rigidbody2D.velocity = new Vector2(0,rigidbody2D.velocity.y);
        }

		this.transform.up = preserveUp;
	}

	void OnCollisionEnter2D(Collision2D coll){

        if (ground == null)
            direction = !direction;

        ground = coll.gameObject;
	}

    void OnCollisionStay2D(Collision2D coll){
        ground = coll.gameObject;
    }
}
