using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    private float deadLingerTimer;
    public float deadLingerTimerMax = 5.0f;
    private bool disableSeek = true;
    public GameObject lastPlatform;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        Movement_Coordinator movCoord = GetComponent<Movement_Coordinator>();

        if(movCoord != null)
        {
            if (movCoord.currentMovement.GetType() != System.Type.GetType("Idle_Movement"))
                GetComponent<Animator>().SetBool("move", true);
            else
                GetComponent<Animator>().SetBool("move", false);
        }

        Seek_Movement seek = gameObject.GetComponent<Seek_Movement>();
        if (seek != null)
            seek.enabled = disableSeek;

        if (dead)
        {
            if (GetComponent<Parasite>() == null)
            {
                deadLingerTimer -= Time.deltaTime;
            }

            if (deadLingerTimer <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public override void die()
    {
        base.die();
        Animator anim = GetComponentInParent<Animator>();
        if (anim != null)
            anim.SetBool("dead", true);

        //gameObject.GetComponent<SpriteRenderer>().color = new Color(myColor.r / 2, myColor.g / 2, myColor.b / 2, myColor.a);

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameObject emitter = (GameObject)Instantiate(new GameObject(), transform.position, transform.rotation);
        ExperienceParticles ep = emitter.AddComponent<ExperienceParticles>();
        ep.experience = experience;

        player.experience += experience;
        player.CheckForUpgrade();
        GameManager.instance.statsManager.enemiesKilledTotal++;

        bool AOEon = false;
        if (transform.FindChild("PreserveScale") != null)
        {
            if (transform.FindChild("PreserveScale").FindChild("ShatteringArrow(Clone)") != null ||
               transform.FindChild("PreserveScale").FindChild("ExplodingArrow(Clone)") != null)
                AOEon = true;
        }

        if (GetComponent<Parasite>() != null || AOEon)
        {
            if (GetComponent<Parasite>() != null)
            {
                GetComponent<Parasite>().currentDuration = 5.0f;
                ParticleSystem psys = GetComponent<ParticleSystem>();
                if (psys != null)
                    psys.emissionRate *= 4;
            }

            deadLingerTimer = deadLingerTimerMax;
        }

        MonoBehaviour[] components = GetComponents<MonoBehaviour>();

        foreach (Object c in components)
        {
            if (c.GetType() != System.Type.GetType("SpriteRenderer") &&
                c.GetType() != System.Type.GetType("ParticleSystem") &&
                c.GetType() != System.Type.GetType("Enemy") &&
                c.GetType() != System.Type.GetType("Parasite") &&
                c.GetType() != System.Type.GetType("Health") &&
                c.GetType() != System.Type.GetType("HealthBar"))
            {
                if (c.GetType() == System.Type.GetType("Collider2D") && (GetComponent<Parasite>() != null || AOEon))
                {
                    if (!AOEon && !((Collider2D)c).isTrigger)
                        continue;
                }

                ((MonoBehaviour)c).enabled = false;
            }
        }

        if (AOEon)
        {
            Renderer[] rends = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                if (!r.gameObject.name.Contains("Effect"))
                    r.enabled = false;

            Rigidbody2D[] bods = GetComponentsInChildren<Rigidbody2D>();
            foreach (Rigidbody2D b in bods)
            {
                b.isKinematic = false;
                b.velocity = Vector2.zero;
                b.drag = 100000;
                b.gravityScale = 0;
            }

            BoxCollider2D[] bxcols = GetComponentsInChildren<BoxCollider2D>();
            foreach (BoxCollider2D bx in bxcols)
                bx.enabled = false;
        }
    }

    public void setAI(bool set)
    {
        Movement_Coordinator coord = gameObject.GetComponent<Movement_Coordinator>();
        if (coord != null)
            coord.enabled = set;
        Enemy_Movement[] movementAI = gameObject.GetComponents<Enemy_Movement>();
        foreach (Enemy_Movement aiMovement in movementAI)
            aiMovement.enabled = set;

        AttackAI[] attackAI = GetComponents<AttackAI>();
        foreach (AttackAI attack in attackAI)
        {
            attack.enabled = set;
        }
        disableSeek = set;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
            lastPlatform = coll.gameObject;
    }
}