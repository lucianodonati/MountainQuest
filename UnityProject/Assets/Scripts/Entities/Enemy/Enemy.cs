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

        gameObject.GetComponent<SpriteRenderer>().color = new Color(myColor.r / 2, myColor.g / 2, myColor.b / 2, myColor.a);

        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.experience += experience;
        player.CheckForUpgrade();
        StatsManager.instance.enemiesKilledTotal++;

        if (GetComponent<Parasite>() != null)
        {
            GetComponent<Parasite>().currentDuration = 5.0f;
            GetComponent<ParticleSystem>().emissionRate *= 4;

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
                if (c.GetType() == System.Type.GetType("Collider2D") && GetComponent<Parasite>() != null)
                {
                    if (!((Collider2D)c).isTrigger)
                        continue;
                }

                ((MonoBehaviour)c).enabled = false;
            }
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