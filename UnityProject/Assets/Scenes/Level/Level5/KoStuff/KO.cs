using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : Entity
{
    private List<KOAttack> KOAttacks;
   private KOAttack currentAttack = null;

    private KoPlatforms myPlats;

    private Animator myAnimator;

    // ALLLLLL the Data Members
    private bool facingRight = false;

    public Player player;

    private Vector2 preserveUp;
    public bool ignoreEdges = false;
    public float moveSpeed = 4;
    private GameObject ground;
    private GameObject target;
    public LayerMask layerMask;

    protected override void Start()
    {
        preserveUp = this.transform.up;
        myPlats = GameObject.Find("KoPlatforms").GetComponent<KoPlatforms>();
        myAnimator = GetComponent<Animator>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (currentAttack == null)
            currentAttack = getRandomAttack();
        else
        {
            if (currentAttack.doneAttacking)
            {
                //currentAttack.enabled = false;
                currentAttack = null;
            }
            else
            {
                if (facingRight && (rigidbody2D.velocity.x < 0) || (!facingRight && (rigidbody2D.velocity.x > 0)))
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

                facingRight = rigidbody2D.velocity.x > 0;

                currentAttack.Update();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (InFOV(coll.gameObject))
            {
                target = coll.gameObject;
            }
        }
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

    public override void die()
    {
        myAnimator.SetBool("dead", true);
        base.die(); // Play sound and set bool "dead" to true
    }

    private KOAttack getRandomAttack()
    {
        return KOAttacks[0]; // For now
    }

    public void teleportToRandomPlat()
    {
        int randomPlat = Random.Range(0, myPlats.koPlats.Count);
        transform.position = getCenterPos(myPlats.koPlats[randomPlat]);
    }

    public void teleportToRandomCoolPos()
    {
        int randomPos = Random.Range(0, myPlats.coolAttackPos.Count);
        transform.position = myPlats.coolAttackPos[randomPos].transform.position;
    }

    private Vector3 getCenterPos(GameObject _gobj)
    {
        BoxCollider2D coll = _gobj.GetComponent<BoxCollider2D>();

        Vector3 pos = _gobj.transform.position;

        return new Vector3(pos.x + coll.size.x * 0.7f, pos.y + coll.size.y * 6);
    }
}