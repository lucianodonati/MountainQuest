using System.Collections;
using UnityEngine;

public abstract class DamageType : MonoBehaviour
{
    public float damage = 0.0f;

    private void Awake()
    {
        enabled = false;
    }

    protected Health getAttackedUnityHealth()
    {
        Entity attacked = GetComponent<Entity>();
        Health attackedHealth = null;
        if (attacked != null)
        {
            attackedHealth = attacked.health;
            if (attackedHealth == null)
                Debug.LogWarning("Trying to access health but entity has no health.");
        }
        else
            Debug.LogWarning("Trying to access Entity but the parent is not an entity.");

        return attackedHealth;
    }

    public abstract void attachToEnemy(Entity theOtherGuy);
}