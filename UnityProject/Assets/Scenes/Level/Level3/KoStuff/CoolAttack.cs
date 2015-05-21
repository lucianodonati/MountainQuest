using System.Collections;
using UnityEngine;

public class CoolAttack : KOAttack
{
    private int jumps;
    private float timer = 1.0f, animTimer = 4.5f;
    private KO me;
    private Animator anim;
    private bool shoot = false;

    // Use this for initialization
    public override void Start()
    {
        me = GetComponent<KO>();
        player = GameObject.Find("Player");
        anim = me.GetComponent<Animator>();
       // jumps = Random.Range(2, 4);
        jumps = 2;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (jumps > 0)
        {
            if (timer <= 0.0f)
            {
                timer = 1.0f;
                jumps--;
                me.teleportToRandomCoolPos();
               // anim.SetInteger("attack", 0);
            }
            else
            {
              //  anim.SetInteger("attack", myAnim);
                timer -= Time.deltaTime;
            }
        }
        else
        {
            if (animTimer > 0 && animTimer <= 4.0 && animFinished == false)
            {
                anim.SetInteger("attack", myAnim);
                animFinished = true; 
            }
            animTimer -= Time.deltaTime;
            if (animTimer < 0.0f && !shoot)
            {
                shoot = true;
                Vector3 playerPos = player.transform.position;
                playerPos -= transform.position;
                playerPos.z = 0;
                for (int i = 0; i < 2; i++)
                {
                    int mult = i % 2 == 0 ? 1 : -1;
                    GameObject currArrow = (GameObject)Instantiate(me.SoulOrb, gameObject.transform.position + Vector3.forward + new Vector3(mult * 10, -2),
                                                      Quaternion.FromToRotation(transform.up, playerPos));

                    currArrow.GetComponent<Arrow>().owner = this.gameObject;

                    currArrow.rigidbody2D.velocity = playerPos.normalized * 7.5f;
                }
                anim.SetInteger("attack", 0);
                Destroy(this);
            }
        }
    }
}