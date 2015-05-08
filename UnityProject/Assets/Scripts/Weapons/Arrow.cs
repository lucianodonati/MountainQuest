using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float stuckTimer = 5;
    private bool stuck = false, justFired = true;
    public int numCollisions = 0;
    public DamageType damageType;

    //AOE Emitter
    public GameObject AOE_Emitter;

    //screenshake variables
    public float OnAOEShakeAmount;
    public float OnAOEDampeningAmount;

    // Use this for initialization
    private void Start()
    {
        SoundFX sfx = GetComponent<SoundFX>();
        //if (sfx != null)
        //    sfx.Play("Fire");
        rigidbody2D.velocity = transform.up * speed;
        //GetComponent<BoxCollider2D>().isTrigger = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (stuck)
        {
            stuckTimer -= Time.deltaTime;
            if (stuckTimer <= 0)
                Destroy(this.gameObject);
        }

        //rigidbody2D.position += rigidbody2D.velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if ((name.Contains("ExplodingArrow") || name.Contains("ShatteringArrow")))
        {
            if (!GetComponent<AOE>().enabled)
            {
                SoundFX sfx = GetComponent<SoundFX>();
                if (sfx != null)
                   sfx.Play("Fire");
                if (AOE_Emitter != null)
                {
                    GameObject emitter = (GameObject)Instantiate(AOE_Emitter, transform.position, transform.rotation);
                    emitter.transform.parent = transform;
                }

                Camera.main.gameObject.GetComponent<CameraBehavior>().BeginShake(OnAOEShakeAmount,OnAOEDampeningAmount);

                GetComponent<AOE>().enabled = true;
                GetComponent<CircleCollider2D>().enabled = true;
            }
        }
        else if (coll.gameObject.GetComponent<Entity>())
        {
            if (name.Contains("WindArrow") && numCollisions >= 0)
            {
                numCollisions--;
                justFired = true;
                GetComponent<BoxCollider2D>().isTrigger = true;

                if (numCollisions == -1)
                    GetStuck(coll.collider);
            }

            Entity isEntity = coll.gameObject.GetComponent<Entity>();
            if (isEntity != null)
                damageType.attachToEnemy(isEntity);
        }

        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Boss")
            GameManager.instance.stats.shotsHit++;

        if (!name.Contains("WindArrow") || (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Boss"))
            GetStuck(coll.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (justFired)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            justFired = false;
        }
    }

    protected void GetStuck(Collider2D coll)
    {
        if (coll.tag != "Sphere" /*&& !justFired*/ && transform.parent == null)
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
            }
            transform.parent = dummyChildTransform;
            rigidbody2D.isKinematic = true;
        }
    }
}