using System.Collections;
using UnityEngine;

public class OneTimeHit : DamageType
{
    public float critChance = 0.2f, critMult = 2.0f;

    private void Update()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            Attack();
            Destroy(this);
        }
    }

    private void Attack()
    {
        Health attackedHealth = getAttackedUnityHealth();
        if (attackedHealth != null)
        {
            if (Random.Range(0.0f, 1.0f) <= critChance)
                attackedHealth.TakeDamage(damage * critMult, true);
            else
                attackedHealth.TakeDamage(damage, false);
        }
    }

    public override void attachToEnemy(Entity theOtherGuy)
    {
        theOtherGuy.TakeTamage(this);
    }
}