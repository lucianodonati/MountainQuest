using UnityEngine;
using System.Collections;

public class FireAOE_Emitter : AOE_Emitter {

    public float angleCorrectionSpeed;

	// Use this for initialization
	protected override void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	protected override void Update () {

        particleCount = psys.GetParticles(particles);

        for (int currParticle = 0; currParticle < particleCount; ++currParticle)
        {
            if (particles[currParticle].lifetime <= psys.startLifetime / 2.0f)
            {
                particles[currParticle].rotation = Mathf.Lerp(particles[currParticle].rotation, 0, angleCorrectionSpeed * Time.deltaTime);
                particles[currParticle].velocity += -(Vector3)Physics2D.gravity * Time.deltaTime;
            }

        }

        psys.SetParticles(particles, particleCount);
	}
}
