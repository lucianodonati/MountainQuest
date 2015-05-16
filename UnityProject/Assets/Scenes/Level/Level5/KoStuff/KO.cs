using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : Entity
{
    private int currentAttack = -1;
    private KOAttack currentKOAttack;
    private KoPlatforms myPlats;

    private Animator myAnimator;

    // ALLLLLL the Data Members
    private bool facingRight = false;

    protected override void Start()
    {
        myPlats = GameObject.Find("KoPlatforms").GetComponent<KoPlatforms>();
        myAnimator = GetComponent<Animator>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (currentAttack == -1)
        {
            teleportToRandomPlat();
            currentAttack = getRandomAttack();
            switch (currentAttack)
            {
                case 1:
                    currentKOAttack = gameObject.AddComponent<SteelTornado>();
                    currentKOAttack.myAnim = 1;
                    break;
            }
        }
        else
        {
            if (facingRight && (rigidbody2D.velocity.x < 0) || (!facingRight && (rigidbody2D.velocity.x > 0)))
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

            facingRight = rigidbody2D.velocity.x > 0;

            if (currentKOAttack == null)
                currentAttack = -1;
        }
    }

    public override void die()
    {
        myAnimator.SetBool("dead", true);
        base.die(); // Play sound and set bool "dead" to true
    }

    private int getRandomAttack()
    {
        //return Random.Range(1, 5); // For now
        return 1; // For now
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