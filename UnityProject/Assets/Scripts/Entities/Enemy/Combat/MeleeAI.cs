using UnityEngine;
using System.Collections;

public class MeleeAI : AttackAI {

    public float range;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {
        if (GetComponent<Entity>().isStunned == false)
        {
            if (!weapon.GetComponent<Sword>().swinging)
                weapon.GetComponent<Sword>().ownerDirection = GetComponent<Movement_Coordinator>().currentMovement.direction;

            if (reloadTimer > 0.0f)
                reloadTimer -= Time.deltaTime;

            if (reloadTimer <= 0.0f)
                AttackCheck();
        }
	}

    private void AttackCheck()
    {
        //SWORD CODE
        if (!weapon.GetComponent<Sword>().swinging)
        {

            weapon.GetComponent<Sword>().Follow();

            if (((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < range) &&
                Random.Range(1, 3) < 2 &&
                !weapon.GetComponent<Sword>().swinging)
            {
                weapon.GetComponent<Sword>().Swing();
                reloadTimer = reloadTimerMax;
            }
        }
    }
}
