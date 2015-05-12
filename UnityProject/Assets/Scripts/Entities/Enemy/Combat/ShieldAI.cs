using UnityEngine;
using System.Collections;

public class ShieldAI : AttackAI {

    public GameObject shield;
    private GameObject currShield;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {

        if(GetComponent<Health>().currentHP <= GetComponent<Health>().maxHP/2)
        {
            if (currShield != null)
                reloadTimer = reloadTimerMax;
            else
            {
                reloadTimer -= Time.deltaTime;

                if(reloadTimer <= 0.0f)
                {
                    currShield = (GameObject)Instantiate(shield, transform.position + Vector3.forward, transform.rotation);
                    currShield.GetComponent<DELETEMESHIELD>().owner = transform;
                }
            }
        }
	}
}
