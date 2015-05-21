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
        //GetComponent<BoxCollider2D>().isTrigger = true;
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
        else if (owner != null && owner.name.Contains("Wave"))
            rigidbody2D.velocity = transform.up * speed;

        //rigidbody2D.position += rigidbody2D.velocity * Time.deltaTime;
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
                if (name.Contains("ExplodingArrow") || name.Contains("ShatteringArrow"))
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
                if ((name.Contains("ExplodingArrow") || name.Contains("ShatteringArrow")))
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

                if (other.gameObject.GetComponent<Entity>())
                {
                    if ((name.Contains("WindArrow") || name.Contains("LightningArrow")) && numCollisions >= 0)
                    {
                        numCollisions--;
                        GetComponent<BoxCollider2D>().isTrigger = true;

                        if (numCollisions == -1)
                            GetStuck(other);

                        if (name.Contains("LightningArrow"))
                            GetComponent<LightningArrow>().CreateLightningHitAnimation();
                    }

                    Entity isEntity = other.gameObject.GetComponent<Entity>();
                    if (isEntity != null)
                        damageType.attachToEnemy(isEntity);
                }

                if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Boss")
                    GameManager.instance.statsManager.shotsHit++;

                if ((!name.Contains("WindArrow") && !name.Contains("LightningArrow")) || (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Boss"))
                    GetStuck(other);
            }
        }
    }

    protected void GetStuck(Collider2D coll)
    {
        if (coll.tag != "Sphere" && transform.parent == null)
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            GetComponent<BoxCollider2D>().isTrigger = true;
            stuck = true;
            Transform dummyChildTransform = coll.transform.FindChild("PreserveScale");
            if (dummyChildTransform == null)
            {
                GameObject dummyChild = new GameObject();
                dummyChild.transform.localScale = new Vector3(1, 1, 1);
                dummyChild.transform.position = coll.transform.position;
                dummyChild.name = "PreserveScale";
                if (coll.tag == "Platform")
                    dummyChild.transform.parent = coll.transform.parent;
                else
                    dummyChild.transform.parent = coll.transform;
                dummyChildTransform = dummyChild.transform;
                transform.parent = dummyChildTransform;
                rigidbody2D.isKinematic = true;
            }
        }
    }
}