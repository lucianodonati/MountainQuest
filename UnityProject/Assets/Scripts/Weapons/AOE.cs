using UnityEngine;
using System.Collections;

public class AOE : MonoBehaviour {
    public float duration;
    public float maxRadius;
    public float blastForce;

	// Use this for initialization
	void Start () {
        GetComponent<CircleCollider2D>().radius = 0;
	}
	
	// Update is called once per frame
	void Update () {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle.radius < maxRadius)
            circle.radius += maxRadius / duration * Time.deltaTime;
        else
        {
            circle.enabled = false;
            enabled = false;
        }
	}

    void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Enemy" && !coll.isTrigger)
        {
            if (coll.gameObject.GetComponent<Entity>())
            {
                Vector2 toTarg = (Vector2)(coll.gameObject.transform.position - transform.position);

                coll.gameObject.rigidbody2D.velocity =
                    toTarg.normalized * (blastForce * (maxRadius / toTarg.magnitude));
            }
        }
    }
}
