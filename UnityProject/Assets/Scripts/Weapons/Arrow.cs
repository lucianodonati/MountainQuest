﻿using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float stuckTimer = 5;
    private bool stuck = false, justFired = true;
    public int numCollisions = 0;
    public DamageType damageType;

    // Use this for initialization
    private void Start()
    {
        GetComponent<SoundFX>().Play("Fire");
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

        rigidbody2D.position += rigidbody2D.velocity * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            GameManager.instance.stats.shotsHit++;
            Entity isEntity = coll.gameObject.GetComponent<Entity>();
            if (isEntity != null)
                damageType.attachToEnemy(isEntity);
        }

        GetStuck(coll.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
                dummyChild.name = "PreserveScale";
                if (coll.tag == "Platform")
                    dummyChild.transform.parent = coll.transform.parent;
                else
                    dummyChild.transform.parent = coll.transform;
                dummyChildTransform = dummyChild.transform;
            }
            transform.parent = dummyChildTransform;
        }
    }
}