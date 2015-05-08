using UnityEngine;
using System.Collections;

public class Parasite : Affliction {

    public float infectionChance;
    public float decayRate;
    public float germinationTimer;
    public float germinationTimerMax;

	// Use this for initialization
	protected override void Start () {
	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();

        if (germinationTimer > 0.0f)
            germinationTimer -= Time.deltaTime;
	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (germinationTimer <= 0.0f &&
                Random.Range(0.0f, 100.0f) <= infectionChance &&
                coll.gameObject.GetComponent<Parasite>() == null &&
                !coll.gameObject.GetComponent<Entity>().dead)
            {
                enabled = true;
                attachToEnemy(coll.gameObject.GetComponent<Entity>());
            }
        }
    }

    public override void attachToEnemy(Entity theOtherGuy)
    {
        base.attachToEnemy(theOtherGuy);
    }
}
