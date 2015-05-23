using UnityEngine;
using System.Collections;

public class KeyParticles : MonoBehaviour {

    private ParticleSystem psys;
    private ParticleSystem.Particle[] finalparticles;
    private int numparticles;

    private float dieTimer;
    private float dieTimerMax = 5f;

	// Use this for initialization
	void Start () {
        psys = gameObject.GetComponent<ParticleSystem>();

        psys.startSpeed = 10;
        psys.Emit(100);
        psys.simulationSpace = ParticleSystemSimulationSpace.World;

        dieTimer = dieTimerMax;

        finalparticles = new ParticleSystem.Particle[psys.maxParticles];

        numparticles = psys.GetParticles(finalparticles);

        if (numparticles <= 0)
            Destroy(gameObject);
        else
        {
            for (int i = 0; i < numparticles; ++i)
            {
                int modif = Random.Range(1, 3);

                finalparticles[i].velocity =
                        new Vector3(finalparticles[i].velocity.x * modif, finalparticles[i].velocity.y * modif, finalparticles[i].velocity.z);
            }
            psys.SetParticles(finalparticles, numparticles);
        }
	}
	
	// Update is called once per frame
	void Update () {
        numparticles = psys.GetParticles(finalparticles);

        if (dieTimer <= 2 * dieTimerMax / 3)
        {
            for (int i = 0; i < numparticles; ++i)
            {
                Vector3 to =
                    (GetComponent<DoorRequirement>().door.transform.position - finalparticles[i].position);

                to.z = 0;

                if (to.magnitude > 1)
                {
                    to = to.normalized * (dieTimerMax / dieTimer);
                    finalparticles[i].velocity =
                        new Vector3(finalparticles[i].velocity.x + to.x,
                                    finalparticles[i].velocity.y + to.y,
                                    psys.transform.position.z);
                }
                else
                {
                    finalparticles[i].position = GetComponent<DoorRequirement>().door.transform.position;
                    finalparticles[i].velocity = Vector3.zero;
                }
            }
        }
        else
        {
            for (int i = 0; i < numparticles; ++i)
            {
                finalparticles[i].velocity =
                    new Vector3(Mathf.SmoothStep(finalparticles[i].velocity.x, 0, Time.deltaTime),
                                Mathf.SmoothStep(finalparticles[i].velocity.y, 0, Time.deltaTime),
                                psys.transform.position.z);
            }
        }

        psys.SetParticles(finalparticles, numparticles);
	}
}
