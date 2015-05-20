using System.Collections;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int AnimationInt = 1;

    public bool swinging = false;
    public bool halfswung = false;

    public bool ownerDirection;

    public float swordspeed = 8f;
    public float lowSwingAngle = 90f;
    public float swingMinX = 0.5f;
    public float swingMaxX = 1f;
    public float swingMaxY = 0.5f;
    public float swingMinY = -0.3f;

    public DamageType damageType;

    private bool hit = false;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (swinging)
        {
            Quaternion toRot;
            Vector3 toPos;
            if (!halfswung)
            {
                if (ownerDirection)
                {
                    toRot = Quaternion.Euler(0, 0, -lowSwingAngle);
                    toPos = new Vector3(swingMaxX, swingMinY, -1f);
                }
                else
                {
                    toRot = Quaternion.Euler(0, 0, lowSwingAngle);
                    toPos = new Vector3(-swingMaxX, swingMinY, -1f);
                }
            }
            else
            {
                if (ownerDirection)
                {
                    toRot = Quaternion.Euler(0, 0, 0);
                    toPos = new Vector3(swingMinX, swingMaxY, -1f);
                }
                else
                {
                    toRot = Quaternion.Euler(0, 0, 0);
                    toPos = new Vector3(-swingMinX, swingMaxY, -1f);
                }
            }

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                              toRot,
                                                              ((swordspeed * 2) * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position,
                                                          transform.parent.position + toPos,
                                                          ((swordspeed * 2) * Time.deltaTime));

            if (Quaternion.Angle(transform.rotation, toRot) < 0.1f && !halfswung)
            {
                halfswung = true;
            }
            else if (Quaternion.Angle(transform.rotation, toRot) < 0.1f && halfswung)
            {
                halfswung = false;
                swinging = false;
                hit = false;
            }
        }
        else
        {
            Animator anim = GetComponentInParent<Animator>();
            if (anim != null)
                anim.SetInteger("attack", 0);
        }
    }

    public void Follow()
    {
        if (!ownerDirection && transform.rotation != Quaternion.Euler(0, 0, 45))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                              Quaternion.Euler(0, 0, 45),
                                                              (swordspeed * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position,
                                                          transform.parent.position + new Vector3(-0.5f, 0.5f, -1f),
                                                          (swordspeed * Time.deltaTime));
        }
        else if (ownerDirection && transform.rotation != Quaternion.Euler(0, 0, -45))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                              Quaternion.Euler(0, 0, -45),
                                                              (swordspeed * Time.deltaTime));
            transform.position = Vector3.Lerp(transform.position,
                                                          transform.parent.position + new Vector3(0.5f, 0.5f, -1f),
                                                          (swordspeed * Time.deltaTime));
        }
    }

    public void Swing()
    {
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
            anim.SetInteger("attack", AnimationInt);
        swinging = true;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag != gameObject.tag && coll.isTrigger == false && !hit)
        {
            if (coll.gameObject.tag == "Enemy")
            {
                damageType.attachToEnemy(coll.gameObject.GetComponent<Entity>());
            }
            else if (coll.gameObject.tag == "Player")
            {
                damageType.attachToEnemy(coll.gameObject.GetComponent<Entity>());
            }

            hit = true;
        }
    }
}