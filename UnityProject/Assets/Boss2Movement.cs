using System.Collections;
using UnityEngine;

public class Boss2Movement : Enemy
{
    public GameObject Arrow;

    public bool Flurrying = false;
    public bool Spraying = false;
    public bool Shooting = false;
    public bool atLongRange = false;
    public bool Vaulting = false;
    public float VaultTimer;

    public float shotsTaken = 0;
    public float stabsTaken = 0;

    public float shotDelay = 0.4f;
    public float stabDelay = 0.4f;
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
        SprayDirection = new Vector3(-5.0f, 0, 0);
        base.Start();
    }

    protected override void Update()
    {
        VaultTimer -= Time.deltaTime;

        if (VaultTimer <= 0)
        {
            player.GetComponent<Player>().rigidbody2D.isKinematic = false; 
        }
        if (Vaulting == true)
        {
            Vault();
        }
        if (Flurrying == true)
        {
            FlurryAttack();
        }
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

                    if (InFOV(target))
                    {
                        Vector3 toPlayer = target.transform.position - transform.position;
                        toPlayer.y = toPlayer.z = 0;

                        if (toPlayer.x < 0)
                            direction = false; //left
                        else if (toPlayer.x > 0)
                            direction = true; //right


                        if (Mathf.Abs(toPlayer.x) < 6.5) // if short range
                        {
                            float rand = Random.value;
                            atLongRange = false;

                            if (rand >= 0 && rand <= 0.39 && Flurrying == false)
                            {
                                Vaulting = true;
                            }
                            else if (rand >= 0.40 && rand <= 1.0)
                            {
                                Flurrying = true;
                            }

                        }

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


    private void FlurryAttack()
    {

        stabDelay -= Time.deltaTime;
        // GetComponent<Animator>().SetBool("isAttacking", false);
        // animation.Play()
        if (stabDelay <= 0)
        {
            if (Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position) <= 4.0)
            {
                player.GetComponent<Player>().health.TakeDamage(11, false);
            }

            stabsTaken++;
            stabDelay = 0.4f;
            if (stabsTaken >= 5)
            {
                Flurrying = false;
            }
        }
    }



    private void Vault()
    {
        Rigidbody2D playerBody = player.GetComponent<Player>().rigidbody2D;

        if (direction == false)
        {
            Vector2 KB = new Vector2(-4, 2);

            playerBody.isKinematic = true;
            // playerBody.AddForce(Vector3.left * 1000);
            playerBody.velocity += KB;

            Vector2 jumpBack = new Vector2(7, 5);
            gameObject.rigidbody2D.velocity += jumpBack;
        }
        if (direction == true)
        {
            Vector2 KB = new Vector2(4, 2);

            playerBody.isKinematic = true;
           // playerBody.AddForce(Vector3.right * 1000);
            playerBody.velocity += KB;

            Vector2 jumpBack = new Vector2(-7, 5);
            gameObject.rigidbody2D.velocity += jumpBack;
        }
        VaultTimer = 0.7f;

        ShootPlayer();
        Vaulting = false;
    }

}