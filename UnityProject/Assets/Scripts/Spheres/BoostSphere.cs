﻿using System.Collections;
using UnityEngine;

public class BoostSphere : BaseSphere
{
    public float DamageModifier = 10;
    public float VelocityModifier = 1.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (proj != null)
        {
            StatsManager.instance.arrowsBoosted++;
            proj.damageType.damage += DamageModifier;
            proj.owner = this.gameObject;
        }
        if (other.rigidbody2D != null)
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("EnterSphere");

            if (other.rigidbody2D.velocity.magnitude < 32)
            {
                other.rigidbody2D.velocity *= VelocityModifier;
                if (other.rigidbody2D.velocity.magnitude > 32)
                {
                    other.rigidbody2D.velocity = other.rigidbody2D.velocity.normalized * 32;
                }
            }
        }
    }

    public void SetOwner(Player owner)
    {
        Owner = owner;
    }
}