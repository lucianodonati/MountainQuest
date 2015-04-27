using UnityEngine;
using System.Collections;

public class Boss1Movement : Enemy
{
    public float stompTimer = 1.0f;
    public float chargeTimer = 1.0f;
    public float runTimer = 2.0f;
    public bool attacking = false;
    public bool running = false;
    public bool charging = false;
    public bool stomping = false;
    public float attackDelay = 0.7f;
    public Player player;
    public float tiredTimer = 0;
    public bool facingRight = false;
    /// ///////////////////////////////////////////

    public bool ignoreEdges = false;

    public bool direction;
    public float moveSpeed = 4;


    public GameObject ground;

    public GameObject target;

    public float aggroTimer;
    public float aggroTimerMax = 3.0f;

    public LayerMask layerMask;



    protected override void Update()
    {
        if (facingRight && (rigidbody2D.velocity.x < 0) || (!facingRight && (rigidbody2D.velocity.x > 0)))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
       
        if (rigidbody2D.velocity.x > 0)
            facingRight = true;
        else if (rigidbody2D.velocity.x < 0)
            facingRight = false;



        base.Update();

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

                    ///////////////////////////////////////////
                    if (Mathf.Abs(toPlayer.x) <= 6.5 && running == false && charging == false && stomping == false)
                    {
                        attacking = true;
                        GetComponent<Animator>().SetBool("isAttacking", true);
                    }

                    tiredTimer -= Time.deltaTime;
                    if (attacking == true && tiredTimer <= 0)
                    {
                        AttackPlayer();
                    }

                    if (Mathf.Abs(toPlayer.x) <= 13.5 && Mathf.Abs(toPlayer.x) >= 6.5)
                    {
                        float rand = Random.value;

                        if (rand >= 0 && rand <= 0.14 && running == false && charging == false && attacking == false)
                        {
                            stomping = true;
                        }
                        else if (rand >= 0.15 && rand <= 0.50 && running == false && attacking == false && stomping == false)
                        {
                            charging = true;
                        }

                        if (stomping == true)
                        {
                            stompTimer -= Time.deltaTime;
                            Stomp();
                        }
                        if (charging == true)
                        {
                            chargeTimer -= Time.deltaTime;
                            Charge();
                        }
                    }


                    if (Mathf.Abs(toPlayer.x) <= 22.5 && Mathf.Abs(toPlayer.x) >= 13.5)
                    {
                        float rand = Random.value;

                        if (rand >= 0 && rand <= 0.44 && running == false && charging == false && attacking == false)
                        {
                            stomping = true;
                        }
                        else if (rand >= 0.45 && rand <= 0.80 && running == false && attacking == false && stomping == false)
                        {
                            charging = true;
                        }

                        if (charging == true)
                        {
                            chargeTimer -= Time.deltaTime;
                            Charge();
                        }
                        if (stomping == true)
                        {
                            stompTimer -= Time.deltaTime;
                            Stomp();
                        }

                    }
                    else if (charging == false && stomping == false && attacking == false)
                    {
                        running = true;

                    }

                    if (running == true)
                    {
                        ///////////////////////////////////////////
                        runTimer -= Time.deltaTime;
                        if (runTimer <= 0)
                        {
                            running = false;
                            runTimer = 2.0f;
                        }
                        if (isSlowed == true)
                        {
                            if (Mathf.Abs(toPlayer.x) > moveSpeed)
                                toPlayer.x = toPlayer.x / Mathf.Abs(toPlayer.x) * moveSpeed / 2;
                        }
                        else if (isSlowed == false)
                        {
                            if (Mathf.Abs(toPlayer.x) > moveSpeed)
                                toPlayer.x = toPlayer.x / Mathf.Abs(toPlayer.x) * moveSpeed;
                        }

                        rigidbody2D.velocity = toPlayer;
                    }


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

    }

    void OnTriggerStay2D(Collider2D coll)
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

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Wall" && charging == true)
        {
            chargeTimer = 1.0f;
            charging = false;
        }

        if (coll.gameObject.tag == "Player" && charging == true)
        {
            chargeTimer = 1.0f;
            charging = false;
            player.GetComponent<Player>().health.TakeDamage(20, false);
            if (direction == false)
            {
                // player.GetComponent<Player>().rigidbody2D.isKinematic = true;
                player.GetComponent<Player>().rigidbody2D.AddForce(Vector3.left * 1000);
            }
            if (direction == true)
            {
                // player.GetComponent<Player>().rigidbody2D.isKinematic = true;
                player.GetComponent<Player>().rigidbody2D.AddForce(Vector3.right * 1000);
            }

        }

        if (coll.gameObject.tag == "Player" && stomping == true)
        {
            stompTimer = 1.0f;
            stomping = false;
            player.GetComponent<Player>().health.TakeDamage(30, false);


        }


        if (coll.gameObject.tag == "Platform" && ground == null)
        {
            ground = coll.gameObject;
            stomping = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform" && ground == null)
        {
            ground = coll.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            ground = null;
        }
    }

    bool InFOV(GameObject targ)
    {

        bool val = false;

        RaycastHit2D checkFOV =
            Physics2D.Linecast(transform.position, targ.transform.position, layerMask);

        if (checkFOV.collider.transform == targ.transform)
        {
            val = true;
        }

        Debug.DrawLine(transform.position, checkFOV.point);

        return val;
    }

    void AttackPlayer()
    {
        bool startAtkDir = direction;

        attackDelay -= Time.deltaTime;
        if (attackDelay <= 0)
        {
            GetComponent<Animator>().SetBool("isAttacking", false);
            attacking = false;
            attackDelay = 0.7f;
            // animation.Play();
            if (startAtkDir == direction && Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position) <= 6.5)
            {
                player.GetComponent<Player>().health.TakeDamage(15, false);
            }

            tiredTimer = 1.3f;
        }

    }

    void Stomp()
    {
        if (stompTimer <= 0)
        {
            stompTimer = 1.0f;
            stomping = false;
        }
        if (direction == true)
        {
            stompTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(7, 12);

        }
        else if (direction == false)
        {
            stompTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(-7, 12);

        }
    }



    void Charge()
    {
        if (chargeTimer <= 0)
        {
            chargeTimer = 1.0f;
            charging = false;
        }
        if (direction == true)
        {
            chargeTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector3(25, 0, 0);
        }
        else if (direction == false)
        {
            chargeTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector3(-25, 0, 0);
        }
    }

}
