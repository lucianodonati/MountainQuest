using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : Entity
{
    private List<KOAttack> KOAttacks;
    private KOAttack currentAttack;

    // ALLLLLL the Data Members
    private bool facingRight = false;

    public Player player;

    private Vector2 preserveUp;
    public bool ignoreEdges = false;
    public bool direction;
    public float moveSpeed = 4;
    private GameObject ground;
    private GameObject target;
    public LayerMask layerMask;

    protected override void Start()
    {
        preserveUp = this.transform.up;
        base.Start();
    }

    protected override void Update()
    {
        //if (currentAttack.doneAttacking)
        //    currentAttack = getRandomAttack();

        if (facingRight && (rigidbody2D.velocity.x < 0) || (!facingRight && (rigidbody2D.velocity.x > 0)))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        if (rigidbody2D.velocity.x > 0)
            facingRight = true;
        else if (rigidbody2D.velocity.x < 0)
            facingRight = false;

        base.Update();


        if (InFOV(target))
        {
            Vector3 toPlayer = player.transform.position - transform.position;
            toPlayer.y = toPlayer.z = 0;

            if (toPlayer.x < 0)
                direction = false; //left
            else if (toPlayer.x > 0)
                direction = true; //right

            // All Luci's attack choosing jargon
        }

        if (ground != null)
        {
            if (!ignoreEdges)
            {
                if (collider2D.bounds.min.x + (rigidbody2D.velocity.x * Time.deltaTime) < ground.collider2D.bounds.min.x ||
                   collider2D.bounds.max.x + (rigidbody2D.velocity.x * Time.deltaTime) > ground.collider2D.bounds.max.x)
                    rigidbody2D.velocity = Vector2.zero;
            }
        }
        //else if (target != null)
        //{
        //    target = null;
        //}
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (InFOV(coll.gameObject))
            {
                target = coll.gameObject;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform" && ground == null)
        {
            ground = coll.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            ground = null;
        }
    }

    private bool InFOV(GameObject targ)
    {
        bool val = false;
        if (targ == null)
            targ = GameObject.Find("Player");

        RaycastHit2D checkFOV =
            Physics2D.Linecast(transform.position, targ.transform.position, layerMask);
        if (checkFOV.collider != null)
        {
            if (checkFOV.collider.transform == targ.transform)
            {
                val = true;
            }
        }
        Debug.DrawLine(transform.position, checkFOV.point);

        return val;
    }

    public override void die()
    {
        // Play animation here

        base.die(); // Play sound and set bool "dead" to true
    }

    private KOAttack getRandomAttack()
    {
        return KOAttacks[0]; // For now
    }
}