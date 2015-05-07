﻿using System.Collections;
using UnityEngine;

public class Seek_Movement : Enemy_Movement
{
    //public bool direction;
    public float moveSpeed = 4;

    //private Vector3 preserveUp;

    public GameObject ground;

    public GameObject target;

    public float aggroTimer;
    public float aggroTimerMax = 3.0f;

    public LayerMask layerMask;

    public bool ignoreEdges = false;

    // Use this for initialization
    protected override void Start()
    {
        //preserveUp = this.transform.up;
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GetComponent<Entity>().isSlowed)
            moveSpeed = 3;
        if (aggroTimer > 0.0f)
        {
            aggroTimer -= Time.deltaTime;

            if (ground != null)
            {
                Vector2 velocityHold = rigidbody2D.velocity;

                if (InFOV(target))
                {
                    Vector3 toPlayer = target.transform.position - transform.position;
                    toPlayer.y = toPlayer.z = 0;

                    if (toPlayer.x < 0)
                        direction = false; //left
                    else if (toPlayer.x > 0)
                        direction = true; //right

                    if (Mathf.Abs(toPlayer.x) > moveSpeed)
                        toPlayer.x = toPlayer.x / Mathf.Abs(toPlayer.x) * moveSpeed;

                    rigidbody2D.velocity = toPlayer;
                }
                else
                {
                    rigidbody2D.velocity = velocityHold.normalized * moveSpeed;
                }

                if (!ignoreEdges)
                {
                    if (collider2D.bounds.min.x + (rigidbody2D.velocity.x * Time.deltaTime) < ground.collider2D.bounds.min.x ||
                       collider2D.bounds.max.x + (rigidbody2D.velocity.x * Time.deltaTime) > ground.collider2D.bounds.max.x)
                        rigidbody2D.velocity = Vector2.zero;
                }
            }
        }
        else if (target != null)
        {
            target = null;
        }

        this.transform.up = preserveUp;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (InFOV(coll.gameObject))
            {
                aggroTimer = aggroTimerMax;
                target = coll.gameObject;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform" && ground == null)
        {
            ground = coll.gameObject;
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

    public bool InFOV(GameObject targ)
    {
        bool val = false;

        RaycastHit2D checkFOV =
            Physics2D.Linecast(transform.position, targ.transform.position, layerMask);
        if (checkFOV.collider != null)
            if (checkFOV.collider.transform == targ.transform)
                val = true;

        Debug.DrawLine(transform.position, checkFOV.point);

        return val;
    }
}