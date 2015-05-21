using System.Collections;
using UnityEngine;

public class PlatAttack : KOAttack
{
    private KO ko;
    private int jumpsMax = 5, jumps;
    private float jumpTimer = 0.8f, animTimer = 4.0f;

    // Use this for initialization
    public override void Start()
    {
        strikeTimer = 3.0f;
        player = GameObject.Find("Player");
        ko = GetComponent<KO>();
        jumps = Random.Range(1, jumpsMax);
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
                if (animTimer <= 0.0f)
                {

                    GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", 0);
                    Destroy(this);
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