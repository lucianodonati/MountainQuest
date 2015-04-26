using System.Collections;
using UnityEngine;

public class Affliction : DamageType
{
    public float slow = 0.0f;
    public float ticEvery = 2.0f, duration = 5.0f, timer = 0.0f;
    private float damageDealt = 0.0f;

    // Update is called once per frame
    private void Update()
    {
        if (duration >= 0.0f)
        {
            if (timer <= 0.0f)
            {
                timer = ticEvery;
                Health attackedHealth = getAttackedUnityHealth();
                if (attackedHealth != null)
                {
                    attackedHealth.TakeDamage(damage, false);
                    damageDealt += damage; // Comment the line on OnDestroy to debug how much damage dps does.
                }
            }
            timer -= Time.deltaTime;
            duration -= Time.deltaTime;
        }
        else
            Destroy(this);
    }

    public override void attachToEnemy(Entity theOtherGuy)
    {
        theOtherGuy.TakeTamage(this);
    }

    private void OnDestroy()
    {
        //Debug.Log("Damage dealt (DPS Total): " + damageDealt);
    }
}