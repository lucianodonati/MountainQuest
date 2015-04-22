using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float stuckTimer = 5;
    private bool stuck = false;
    public int numCollisions = 0;

    // Use this for initialization
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 14, true);
        rigidbody2D.velocity = transform.up * speed;
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
        Physics2D.IgnoreLayerCollision(9, 14, false);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("HIT ");
        CheckCollision(coll.gameObject.tag);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckCollision(other.tag);
    }

    private void CheckCollision(string tag)
    {
        if (tag != "Sphere")
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            stuck = true;
        }
    }
}