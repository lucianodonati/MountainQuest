using UnityEngine;
using System.Collections;

public class Boss1Movement : MonoBehaviour
{
    public float runTimer = 3.0f;
    public bool attacking = false;
    public bool running = false;
    public bool charging = false;
    public bool stomping = false;
    public float attackDelay = 0.9f;
    public Player player;
    public float tiredTimer = 0;
    /// ///////////////////////////////////////////
    
    public bool direction;
    public float moveSpeed = 4;

    private Vector3 preserveUp;

    public GameObject ground;

    public GameObject target;

    public float aggroTimer;
    public float aggroTimerMax = 3.0f;
    
    public LayerMask layerMask;

    // Use this for initialization
    void Start()
    {
        preserveUp = this.transform.up;
    }

    // Update is called once per frame
    void Update()
    {

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
                        attacking = true;
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
                            Stomp();                            
                        }
                        if (charging == true)
                        {
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
                            Charge();                            
                        }
                        if (stomping == true)
                        {
                            Stomp();
                        }

                    }
                    else if (charging == false && stomping == false && attacking == false)
                    {
                        running = true;
                        
                    }

                    if (running == true )
                    {
                        ///////////////////////////////////////////
                        runTimer -= Time.deltaTime;
                        if (runTimer <= 0)
                        {
                            running = false;
                            runTimer = 3.0f;
                        }

                        if (Mathf.Abs(toPlayer.x) > moveSpeed)
                            toPlayer.x = toPlayer.x / Mathf.Abs(toPlayer.x) * moveSpeed;

                        rigidbody2D.velocity = toPlayer;
                    }
                 

                }
                else
                {
                    rigidbody2D.velocity = velocityHold.normalized * moveSpeed;
                }

                if (collider2D.bounds.min.x + (rigidbody2D.velocity.x * Time.deltaTime) < ground.collider2D.bounds.min.x ||
                   collider2D.bounds.max.x + (rigidbody2D.velocity.x * Time.deltaTime) > ground.collider2D.bounds.max.x)
                    rigidbody2D.velocity = Vector2.zero;
            }

        }
        else if (target != null)
        {
            target = null;
        }

        this.transform.up = preserveUp;
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
            charging = false;
        }

        if (coll.gameObject.tag == "Player" && charging == true)
        {
            charging = false;
          //  player.GetComponent<Player>().health.TakeDamage(30);
            // knockback 
            if (direction == false)
            {
                player.GetComponent<Player>().rigidbody2D.AddForce(Vector3.left * 10);                
            }
            if (direction == true)
            {
                player.GetComponent<Player>().rigidbody2D.AddForce(Vector3.right * 10);
            }
        } 
       
        if (coll.gameObject.tag == "Platform" && ground == null)
        {
            ground = coll.gameObject;
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
            print("Danger Will Robinson");
            attacking = false;
            attackDelay = 0.9f;
            // attack animation
            if (startAtkDir == direction && Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position) <= 6.5)
            {
              //  player.GetComponent<Player>().health.TakeDamage(20);
            }
            print("U WOT M8!?");

            tiredTimer = 2.0f;
        }

    } 

    void Stomp()
    {
        stomping = false;
    }



    void Charge()
    {
        
        if (direction == true)
        {
            rigidbody2D.velocity = new Vector3(25, 0, 0);            
        }
        else if (direction == false)
        {
            rigidbody2D.velocity = new Vector3(-25, 0, 0);
        }
    }

}
