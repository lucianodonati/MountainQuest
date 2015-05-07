using UnityEngine;
using System.Collections;

public class AOE_Emitter : MonoBehaviour {

    public int numParticles;
    public float lifetimeVariance;

    protected ParticleSystem psys;
    protected ParticleSystem.Particle[] particles;
    protected int particleCount;
    protected int maxradius;

	// Use this for initialization
	protected virtual void Start () {
        psys = gameObject.GetComponent<ParticleSystem>();
        particles = new ParticleSystem.Particle[psys.maxParticles];

        psys.Emit(numParticles);

        particleCount = psys.GetParticles(particles);

        for(int currParticle = 0; currParticle < particleCount; ++currParticle)
        {
            particles[currParticle].lifetime += Random.Range(-lifetimeVariance, lifetimeVariance);

            particles[currParticle].rotation = Vector2.Angle(Vector2.up, (Vector2)particles[currParticle].velocity);

            if (particles[currParticle].velocity.x < 0)
                particles[currParticle].rotation *= -1;

            particles[currParticle].velocity *= (psys.startLifetime / particles[currParticle].lifetime);

            particles[currParticle].velocity *= 2;
        }

        psys.SetParticles(particles,particleCount);
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
