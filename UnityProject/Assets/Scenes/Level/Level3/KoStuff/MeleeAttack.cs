using System.Collections;
using UnityEngine;

public class MeleeAttack : KOAttack
{
    private KO ko;
    private int jumps;
    private float jumpTimer = 0.8f, animTimer = 2.0f;

    // Use this for initialization
    public override void Start()
    {
        strikeTimer = 1.4f;
        player = GameObject.Find("Player");
        ko = GetComponent<KO>();
        jumps = Random.Range(2, 4);

    }

    // Update is called once per frame
    public override void Update()
    {
        if (jumpTimer <= 0.0f || jumps < 0)
        {
            if (jumps < 0)
            {
                animTimer -= Time.deltaTime;
                strikeTimer -= Time.deltaTime;
                GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", myAnim);
                if (strikeTimer <= 0 && animFinished == false)
                {
                    animFinished = true;
                    if (Vector2.Distance(transform.position, player.transform.position) < 6.0f)
                    player.GetComponent<Health>().TakeDamage(40, true);                    
                }
                //if (animTimer > 0.0f && animTimer <= 2.0f && animFinished == false)
                //{
                //    animFinished = true;
                //    GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", myAnim);
                //    if (Vector2.Distance(transform.position, player.transform.position) < 6.0f)
                //        player.GetComponent<Health>().TakeDamage(40, true);
                //}

                if (animTimer <= 0.0f)
                {
                    Destroy(this);
                    GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", 0);
                }
            }
            else
            {
                ko.teleportToRandomPlat();
                jumpTimer = 0.8f;
                jumps--;
            }
        }
        else
            jumpTimer -= Time.deltaTime;
    }
}