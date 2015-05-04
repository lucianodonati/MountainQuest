using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public GameObject lastPlatform;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        transform.up = Vector2.up;
        base.Update();
    }

    public override void die()
    {
        base.die();
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
            lastPlatform = coll.transform.parent.gameObject;
    }
}