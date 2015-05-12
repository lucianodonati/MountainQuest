using System.Collections;
using UnityEngine;

public class Boss2Movement : Enemy
{
    public GameObject Arrow;

    public bool Spraying = false;
    public bool Shooting = false;
    public bool atLongRange = false;

    public float shotsTaken = 0;
    public float runTimer = 2.0f;
    public bool attacking = false;

    float shotDelay = 0.3f;

    public Player player;
    public bool facingRight = false;
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
        SprayDirection = new Vector3(-5.0f, 0,0);
        base.Start();
    }

    protected override void Update()
    {
        if (Spraying == true) // If teh boss is on Arrow spray mode ignore all other updates
        {
            ArrowSpray();
        }
        else if (Shooting == true) // If teh boss is on Shooting mode ignore all other updates
        {
            ShootPlayer();
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
                    Vector2 velocityHold = rigidbody2D.velocity;

                    if (InFOV(target))
                    {
                        Vector3 toPlayer = target.transform.position - transform.position;
                        toPlayer.y = toPlayer.z = 0;

                        if (toPlayer.x < 0)
                            direction = false; //left
                        else if (toPlayer.x > 0)
                            direction = true; //right





                        if (Mathf.Abs(toPlayer.x) <= 13.5 && Mathf.Abs(toPlayer.x) >= 6.5) // if medium range
                        {
                            float rand = Random.value;
                            atLongRange = false;

                            if (rand >= 0 && rand <= 0.29)
                            {
                                Spraying = true;
                            }
                            else if (rand >= 0.30 && rand <= 1.0)
                            {
                                Shooting = true;

                            }


                        }

                        if (Mathf.Abs(toPlayer.x) >= 13.5) // if long range
                        {
                            float rand = Random.value;
                            atLongRange = true;

                            if (rand >= 0 && rand <= 0.19)
                            {
                                Spraying = true;
                            }
                            else if (rand >= 0.20 && rand <= 1.0)
                            {
                                Shooting = true;
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






    private void ArrowSpray()
    {
        shotDelay -= Time.deltaTime;
        if (shotDelay <= 0)
        {
            
            if (SprayDirection.x <= 0)
            SprayDirection += new Vector3(1, 0.5f, 0);
            if (SprayDirection.x > 0)
               SprayDirection += new Vector3(1, -0.5f, 0);

            Vector3 ArrowAngle = transform.position + SprayDirection;
            ArrowAngle -= transform.position;
            ArrowAngle.z = 0;

            GameObject currArrow = (GameObject)Instantiate(Arrow, gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(preserveUp, ArrowAngle));

            currArrow.rigidbody2D.velocity = ArrowAngle.normalized * 7.5f;

            if (atLongRange == true)
            {
                shotDelay = 0.2f;
            }
            else
            shotDelay = 0.4f;

            if (SprayDirection.x >= 6)
            {
                Spraying = false;
                SprayDirection = new Vector3(-5.0f, 0, 0);
            }
        }
        


    }

    private void ShootPlayer()
    {
        shotDelay -= Time.deltaTime;
        if (shotDelay <= 0)
        {
            Vector3 playerPos = player.transform.position;
            playerPos -= transform.position;
            playerPos.z = 0;

            GameObject currArrow = (GameObject)Instantiate(Arrow, gameObject.transform.position + Vector3.back,
                                                            Quaternion.FromToRotation(preserveUp, playerPos));

            currArrow.rigidbody2D.velocity = playerPos.normalized * 7.5f;
            shotsTaken++;

            if (atLongRange == true)
            {
                shotDelay = 0.2f;
            }
            else
                shotDelay = 0.4f;

            if (shotsTaken >= 5)
            {
                Shooting = false;
            }
        }

    }

}


