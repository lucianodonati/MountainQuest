using UnityEngine;
using System.Collections;

public class WaveSphere : MonoBehaviour {

    public float DamageModifier = 10;
    public float AliveTimer = 7;
    public Player Owner;
    public int arrowsPerWave = 20;

    private int arrowsCreated;
    public int MaxArrowsCreated = 100;

    private float instabilityTimer;
    private float instabilityTimerMax = 3;

    public bool destabilizeViolently = false;

    Vector3 posSaver;

	// Use this for initialization
	void Start () {
        posSaver = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        if (AliveTimer != -1)
        {
            AliveTimer -= Time.deltaTime;
            if (AliveTimer <= 0)
            {
                if (Owner != null && Owner.GetComponent<Player>() != null)
                    Owner.GetComponent<Player>().RemoveBSphere();
                Destroy(this.gameObject);
            }
        }

        if(instabilityTimer >= 0.0f)
        {
            instabilityTimer -= Time.deltaTime;
            transform.position = posSaver + ((Vector3)Random.insideUnitCircle * (arrowsCreated * (instabilityTimer / instabilityTimerMax)/100));
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool notAOE = true;

        if (other.GetComponent<AOE>() != null)
            if (other.GetComponent<AOE>().enabled)
                notAOE = false;

        if (notAOE)
        {
            Arrow proj = other.GetComponent<Arrow>();
            if (proj != null)
            {
                if (proj.owner != this.gameObject)
                {
                    proj.damageType.damage += DamageModifier;
                    proj.owner = this.gameObject;

                    SoundFX sfx = GetComponent<SoundFX>();
                    if (sfx != null)
                        sfx.Play("Poop");

                    float currangle = 0, angleIncrement = 360.0f / (float)arrowsPerWave;

                    if (arrowsCreated + arrowsPerWave <= MaxArrowsCreated)
                    {
                        for (int i = 0; i < arrowsPerWave; ++i)
                        {
                            GameObject currArrow =
                                (GameObject)Instantiate(other.gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, currangle)));
                            currArrow.GetComponent<Arrow>().owner = this.gameObject;
                            currangle += angleIncrement;
                        }

                        if (instabilityTimer <= 0.0f)
                            arrowsCreated = arrowsPerWave;
                        else
                            arrowsCreated += arrowsPerWave;

                        instabilityTimer = instabilityTimerMax;
                    }

                    if (arrowsCreated >= MaxArrowsCreated)
                    {
                        sfx.Play("Destabilize");

                        if (destabilizeViolently)
                        {
                            currangle = 0;
                            angleIncrement = 360 / ((float)arrowsPerWave * 4.0f);

                            for (int i = 0; i < arrowsPerWave * 4; ++i)
                            {
                                GameObject currArrow =
                                    (GameObject)Instantiate(other.gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, currangle)));
                                currArrow.GetComponent<Arrow>().owner = this.gameObject;

                                Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);

                                currArrow.GetComponent<SpriteRenderer>().color = newColor;

                                if (currArrow.GetComponent<ParticleSystem>() != null)
                                {
                                    currArrow.GetComponent<ParticleSystem>().startColor = newColor;
                                    //currArrow.GetComponent<ParticleSystem>().emissionRate *= 32;
                                    //currArrow.GetComponent<ParticleSystem>().startLifetime /= 16;
                                }

                                currArrow.rigidbody2D.gravityScale = Random.Range(0.0f, 5.0f);

                                currArrow.rigidbody2D.velocity =
                                    currArrow.rigidbody2D.velocity.normalized
                                        * Random.Range(currArrow.rigidbody2D.velocity.magnitude / 10, 4.0f * currArrow.rigidbody2D.velocity.magnitude);

                                currArrow.rigidbody2D.angularVelocity = Random.Range(180.0f, 360.0f);

                                currangle += angleIncrement;
                            }
                        }

                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
