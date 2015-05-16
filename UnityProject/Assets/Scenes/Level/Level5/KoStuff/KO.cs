using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KO : Entity
{
    private int currentAttack = -1;
    private KOAttack currentKOAttack;
    private KoPlatforms myPlats;
    private float deathTimerPoop = 3.5f;
    private Animator myAnimator;
    public GameObject Dagger, SoulOrb;

    private ParticleSystem psys;
    public GameObject Emitter;

    // ALLLLLL the Data Members
    private bool facingRight = false;

    protected override void Start()
    {
        myPlats = GameObject.Find("KoPlatforms").GetComponent<KoPlatforms>();
        myAnimator = GetComponent<Animator>();
        GameObject emitter = (GameObject)Instantiate(Emitter, transform.position, transform.rotation);
        emitter.transform.parent = transform;
        psys = emitter.GetComponent<ParticleSystem>();
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (!dead)
        {
            if (health.currentHP <= 0)
                die();

            if (currentAttack == -1)
            {
                teleportToRandomPlat();
                currentAttack = getRandomAttack();
                switch (currentAttack)
                {
                    case 1:
                        currentKOAttack = gameObject.AddComponent<SteelTornado>();
                        break;

                    case 2:
                        currentKOAttack = gameObject.AddComponent<PlatAttack>();
                        break;

                    case 3:
                        currentKOAttack = gameObject.AddComponent<CoolAttack>();
                        break;

                    case 4:
                        break;

                    case 5:
                        currentKOAttack = gameObject.AddComponent<RainOfDaggers>();
                        break;
                }
                if (currentKOAttack != null)
                    currentKOAttack.myAnim = currentAttack;
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
        else
        {
            if (deathTimerPoop < 0.0f)
            {
                deathTimerPoop = 0.0f;
                SoundFX sfx = GetComponent<SoundFX>();
                if (sfx != null)
                    sfx.Play("Died");
            }
            else if (deathTimerPoop > 0.0f)
                deathTimerPoop -= Time.deltaTime;
        }
    }

    public override void die()
    {
        CameraWaypoint cw = gameObject.AddComponent<CameraWaypoint>();
        cw.stationaryFocus = this.transform;
        cw.isStationary = true;
        cw.cameraSize = 8.0f;

        Camera.main.GetComponent<CameraBehavior>().SendMessage("SetView", cw);

        if (currentKOAttack != null)
            Destroy(currentKOAttack);
        myAnimator.SetInteger("attack", 0);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        teleportToRandomPlat();
        rigidbody2D.velocity = new Vector3();

        myAnimator.SetBool("dead", true);

        dead = true;
    }

    private int getRandomAttack()
    {
        //return Random.Range(1, 5); // For now
        return 2; // For now
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

        return new Vector3(pos.x + coll.size.x * 0.7f, pos.y + coll.size.y * 7);
    }

    public void healKO(float _ammount)
    {
        health.Heal(_ammount);
        psys.Emit((int)_ammount);
    }
}