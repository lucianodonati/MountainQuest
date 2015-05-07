using UnityEngine;
using System.Collections;

public class ChargeAI : AttackAI {

    private float chargeTimer;
    public float chargeTimerMax = 1.0f;
    public bool charging = false;
    public float chargeSpeed = 25.0f;

    public float percentChance = 30.0f;

    public float maximumRange;
    public float minimumRange;

    public LayerMask layerMask;

    private GameObject target;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {

        if (reloadTimer > 0.0f)
            reloadTimer -= Time.deltaTime;
        else if (target != null)
        {
            if (Random.Range(0.0f, 100.0f) / 100.0f <= percentChance && !charging && InFOV(target))
                charging = true;

            if (charging)
            {
                coordinator.currentMovement.enabled = false;
                Charge();
            }
            else
                coordinator.currentMovement.enabled = true;
        }

	}

    void Charge()
    {
        if (chargeTimer <= 0.0f)
        {
            reloadTimer = reloadTimerMax;
            chargeTimer = chargeTimerMax;
            charging = false;
        }
        else
        {
            chargeTimer -= Time.deltaTime;

            if (coordinator.currentMovement.direction)
                rigidbody2D.velocity = new Vector3(chargeSpeed, 0, 0);
            else
                rigidbody2D.velocity = new Vector3(-chargeSpeed, 0, 0);
        }
    }

    public bool InFOV(GameObject targ)
    {
        bool val = false;

        RaycastHit2D checkFOV =
            Physics2D.Linecast(transform.position, targ.transform.position, layerMask);

        if (checkFOV.collider != null)
            if (checkFOV.collider.transform == targ.transform && checkFOV.distance > minimumRange && checkFOV.distance <= maximumRange)
                val = true;

        return val;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if(charging && Mathf.Abs(rigidbody2D.velocity.x/chargeSpeed) >= 0.9f)
        {
            reloadTimer = reloadTimerMax;

            if(coll.gameObject.tag == "Wall")
                chargeTimer = 0.0f;
            else if (coll.gameObject.tag == "Player")
            {
                chargeTimer = 0.0f;

                coll.gameObject.GetComponent<Player>().health.TakeDamage(20, false);

                coll.gameObject.GetComponent<Player>().rigidbody2D.AddForce((coordinator.currentMovement.direction
                                                                            ? Vector3.right
                                                                            : Vector3.left)
                                                                                            * 1000);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (InFOV(coll.gameObject))
                target = coll.gameObject;
        }
    }
}
