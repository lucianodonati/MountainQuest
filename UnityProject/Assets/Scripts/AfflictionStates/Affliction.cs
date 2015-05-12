using System.Collections;
using UnityEngine;

public class Affliction : DamageType
{
    public bool slow = false;
    public float ticEvery = 2.0f, currentDuration = 5.0f,initialDuration = 5.0f, timer = 0.0f;
    private float damageDealt = 0.0f;
    public bool particle = false, color = false;
    public Color changeColor;
    public OverrideEffect effect;

    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (slow == true)
        {
            Entity Slowed = GetComponent<Entity>();
            //  IsSlowed move = null;
            if (Slowed != null)
            {
                Slowed.isSlowed = true;
            }
        }
        
        if (currentDuration >= 0.0f)
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
            currentDuration -= Time.deltaTime;
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
        ParticleSystem pSys = GetComponent<ParticleSystem>();
        if (pSys != null)
            pSys.enableEmission = false;

        if (gameObject.GetComponent<Entity>() != null)
            gameObject.GetComponent<SpriteRenderer>().color = GetComponent<Entity>().myColor;
        //Debug.Log("Damage dealt (DPS Total): " + damageDealt);
    }
}