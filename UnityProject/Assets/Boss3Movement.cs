using System.Collections;
using UnityEngine;

public class Boss3Movement : Enemy
{
    public GameObject Arrow;
    public GameObject firebolt;
    public GameObject flames;


    public bool Barraging = false;
    public bool flameThrowing = false;

    public float boltsShot = 0f;
    public float AbilityDelay = 2.0f;
    public float flamesShot = 0f;

    public float shotsTaken = 0;

    public float BarrageDelay = 0.3f;
    public float flameDelay = 0.1f;

    public Player player;
    public bool facingRight = false;

    private float WoFBurnTimer = 1.0f;
    /// ///////////////////////////////////////////

    private Vector2 preserveUp;

    public bool ignoreEdges = false;

    public bool direction;
    public float moveSpeed = 4;

    public GameObject ground;

    public GameObject target;

    public float aggroTimer;
    public float aggroTimerMax = 3.0f;

    public LayerMask layerMask;

    public Vector3 SprayDirection;


    protected override void Start()
    {
        preserveUp = this.transform.up;
        SprayDirection = new Vector3(-5.0f, 0, 0);
        base.Start();
    }
    
    protected override void Update()
    {
        AbilityDelay -= Time.deltaTime;
        WoFBurnTimer -= Time.deltaTime;
        if (WoFBurnTimer <= 0)
        {
            WoFBurnTimer = 1;


            if (Mathf.Abs(player.transform.position.x - transform.position.x) < 6.5)
            {

                player.GetComponent<Health>().TakeDamage(5, false);
                if (player.GetComponent<PlayerController>() != null)
                {
                    Debug.Log("Before: " + player.GetComponent<PlayerController>().movementSpeed);
                    player.GetComponent<PlayerController>().movementSpeed = 5f;
                    Debug.Log("After: " + player.GetComponent<PlayerController>().movementSpeed);
                }
            }
            else
            {
                player.GetComponent<PlayerController>().movementSpeed = 10;
            }

        }
        if (Barraging == true)
        {
            FireBarrage();
        }
        else if (flameThrowing == true)
        {
            Flamethrower();
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
                        Vector3 toPlayer = target.transform.position - transform.position;
                        toPlayer.y = toPlayer.z = 0;

                        if (toPlayer.x < 0)
                            direction = false; //left
                        else if (toPlayer.x > 0)
                            direction = true; //right

                        if (AbilityDelay <= 0)
                        {

                            AbilityDelay = 2.0f;

                            if (Mathf.Abs(toPlayer.x) < 6.5) // if short range
                            {
                                float rand = Random.value;

                                if (rand >= 0 && rand <= 0.29)
                                {
                                   Barraging = true;
                                }
                                else if (rand >= 0.30 && rand <= 0.39)
                                {
                                    Fireball();
                                }
                                else if (rand >= 0.40 && rand <= 1.0)
                                {
                                    //run
                                }

                            }

                            if (Mathf.Abs(toPlayer.x) <= 13.5 && Mathf.Abs(toPlayer.x) >= 6.5) // if medium range
                            {
                                float rand = Random.value;

                                if (rand >= 0 && rand <= 0.59)
                                {
                                    Barraging = true;
                                }
                                else if (rand >= 0.60 && rand <= 0.84)
                                {
                                    Fireball();
                                }
                                else if (rand >= 0.85 && rand <= 1.0)
                                {
                                    flameThrowing = true;
                                }
                            }

                            if (Mathf.Abs(toPlayer.x) >= 13.5) // if long range
                            {
                                float rand = Random.value;

                                if (rand >= 0 && rand <= 0.29)
                                {
                                    Barraging = true;
                                }
                                else if (rand >= 0.30 && rand <= 0.69)
                                {
                                   Fireball();
                                }
                                else if (rand >= 0.70 && rand <= 1.0)
                                {
                                    flameThrowing = true;
                                }
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






    private void FireBarrage()
    {
        BarrageDelay -= Time.deltaTime;
        if (BarrageDelay <= 0)
        {
            float rand = Random.Range(-4.0f, 4.0f);

            Vector3 playerPos = player.transform.position;
            playerPos -= transform.position;
            playerPos.z = 0;

            playerPos.y += rand;

            GameObject currArrow = (GameObject)Instantiate(firebolt, gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(preserveUp, playerPos));

            currArrow.GetComponent<Arrow>().owner = this.gameObject;

            currArrow.rigidbody2D.velocity = playerPos.normalized * 7.5f;

            boltsShot++;
            BarrageDelay = 0.3f;
            if (boltsShot >= 5)
            {
                Barraging = false;
                boltsShot = 0;
            }


        }


    }

    private void Fireball()
    {
        
            Vector3 playerPos = player.transform.position;
            playerPos -= transform.position;
            playerPos.z = 0;

            GameObject currArrow = (GameObject)Instantiate(Arrow, gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(preserveUp, playerPos));

            currArrow.GetComponent<Arrow>().owner = this.gameObject;

            currArrow.rigidbody2D.velocity = playerPos.normalized * 7.5f;
    }

    private void Flamethrower()
    {


        flameDelay -= Time.deltaTime;
        if (flameDelay <= 0)
        {
            float rand = Random.Range(-5.0f, 6.0f);

            Vector3 playerPos = player.transform.position;
            playerPos -= transform.position;
            playerPos.z = 0;

            playerPos.y += rand;

            GameObject flameThrowah = (GameObject)Instantiate(flames, gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(preserveUp, playerPos));

            flameThrowah.GetComponent<Arrow>().owner = this.gameObject;

            flameThrowah.rigidbody2D.velocity = playerPos.normalized * 7.5f;

            flamesShot++;
            flameDelay = 0.03f;
            if (flamesShot >= 100)
            {
                flameThrowing = false;
                flamesShot = 0;
            }


        }

    }





}