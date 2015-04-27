using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Health health;
    public bool isSlowed = false;
    // Use this for initialization
    protected virtual void Start()
    {
        gameObject.AddComponent<Health>();
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (health.currentHP <= 0.0f)
            die();
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
    }

    public virtual void die()
    {
        if (tag == "Enemy" || tag == "Boss")
            GameManager.instance.stats.enemiesKilledTotal++;

        Destroy(gameObject);
    }
}