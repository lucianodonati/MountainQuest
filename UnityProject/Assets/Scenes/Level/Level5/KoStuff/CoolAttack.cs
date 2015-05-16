using System.Collections;
using UnityEngine;

public class CoolAttack : KOAttack
{
    private int jumps;
    private float timer = 1.0f;
    private KO me;

    // Use this for initialization
    private void Start()
    {
        me = GetComponent<KO>();
        jumps = 4;
    }

    // Update is called once per frame
    private void Update()
    {
        base.Update();

        attackTimer = 5.0f;
        if (jumps > 0)
        {
            if (timer < 0.0f)
            {
                timer = 1.0f;
                jumps--;
                me.teleportToRandomCoolPos();
                GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", 0);
            }
            else
            {
                GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", myAnim);
                timer -= Time.deltaTime;
            }
        }
        else
        {
            GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", myAnim);
        }
    }
}