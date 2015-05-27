using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject owner;

    public float speed;
    public float stuckTimer = 5;
    public int numCollisions = 0;
    public DamageType damageType;

    public bool stuck = false;

    public bool createdInsideShield = false;

    //AOE Emitter
    public GameObject AOE_Emitter;

    //screenshake variables
    public float OnAOEShakeAmount;

    public float OnAOEDampeningAmount;

    // Use this for initialization
    protected virtual void Start()
    {
        SoundFX sfx = GetComponent<SoundFX>();
        if (sfx != null)
            sfx.Play("Fire");
        rigidbody2D.velocity = transform.up * speed;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (stuck)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
                Destroy(gameObject);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != owner && !other.isTrigger)
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("Hit");

            if (stuck)
            {
                if (name.Contains("Exploding") || name.Contains("Shattering"))
                {
                    CircleCollider2D circle = GetComponent<CircleCollider2D>();
                    if (circle.enabled &&
                        (other.tag == "Enemy" || other.tag == "Boss") &&
                        other.Equals((Collider2D)other.gameObject.GetComponent<BoxCollider2D>()))
                    {
                        Entity isEntity = other.gameObject.GetComponent<Entity>();
                        if (isEntity != null)
                            damageType.attachToEnemy(isEntity);
                    }
                }
            }
            else
            {
                if ((name.Contains("Exploding") || name.Contains("Shattering")))
                {
                    if (!GetComponent<AOE>().enabled)
                    {
                        if (AOE_Emitter != null)
                        {
                            GameObject emitter = (GameObject)Instantiate(AOE_Emitter, transform.position, transform.rotation);
                            emitter.transform.parent = transform;
                        }

                        Camera.main.gameObject.GetComponent<CameraBehavior>().BeginShake(OnAOEShakeAmount, OnAOEDampeningAmount);

                        GetComponent<AOE>().enabled = true;
                        GetComponent<CircleCollider2D>().enabled = true;
                    }
                }

                Entity isEntity = other.gameObject.GetComponent<Entity>();
                if (isEntity)
                {
                    if (name.Contains("Wind") && numCollisions >= 0)
                    {
                        numCollisions--;

                        if (numCollisions <= 0)
                            GetStuck(other);

                        if (name.Contains("Lightning"))
                            GetComponent<LightningArrow>().CreateLightningHitAnimation();
                    }
                    damageType.attachToEnemy(isEntity);
                }

                if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
                    GameManager.instance.statsManager.shotsHit++;

                if (!name.Contains("Wind") ||
                    (name.Contains("Wind") && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Boss" && other.gameObject.tag != "Target"))
                    GetStuck(other);
            }
        }
    }

    protected void GetStuck(Collider2D coll)
    {
        if (coll.tag != "Sphere" && transform.parent == null)
        {
            stuck = true;
            Transform dummyChildTransform = coll.transform.FindChild("PreserveScale");
            if (dummyChildTransform == null)
            {
                GameObject dummyChild = new GameObject();
                dummyChild.transform.localScale = new Vector3(1, 1, 1);
                dummyChild.transform.position = coll.transform.position;
                dummyChild.name = "PreserveScale";
                dummyChild.transform.parent = coll.transform;
                dummyChildTransform = dummyChild.transform;
            }
            transform.parent = dummyChildTransform;
            rigidbody2D.velocity = new Vector2(0, 0);
            rigidbody2D.isKinematic = true;
        }
    }
}