using UnityEngine;
using System.Collections;

public class Axe : MonoBehaviour {

    public float arcHeight = 10;
    public float arcHeightError = 3;
    public float speed = 10;
    public float speedError = 2;
    public float spinSpeed = 5;

    public DamageType damageType;

	// Use this for initialization
    void Start()
    {

        rigidbody2D.velocity = new Vector3(0, arcHeight + Random.Range(-arcHeightError,arcHeightError), 0) 
                                    + (transform.up * (speed + Random.Range(-speedError, speedError)));

        if (rigidbody2D.velocity.x > 0)
            rigidbody2D.angularVelocity = -spinSpeed;
        else if (rigidbody2D.velocity.x < 0)
            rigidbody2D.angularVelocity = spinSpeed;

    }
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            damageType.attachToEnemy(coll.gameObject.GetComponent<Entity>());
            Destroy(gameObject);
        }
        else if (coll.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
