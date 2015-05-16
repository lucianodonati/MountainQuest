using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float currentHP = 100.0f, maxHP = 100.0f;
    public bool showHealthBar;
    public HealthBar healthBar;

    // Use this for initialization
    private void Start()
    {
        gameObject.AddComponent<HealthBar>();
        healthBar = gameObject.GetComponent<HealthBar>();
        healthBar.show = showHealthBar;
    }

    public void TakeDamage(float _damage, bool crit)
    {
        //Debug.Log("Health: Damage received (" + _damage + ", " + (crit ? "crit" : "no crit") + ")");
        if (_damage >= 0)
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("Hit");
            if (tag == "Player")
                StatsManager.instance.damageTaken += _damage;
            else if (tag == "Enemy" || tag == "Boss")
                StatsManager.instance.damageDealt += _damage;

            currentHP -= _damage;
            if (currentHP < 0.0f)
                currentHP = 0.0f;

            healthBar.CreateDamageLabel(_damage, false, crit);
        }
        else
            Debug.LogWarning(gameObject.name + ": Trying to deal negative damage (Use heal function).");
    }

    public void Heal(float _hp)
    {
        if (_hp >= 0)
        {
            currentHP += _hp;
            if (currentHP > maxHP)
                currentHP = maxHP;
            healthBar.CreateDamageLabel(_hp, true, false);
        }
        else
            Debug.LogWarning(gameObject.name + ": Trying to heal negative hp (Use TakeDamage function).");
    }
}