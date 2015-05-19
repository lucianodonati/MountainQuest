﻿using UnityEngine;
using System.Collections;

public class ExperienceOrb : MonoBehaviour {

    private float changeTimer;
    public float changeTimerMax;

    private Vector3 newAngVeloc;
    public float rotationSpeedMax = 2.5f;

    ParticleSystem psys;

	// Use this for initialization
	void Start () {
        renderer.material.renderQueue = 1;
        psys = GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {

        if (changeTimer > 0.0f)
            changeTimer -= Time.deltaTime;
        else
        {
            newAngVeloc = new Vector3(Random.Range(-rotationSpeedMax,rotationSpeedMax),
                                      Random.Range(-rotationSpeedMax,rotationSpeedMax),
                                      Random.Range(-rotationSpeedMax,rotationSpeedMax));
            changeTimer = changeTimerMax;
        }

        if ((rigidbody.angularVelocity + (newAngVeloc * Time.deltaTime)).magnitude < 2)
            rigidbody.angularVelocity += newAngVeloc * Time.deltaTime;
        else
            rigidbody.angularVelocity = Vector3.ClampMagnitude(rigidbody.angularVelocity, 2);

        float modif = (Mathf.Abs(Mathf.Sin(Time.time/16)) * 0.25f);

        transform.localScale = new Vector3(0.25f + modif, 0.25f + modif, 0.25f + modif);
	}
}
