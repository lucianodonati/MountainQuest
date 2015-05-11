using System.Collections;
using UnityEngine;

public class RangedAI : AttackAI
{
    public LayerMask layerMask;

    private GameObject target;

    public float maximumRange;
    public float minimumRange;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (target != null)
        {
            if (InFOV(target))
            {
                Attack();
            }

            if ((target.transform.position - transform.position).magnitude > maximumRange)
                target = null;
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

    private void Attack()
    {
        if (reloadTimer > 0.0f)
            reloadTimer -= Time.deltaTime;

        if (reloadTimer <= 0.0f)
        {
            GameObject currArrow = (GameObject)Instantiate(weapon,
                                                            gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(transform.up, (Vector3)((Vector2)(target.transform.position - transform.position))));

            currArrow.rigidbody2D.velocity = (target.transform.position - transform.position).normalized * 7.5f;

            if (currArrow.GetComponent<Arrow>() != null)
                currArrow.GetComponent<Arrow>().owner = this.gameObject;

            reloadTimer = reloadTimerMax;
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