using System.Collections;
using UnityEngine;

public class WaveSphere : BaseSphere
{
    public float DamageModifier = 10;
    public int arrowsPerWave = 20;

    private int arrowsCreated;

    private int timesHit;
    public int maxTimesHit = 5;
    public float timesHitAOEDivider = 2;

    private float instabilityTimer;
    private float instabilityTimerMax = 3;

    public bool destabilizeViolently = false;
    public float destabilizeArrowMultiplier = 4;

    public float minAngVel = 180.0f;
    public float maxAngVel = 360.0f;

    private Vector3 posSaver;

    private void Awake()
    {
    }

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        posSaver = transform.position;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (instabilityTimer >= 0.0f)
        {
            instabilityTimer -= Time.deltaTime;
            transform.position =
                posSaver + ((Vector3)Random.insideUnitCircle * (arrowsCreated * (instabilityTimer / instabilityTimerMax) / 100));
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
            if (proj != null && !proj.stuck)
            {
                if (proj.owner != this.gameObject)
                {
                    proj.damageType.damage += DamageModifier;
                    proj.owner = this.gameObject;

                    SoundFX sfx = GetComponent<SoundFX>();
                    if (sfx != null)
                        sfx.Play("Wave");

                    float currangle = 0, angleIncrement = 360.0f / (float)arrowsPerWave;

                    if (timesHit <= ((other.GetComponent<AOE>() != null) ? (int)(maxTimesHit / timesHitAOEDivider) : maxTimesHit))
                    {
                        for (int i = 0; i < arrowsPerWave; ++i)
                        {
                            GameObject currArrow =
                                (GameObject)Instantiate(other.gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, currangle)));
                            currArrow.GetComponent<Arrow>().owner = this.gameObject;
                            currangle += angleIncrement;
                        }

                        if (instabilityTimer <= 0.0f)
                        {
                            arrowsCreated = arrowsPerWave;
                            timesHit = 1;
                        }
                        else
                        {
                            arrowsCreated += arrowsPerWave;
                            ++timesHit;
                        }

                        instabilityTimer = instabilityTimerMax;
                    }

                    if (timesHit >= ((other.GetComponent<AOE>() != null) ? (int)(maxTimesHit / timesHitAOEDivider) : maxTimesHit))
                    {
                        sfx.Play("Destabilize");

                        if (destabilizeViolently)
                        {
                            currangle = 0;
                            angleIncrement = 360 / ((float)arrowsPerWave * 4.0f);

                            for (int i = 0; i < (int)(arrowsPerWave * destabilizeArrowMultiplier); ++i)
                            {
                                GameObject currArrow =
                                    (GameObject)Instantiate(other.gameObject, transform.position, Quaternion.Euler(new Vector3(0, 0, currangle)));
                                currArrow.GetComponent<Arrow>().owner = this.gameObject;

                                Color newColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);

                                currArrow.GetComponent<SpriteRenderer>().color = newColor;

                                if (currArrow.GetComponent<ParticleSystem>() != null)
                                    currArrow.GetComponent<ParticleSystem>().startColor = newColor;

                                currArrow.rigidbody2D.gravityScale = Random.Range(0.0f, 5.0f);

                                currArrow.rigidbody2D.velocity =
                                    currArrow.rigidbody2D.velocity.normalized
                                        * Random.Range(currArrow.rigidbody2D.velocity.magnitude / 10, 4.0f * currArrow.rigidbody2D.velocity.magnitude);

                                currArrow.rigidbody2D.angularVelocity = Random.Range(minAngVel, maxAngVel);

                                currangle += angleIncrement;
                            }
                        }
                        if (Owner != null)
                            (Owner as Player).RemoveOSphere();
                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}