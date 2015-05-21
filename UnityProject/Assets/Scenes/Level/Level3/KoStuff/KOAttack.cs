using System.Collections;
using UnityEngine;

public abstract class KOAttack : MonoBehaviour
{
    public bool doneAttacking = false;
    public float attackTimer = 5.0f;
    protected GameObject player;
    public int myAnim = 0;
    public bool animFinished = false;
    public float strikeTimer = 0.4f;
    public virtual void Start()
    {
        attackTimer = 5.0f;
        player = GameObject.Find("Player");
        GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", myAnim);
    }

    public virtual void Update()
    {
        if (attackTimer <= 0)
        {
            GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", 0);
            Destroy(this);
        }

        attackTimer -= Time.deltaTime;
    }
}