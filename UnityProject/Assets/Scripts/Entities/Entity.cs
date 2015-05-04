﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int experience;
    public Health health;
    public bool isSlowed = false;
    public Color myColor;

    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.AddComponent<Health>();
        health = GetComponent<Health>();
        myColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health.currentHP <= 0.0f)
            die();

        if (isSlowed)
        {
            rigidbody2D.velocity /= 2;
            isSlowed = false;
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
        Affliction aff = gameObject.AddComponent<Affliction>();
        aff.enabled = true;
        aff.damage = type.damage;
        aff.duration = type.duration;
        aff.ticEvery = type.ticEvery;
        aff.slow = type.slow;
        aff.particle = type.particle;
        if (aff.particle)
        {
            ParticleSystem pSys = GetComponent<ParticleSystem>();
            if (pSys != null)
            {
                pSys.enableEmission = true;
                pSys.startColor = type.gameObject.GetComponent<ParticleSystem>().startColor;
            }
        }
        if (type.effect != null)
        {
            if (type.effect is Panic)
            {
                aff.effect = type.effect;
                Panic effect = gameObject.AddComponent<Panic>(), other = type.effect as Panic;
                effect.duration = aff.duration;
                effect.faceOtherSideEvery_Max = other.faceOtherSideEvery_Max;
                effect.faceOtherSideEvery_Min = other.faceOtherSideEvery_Min;
                effect.speed_Max = other.speed_Max;
                effect.speed_Min = other.speed_Min;
            }
        }
        else
        {
            if (type.color)
            {
                myColor = gameObject.GetComponent<SpriteRenderer>().color;
                gameObject.GetComponent<SpriteRenderer>().color = type.changeColor;
            }
        }
    }

    public virtual void die()
    {
        SoundFX sfx = GetComponent<SoundFX>();
        if (sfx != null)
            sfx.Play("Die");
        if (tag == "Enemy" || tag == "Boss")
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.experience += experience;
            player.CheckForUpgrade();
            GameManager.instance.stats.enemiesKilledTotal++;
        }

        Destroy(gameObject);
    }
}