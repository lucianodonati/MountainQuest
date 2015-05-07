using UnityEngine;
using System.Collections;

public class Idle_Movement : Enemy_Movement {

    float turntimer;
    public float turnTimerMax = 2f;
    public float turnTimerError = 0.5f;

    public LayerMask layerMask;

	// Use this for initialization
	protected override void Start () {
        preserveUp = transform.up;
        turntimer = turnTimerMax;
	}
	
	// Update is called once per frame
	protected override void Update () {

        turntimer -= Time.deltaTime;

        if (turntimer <= 0.0f && !InFOV(GameObject.FindGameObjectWithTag("Player")))
        {
            direction = !direction;

            turntimer = turnTimerMax + Random.Range(-turnTimerError, turnTimerError);
        }
        else if (InFOV(GameObject.FindGameObjectWithTag("Player")))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player.transform.position.x > transform.position.x)
                direction = true;
            else if (player.transform.position.x < transform.position.x)
                direction = false;
        }

        transform.up = preserveUp;
	}

    public bool InFOV(GameObject targ)
    {
        bool val = false;

        RaycastHit2D checkFOV =
            Physics2D.Linecast(transform.position, targ.transform.position, layerMask);

            if (checkFOV.collider != null)
                if (checkFOV.collider.transform == targ.transform)
                    val = true;

        Debug.DrawLine(transform.position, checkFOV.point);

        return val;
    }
}
