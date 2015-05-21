using System.Collections;
using UnityEngine;

public class ExperienceParticles : MonoBehaviour
{
    private ParticleSystem psys;
    private ParticleSystem.Particle[] particles;
    private int particlecount;

    [HideInInspector]
    public int experience;

    private float hangTime;
    public float hangTimeMax = 1.0f;
    public float particleHangTimeSpeed = 1.0f;

    // Use this for initialization
    private void Start()
    {
        psys = gameObject.AddComponent<ParticleSystem>();
        psys.simulationSpace = ParticleSystemSimulationSpace.World;
        psys.emissionRate = 0;
        psys.startColor = new Color(205.0f / 255.0f, 208.0f / 255.0f, 8.0f / 255.0f, 255.0f / 255.0f);
        psys.startLifetime = 20.0f;

        psys.startSize = 0.1f;
        psys.Emit(experience);

        particles = new ParticleSystem.Particle[psys.maxParticles];

        hangTime = hangTimeMax;
    }

    // Update is called once per frame
    private void Update()
    {
        particlecount = psys.GetParticles(particles);

        if (particlecount > 0)
        {
            if (hangTime > 0.0f)
                hangTime -= Time.deltaTime;

            for (int i = 0; i < particlecount; ++i)
            {
                if (hangTime > 0.0f)
                    particles[i].velocity = (particles[i].position - transform.position).normalized * (particleHangTimeSpeed * (hangTime / hangTimeMax));
                else
                {
                    if ((particles[i].position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude > 2.0f)
                        particles[i].velocity =
                            (GameObject.FindGameObjectWithTag("Player").transform.position - particles[i].position) * 4;
                    else
                        particles[i].lifetime = 0;
                }
            }
        }
        else
            Destroy(this.gameObject);

        psys.SetParticles(particles, particlecount);
    }
}