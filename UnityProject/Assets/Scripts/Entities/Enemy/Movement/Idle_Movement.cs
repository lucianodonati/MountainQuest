using UnityEngine;
using System.Collections;

public class Idle_Movement : Enemy_Movement {

    float turntimer;
    public float turnTimerMax = 2f;
    public float turnTimerError = 0.5f;

	// Use this for initialization
	protected override void Start () {
        preserveUp = transform.up;
        turntimer = turnTimerMax;
	}
	
	// Update is called once per frame
	protected override void Update () {

        turntimer -= Time.deltaTime;

        if(turntimer <= 0.0f)
        {
            direction = !direction;

            turntimer = turnTimerMax + Random.Range(-turnTimerError, turnTimerError);
        }

        transform.up = preserveUp;
	}
}
