using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float stuckTimer = 5;
    private bool stuck = false;
    public int numCollisions = 0;
    private bool justFired = true;

    // Use this for initialization
    private void Start()
    {
        rigidbody2D.velocity = transform.up * speed;
        GetComponent<BoxCollider2D>().isTrigger = true;
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

        rigidbody2D.position += rigidbody2D.velocity * Time.deltaTime;
    }

    private void OnCollisionExit2D()
    {
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        CheckCollision(coll.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        justFired = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void CheckCollision(Collider2D coll)
    {
        if (coll.tag != "Sphere" && !justFired)
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            GetComponent<BoxCollider2D>().isTrigger = true;
            stuck = true;
            Transform dummyChildTransform = coll.transform.FindChild("PreserveScale");
            if (dummyChildTransform == null)
            {
                GameObject dummyChild = new GameObject();
                dummyChild.name = "PreserveScale";
                dummyChild.transform.parent = coll.transform;
                dummyChildTransform = dummyChild.transform;
            }
            transform.parent = dummyChildTransform;
        }
    }
}