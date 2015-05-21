using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int experience;
    public Health health;
    public float maxHealth = 100.0f;

    public bool isSlowed = false, isStunned = false;

    public Color myColor;

    public bool showHealth = true;

    //death vars
    public bool dead = false;

    private MonoBehaviour movement;

    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.AddComponent<Health>();
        health = GetComponent<Health>();
        health.currentHP = health.maxHP = maxHealth;
        health.showHealthBar = showHealth;

        myColor = gameObject.GetComponent<SpriteRenderer>().color;

        if (tag == "Player")
            movement = GetComponent<PlayerController>();
        else if (tag == "Enemy")
        {
            movement = GetComponent<Movement_Coordinator>();

            if (movement == null)
                movement = GetComponent<Enemy_Movement>();
        }
        if (movement != null)
            Debug.Log(name + " " + movement.GetType().ToString());
        else
            Debug.LogError(name + " DERP");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.up = Vector2.up;

        if (!dead)
        {
            if (health.currentHP <= 0.0f)
                die();

            if (isSlowed)
            {
                rigidbody2D.velocity /= 2;
                isSlowed = false;
            }

            if (movement != null)
            {
                Vector3 scaleflip = new Vector3(1, 1, 1);

                if (movement.GetType() == System.Type.GetType("PlayerController"))
                {
                    if (((PlayerController)movement).facingRight)
                        scaleflip.x *= -1;
                }
                else if (movement.GetType() == System.Type.GetType("Movement_Coordinator"))
                {
                    if (((Movement_Coordinator)movement).currentMovement.direction)
                        scaleflip.x *= -1;
                }
                else
                {
                    if (((Enemy_Movement)movement).direction)
                        scaleflip.x *= -1;
                }

                transform.localScale = scaleflip;
            }
        }
    }

    public void TakeTamage(OneTimeHit type)
    {
        OneTimeHit hit = gameObject.AddComponent<OneTimeHit>();
        hit.enabled = true;
        hit.critChance = type.critChance;
        hit.damage = type.damage;
        hit.critMult = type.critMult;
    }

    public void TakeTamage(Affliction type)
    {
        Affliction aff;

        if (type.GetType() != System.Type.GetType("Parasite"))
            aff = gameObject.AddComponent<Affliction>();
        else
            aff = gameObject.AddComponent<Parasite>();

        aff.enabled = true;
        aff.damage = type.damage;
        aff.currentDuration = aff.initialDuration = type.initialDuration;
        aff.ticEvery = type.ticEvery;
        aff.slow = type.slow;
        aff.particle = type.particle;

        if (aff.GetType() == System.Type.GetType("Parasite"))
        {
            ((Parasite)aff).infectionChance = ((Parasite)type).infectionChance - ((Parasite)type).decayRate;
            ((Parasite)aff).decayRate = ((Parasite)type).decayRate;
            ((Parasite)aff).germinationTimerMax = ((Parasite)aff).germinationTimer = ((Parasite)type).germinationTimerMax;
        }

        if (aff.particle)
        {
            ParticleSystem pSys = GetComponent<ParticleSystem>();
            if (pSys != null)
            {
                pSys.enableEmission = true;
                pSys.startColor = type.gameObject.GetComponent<ParticleSystem>().startColor;
                GetComponent<ParticleSystemRenderer>().material = type.gameObject.GetComponent<ParticleSystemRenderer>().material;
            }
        }
        if (type.effect != null)
        {
            if (type.effect is Panic)
            {
                aff.effect = type.effect;
                Panic effect = gameObject.AddComponent<Panic>(), other = type.effect as Panic;
                effect.duration = aff.currentDuration;
                effect.faceOtherSideEvery_Max = other.faceOtherSideEvery_Max;
                effect.faceOtherSideEvery_Min = other.faceOtherSideEvery_Min;
                effect.speed_Max = other.speed_Max;
                effect.speed_Min = other.speed_Min;
            }
        }
        if (type.color)
        {
            aff.color = type.color;
            aff.changeColor = type.changeColor;
            myColor = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = type.changeColor;
        }
    }

    public virtual void die()
    {
        SoundFX sfx = GetComponent<SoundFX>();
        if (sfx != null)
            sfx.Play("Die");

        dead = true;
    }
}