using UnityEngine;
using System.Collections;

public class HealAI : AttackAI {

    private Health health;
    private ParticleSystem psys;

    public float healAmount;
    public GameObject particleEmitter;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        GameObject emitter = (GameObject)Instantiate(particleEmitter, transform.position, transform.rotation);
        emitter.transform.parent = transform;
        psys = emitter.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
    protected override void Update()
    {
        if(health == null)
            health = GetComponent<Health>();

        bool notAggro = false;

        if (coordinator.OnSightMovement != "Seek_Movement")
        {
            if (coordinator.nonseekAggroTimerMax <= 0.0f)
                notAggro = true;
        }
        else
        {
            if (coordinator.currentMovement.GetType() != System.Type.GetType("Seek_Movement"))
                notAggro = true;
        }

        if (notAggro)
        {
            if (reloadTimer > 0.0f)
                reloadTimer -= Time.deltaTime;
            else if (health.currentHP < health.maxHP)
            {
                health.Heal(healAmount);
                psys.Emit((int)healAmount);
                reloadTimer = reloadTimerMax;
            }
        }
        else
            reloadTimer = reloadTimerMax;
    }
}
