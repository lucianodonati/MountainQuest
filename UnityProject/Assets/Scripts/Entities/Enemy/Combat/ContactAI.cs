using UnityEngine;
using System.Collections;

public class ContactAI : AttackAI {

    public DamageType damageType;

	// Use this for initialization
	protected override void Start () {

	}
	
	// Update is called once per frame
	protected override void Update () {
        if (reloadTimer > 0.0f)
            reloadTimer -= Time.deltaTime;
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if(reloadTimer <= 0.0f)
        {
            if (coll.gameObject.tag == "Player")
            {
                damageType.attachToEnemy(coll.gameObject.GetComponent<Entity>());

                reloadTimer = reloadTimerMax;
            }
        }
    }
}
