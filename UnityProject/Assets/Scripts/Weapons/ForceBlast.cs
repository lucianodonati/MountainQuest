using UnityEngine;
using System.Collections;

public class ForceBlast : MonoBehaviour {

    public float speed;

    public DamageType damageType;

	// Use this for initialization
	void Start () {
        rigidbody2D.velocity = transform.up * speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            damageType.attachToEnemy(coll.gameObject.GetComponent<Entity>());
            Destroy(gameObject);
        }else if (coll.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
