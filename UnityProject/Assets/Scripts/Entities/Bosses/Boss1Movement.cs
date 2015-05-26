using System.Collections;
using UnityEngine;

public class Boss1Movement : Enemy
{
    public float stompTimer = 0.5f;
    public float chargeTimer = 1.0f;
    public float runTimer = 2.0f;
    public bool attacking = false;
    public bool running = false;
    public bool charging = false;
    public bool stomping = false;
    public float animTimer = 2.4f;
    public Player player;
    public float strikeTimer = 1.6f;
    public bool facingRight = false;
    private bool stomped = false;
    public bool animFinished = false;
    public bool hitPlayer = false;
    public float abilityDelay = 1.0f;
    /// ///////////////////////////////////////////

    public bool ignoreEdges = false;

    public bool direction;
    public float moveSpeed = 4;

    public GameObject ground;

    public GameObject target;

    public float aggroTimer;
    public float aggroTimerMax = 3.0f;

    public LayerMask layerMask;
    /// ///////////////////////////////////////////

    //screenshake vars
    public float missShakeAmount;

    public float missShakeDampening;

    public float stompHitShakeAmount;
    public float stompHitShakeDampening;

    protected override void Update()
    {
        if (health.currentHP <= 0.0f)
            Destroy(gameObject);
        abilityDelay -= Time.deltaTime;

        //stomping = true;

        if (abilityDelay <= 0)
        {
            if (running == true)
            {
                Running();
            }
            else if (attacking == true)
            {
                AttackPlayer();
            }
            else if (stomping == true)
            {
                Stomp();
            }
            else if (charging == true)
            {
                Charge();
            }
            else
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
                        if (InFOV(target))
                        {
                            Vector3 toPlayer = player.transform.position - transform.position;
                            toPlayer.y = toPlayer.z = 0;

                            if (toPlayer.x < 0)
                                direction = false; //left
                            else if (toPlayer.x > 0)
                                direction = true; //right

                            ///////////////////////////////////////////

                            if (Mathf.Abs(toPlayer.x) <= 6.5)
                            {
                                attacking = true;
                            }
                            else if (Mathf.Abs(toPlayer.x) <= 13.5 && Mathf.Abs(toPlayer.x) >= 6.5)
                            {
                                float rand = Random.value;

                                if (rand >= 0 && rand <= 0.34)
                                {
                                    stomping = true;
                                }
                                else if (rand >= 0.35 && rand <= 0.74)
                                {
                                    charging = true;
                                }
                                else if (rand >= 0.75)
                                {
                                    running = true;
                                }
                            }
                            else if (Mathf.Abs(toPlayer.x) <= 22.5 && Mathf.Abs(toPlayer.x) >= 13.5)
                            {
                                float rand = Random.value;

                                if (rand >= 0 && rand <= 0.44)
                                {
                                    stomping = true;
                                }
                                else if (rand >= 0.45 && rand <= 0.69)
                                {
                                    charging = true;
                                }
                                else if (rand >= 0.70)
                                {
                                    running = true;
                                }
                            }
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
        }
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
        if (coll.gameObject.tag == "Wall" && charging == true)
        {
            Camera.main.gameObject.GetComponent<CameraBehavior>().BeginShake(missShakeAmount, missShakeDampening);
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
            stompTimer = 0.35f;
            stomping = false;
            abilityDelay = 1.0f;
            Camera.main.gameObject.GetComponent<CameraBehavior>().BeginShake(stompHitShakeAmount, stompHitShakeDampening);
            player.GetComponent<Player>().health.TakeDamage(30, false);
            //GetComponent<Animator>().SetInteger("Attack", 3);
            GetComponent<Animator>().SetTrigger("StompLand");
            stomped = false;
        }

        if ((coll.gameObject.tag == "Platform" || coll.gameObject.tag == "Wall") && ground == null)
        {
            if (stomped)
            {
                stompTimer = 0.35f;
                stomped = false;
                Camera.main.gameObject.GetComponent<CameraBehavior>().BeginShake(missShakeAmount, missShakeDampening);
                //GetComponent<Animator>().SetInteger("Attack", 3);
                GetComponent<Animator>().SetTrigger("StompLand");
            }
            stomping = false;
            abilityDelay = 1.0f;
        }

        if (coll.gameObject.tag == "Platform" && ground == null)
            ground = coll.gameObject;
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

    private void AttackPlayer()
    {
        //bool startAtkDir = direction;

        //attackDelay -= Time.deltaTime;
        //if (attackDelay <= 0)
        //{
        //    //GetComponent<Animator>().SetInteger("Attack", 1);
        //    GetComponent<Animator>().SetTrigger("melee");

        //    attacking = false;
        //    attackDelay = 0.7f;
        //    // animation.Play();
        //    if (startAtkDir == direction && Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position) <= 6.5)
        //    {
        //        player.GetComponent<Player>().health.TakeDamage(15, false);
        //    }

        //    tiredTimer = 1.3f;
        //}

        animTimer -= Time.deltaTime;
        strikeTimer -= Time.deltaTime;
        if (animFinished == false)
        {
            animFinished = true;
            GetComponent<Animator>().SetTrigger("meleeAttack");
        }
        if (strikeTimer <= 0 && hitPlayer == false)
        {
            hitPlayer = true;
            if (Vector2.Distance(transform.position, player.transform.position) < 8.0f)
                player.GetComponent<Health>().TakeDamage(40, true);
        }

        if (animTimer <= 0.0f)
        {
            attacking = false;
            GetComponent<Animator>().SetBool("Walking", true);
            animTimer = 3.0f;
            strikeTimer = 1.0f;
            animFinished = false;
            hitPlayer = false;
        }
    }

    private void Stomp()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = toPlayer.z = 0;

        if (toPlayer.x < 0)
            direction = false; //left
        else if (toPlayer.x > 0)
            direction = true; //right

        //GetComponent<Animator>().SetInteger("Attack", 2);
        if (stomped == false)
        {
            GetComponent<Animator>().SetTrigger("StompJump");
            stomped = true;
        }

        //if (stompTimer <= 0)
        //{
        //    stompTimer = 1.0f;
        //    // stomping = false;
        //}
        if (direction == true && stompTimer > 0)
        {
            stompTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(14, 25);
        }
        else if (direction == false & stompTimer > 0)
        {
            stompTimer -= Time.deltaTime;
            rigidbody2D.velocity = new Vector2(-14, 25);
        }
    }

    private void Charge()
    {
        //GetComponent<Animator>().SetInteger("Attack", 0);

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

    private void Running()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = toPlayer.z = 0;

        runTimer -= Time.deltaTime;
        if (runTimer <= 0)
        {
            running = false;
            runTimer = 2.0f;
        }

        if (Mathf.Abs(toPlayer.x) > moveSpeed)
            toPlayer.x = toPlayer.x / Mathf.Abs(toPlayer.x) * moveSpeed;

        rigidbody2D.velocity = toPlayer;
    }
}