﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // calculated for rigidbody2d
    // mass 5
    // linear drag 0
    // angular drag 0.05
    // gravity scale 4
    public bool movementEnabled = true, combatEnabled = true;

    // MOVEMENT VARS
    public float movementSpeed = 10;

    public float dragCoef = 0.2f;

    public float jumpSpeed = 1f;
    private float jumpTimer;
    public float jumpTimerMax = 0.1f;
    public float firstJumpModifier = 20.0f;
    private bool jumplock = false;

    public bool grounded = false;

    public bool facingRight = true;

    private float jumpCooldownTimer;
    public float jumpCooldownTimerMax = 0.1f;
    public float redirectedTimer = 0;

    private Vector2 preserveUp;

    //ARROW VARS
    [HideInInspector]
    public SkillsManager.SetArrowPrefabs Arrow;

    private float arrowCooldownTimer;
    public float arrowCooldownTimerMax = 0.5f;

    //SWORD VARS
    [HideInInspector]
    public GameObject Sword = null;

    //SWITCH VARS
    private bool usingSword = false;

    public GameObject[] Arrows;
    public GameObject[] Swords;

    private int arrowiter;
    private int sworditer;

    //target for looking
    public Vector3 looktarget;

    //REFERENCES TO CHILDREN
    private GameObject looker;

    private GameObject bow;

    private Animator anim;

    // Use this for initialization
    private void Start()
    {
        preserveUp = this.transform.up;

        jumpTimer = jumpTimerMax;
        jumpCooldownTimer = jumpCooldownTimerMax;
        arrowCooldownTimer = arrowCooldownTimerMax;

        looker = transform.GetChild(0).gameObject;
        bow = transform.GetChild(1).gameObject;

        arrowiter = 0;
        sworditer = 0;

        //Arrow = Arrows[arrowiter];
        Arrow = GameManager.instance.skillsManager.arrows[0];
        Sword = Swords[sworditer];

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (redirectedTimer > 0)
        {
            redirectedTimer -= Time.deltaTime;
            if (redirectedTimer <= 0)
                rigidbody2D.gravityScale = 4;
        }

        //MOVEMENT
        Look(looktarget);

        if (grounded)
            jumpCooldownTimer -= Time.deltaTime;

        if (movementEnabled)
        {
            if (!rigidbody2D.isKinematic)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                    anim.SetBool("move", true);
                else
                    anim.SetBool("move", false);

                Walk();                
            }

            if (Input.GetAxisRaw("Vertical") > 0 && jumpCooldownTimer <= 0.0f && jumpTimer > 0.0f && !jumplock && !rigidbody2D.isKinematic)
            {
                anim.SetBool("jump", true);
                Jump();
            }

            if (Input.GetAxisRaw("Vertical") == 0 && !grounded && !jumplock)
                jumplock = true;
        }

        if (combatEnabled)
        {
            SwitchWeaponCheck();

            AttackCheck();
        }
        this.transform.up = Vector2.up;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            grounded = true;
            jumpCooldownTimer = jumpCooldownTimerMax;
        }
        Entity entity = coll.gameObject.GetComponent<Entity>();
        if (entity != null)
        {
            if (redirectedTimer > 0 && usingSword)
            {
                OneTimeHit playerHit = gameObject.GetComponent<OneTimeHit>(), hit = entity.gameObject.AddComponent<OneTimeHit>();
                hit.damage = playerHit.damage;
                hit.critChance = playerHit.critChance;
                hit.critMult = playerHit.critMult;
                hit.enabled = true;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            grounded = true;
            jumplock = false;
            jumpTimer = jumpTimerMax;
            if (anim.GetBool("jump"))
                anim.SetBool("jump", false);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        grounded = false;
    }

    private void Look(Vector3 lookat)
    {
        Vector3 targpos;

        if (lookat == Vector3.zero)
            targpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else
            targpos = lookat;

        if (Sword == null || !Sword.GetComponent<Sword>().swinging)
        {
            if (targpos.x > transform.position.x)
                facingRight = true;
            else if (targpos.x < transform.position.x)
                facingRight = false;

            if (Sword != null)
                Sword.GetComponent<Sword>().ownerDirection = facingRight;
        }

        if (usingSword)
        {
            if (facingRight)
            {
                looker.transform.position = Vector3.Lerp(looker.transform.position,
                                                          looker.transform.parent.position + new Vector3(0.25f, 0, -0.5f),
                                                          (8 * Time.deltaTime));
            }
            else
            {
                looker.transform.position = Vector3.Lerp(looker.transform.position,
                                                          looker.transform.parent.position + new Vector3(-0.25f, 0, -0.5f),
                                                          (8 * Time.deltaTime));
            }
        }
        else
        {
            targpos -= transform.position;
            targpos.z = -0.5f;

            looker.transform.position =
                Vector3.Lerp(looker.transform.position,
                              looker.transform.parent.position + (targpos.normalized * 0.25f),
                             (8 * Time.deltaTime));
        }
    }

    private void Walk()
    {
        if (Input.GetKey(KeyCode.A))
            rigidbody2D.velocity = new Vector2(-1 * movementSpeed, this.gameObject.rigidbody2D.velocity.y);
        else if (Input.GetKey(KeyCode.D))
            rigidbody2D.velocity = new Vector2(1 * movementSpeed, this.gameObject.rigidbody2D.velocity.y);
        else
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x * dragCoef, rigidbody2D.velocity.y);
    }

    private void Jump()
    {
        if (jumpTimer == jumpTimerMax && grounded)
            rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x, jumpSpeed * firstJumpModifier);
        else if (jumpTimer < jumpTimerMax / 5)
        {
            rigidbody2D.velocity = new Vector2(this.gameObject.rigidbody2D.velocity.x, rigidbody2D.velocity.y + jumpSpeed * firstJumpModifier / 3);
            jumplock = true;
        }

        jumpTimer -= Time.deltaTime;
    }

    private void SwitchWeaponCheck()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            GetComponent<Player>().nextSphere();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            usingSword = !usingSword;

            if (usingSword)
            {
                bow.GetComponent<SpriteRenderer>().enabled = false;
                Sword = (GameObject)Instantiate(Swords[sworditer], transform.position, Quaternion.Euler(0, 0, -45));
                Sword.transform.parent = transform;
                Sword.transform.position = Sword.transform.parent.position + new Vector3(0.5f, 0.5f, -1f);
            }
            else
            {
                bow.GetComponent<SpriteRenderer>().enabled = true;
                Destroy(Sword);
                Sword = Swords[sworditer];
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
            PreviousArrow();
        else if (Input.GetKeyDown(KeyCode.E))
            NextArrow();

        //if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
        //{
        //    if (Input.GetKeyDown(KeyCode.Q))
        //    {
        //        if (usingSword)
        //        {
        //            --sworditer;

        //            if (sworditer < 0)
        //                sworditer = Swords.Length - 1;
        //        }
        //        else
        //        {
        //            --arrowiter;

        //            if (arrowiter < 0)
        //                arrowiter = Arrows.Length - 1;
        //        }
        //    }
        //    else if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        if (usingSword)
        //        {
        //            ++sworditer;

        //            if (sworditer >= Swords.Length)
        //                sworditer = 0;
        //        }
        //        else
        //        {
        //            ++arrowiter;

        //            if (arrowiter >= Arrows.Length)
        //                arrowiter = 0;
        //        }
        //    }
        //    Arrow = Arrows[arrowiter];

        //    if (usingSword)
        //    {
        //        Destroy(Sword);
        //        Sword = (GameObject)Instantiate(Swords[sworditer], transform.position, Quaternion.Euler(0, 0, -45));
        //        Sword.transform.parent = transform;
        //        Sword.transform.position = Sword.transform.parent.position + new Vector3(0.5f, 0.5f, -1f);
        //    }
        //}
    }

    private void AttackCheck()
    {
        if (!usingSword)
        {
            //FIRING ARROWS
            if (arrowCooldownTimer > 0.0f)
                arrowCooldownTimer -= Time.deltaTime;

            if (Input.GetMouseButton(0) && arrowCooldownTimer <= 0.0f)
            {
                anim.SetTrigger("ranged");

                Vector3 mousepos = GameObject.FindGameObjectWithTag("MainCamera").camera.ScreenToWorldPoint(Input.mousePosition);
                mousepos -= bow.transform.position;
                mousepos.z = 0;

                GameObject currArrow = (GameObject)Instantiate(Arrow.prefab, bow.transform.position + Vector3.back,
                                                                Quaternion.FromToRotation(preserveUp, mousepos));

                currArrow.rigidbody2D.velocity = mousepos.normalized * 7.5f;

                arrowCooldownTimer = arrowCooldownTimerMax;

                currArrow.GetComponent<Arrow>().owner = this.gameObject;
                if (gameObject.GetComponentInChildren<ShieldSphere>() != null)
                    currArrow.GetComponent<Arrow>().createdInsideShield = true;
                GameManager.instance.statsManager.shotsFired++;
            }
        }
        else
        {
            //SWORD CODE
            if (!Sword.GetComponent<Sword>().swinging)
            {
                Sword.GetComponent<Sword>().Follow();

                if (Input.GetMouseButton(0))
                {
                    Sword.GetComponent<Sword>().Swing();
                }
            }
        }
    }

    private void NextArrow()
    {
        List<SkillsManager.SetArrowPrefabs> tempArrows = GameManager.instance.skillsManager.arrows;
        if (!GameManager.instance.skillsManager.CheckArrowsEmpty())
        {
            int currentOne = tempArrows.IndexOf(Arrow);

            for (int i = currentOne + 1; i != currentOne; i++)
            {
                if (i == tempArrows.Count)
                    i = 0;

                if (tempArrows[i].active)
                {
                    Arrow = tempArrows[i];
                    break;
                }
            }
        }
    }

    private void PreviousArrow()
    {
        List<SkillsManager.SetArrowPrefabs> tempArrows = GameManager.instance.skillsManager.arrows;
        if (!GameManager.instance.skillsManager.CheckArrowsEmpty())
        {
            int currentOne = tempArrows.IndexOf(Arrow);

            for (int i = currentOne - 1; i != currentOne; i--)
            {
                if (i == -1)
                    i = tempArrows.Count - 1;
                //if (i == (tempArrows.Count - 1))
                //    i = 0;

                if (tempArrows[i].active)
                {
                    Arrow = tempArrows[i];
                    break;
                }
            }
        }
    }
}