using System.Collections;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    private ParticleSystem psys;
    private ParticleSystem.Particle[] finalparticles;
    private int numparticles;

    private bool touched = false;

    private float dieTimer;
    public float dieTimerMax = 5f;

    // Use this for initialization
    private void Start()
    {
        psys = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        //if (!touched)
        //   transform.Rotate(new Vector3(0, 70 * Time.deltaTime, 0));
        //else
        //    transform.rotation = Quaternion.identity;

        if (touched)
        {
            dieTimer -= Time.deltaTime;
            if (dieTimer <= 0.0f)
                Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        if (touched)
        {
            numparticles = psys.GetParticles(finalparticles);

            if (dieTimer <= 2 * dieTimerMax / 3)
            {
                for (int i = 0; i < numparticles; ++i)
                {
                    Vector3 to =
                        (GetComponent<NotifyDoor>().attachedDoor.transform.position - finalparticles[i].position);

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
                        finalparticles[i].position = GetComponent<NotifyDoor>().attachedDoor.transform.position;
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

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player" && !touched)
        {
            Destroy(gameObject.collider2D);
            Destroy(gameObject.rigidbody2D);

            psys.startSpeed = 10;
            psys.Emit(100);
            psys.simulationSpace = ParticleSystemSimulationSpace.World;

            dieTimer = dieTimerMax;
            touched = true;

            finalparticles = new ParticleSystem.Particle[psys.maxParticles];

            numparticles = psys.GetParticles(finalparticles);

            for (int i = 0; i < numparticles; ++i)
            {
                int modif = Random.Range(1, 3);

                finalparticles[i].velocity =
                        new Vector3(finalparticles[i].velocity.x * modif, finalparticles[i].velocity.y * modif, finalparticles[i].velocity.z);
            }
            psys.SetParticles(finalparticles, numparticles);
        }
    }
}