using System.Collections;
using UnityEngine;

public class SoulOrb : Arrow
{
    private Transform player;
    protected float rotationRate = 5;
    private bool hit = false;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        stuck = false;
        player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Vector2 toTarget = (Vector2)player.position - (Vector2)transform.position;
        Vector2 newDirection =
            Vector2.Lerp(rigidbody2D.velocity.normalized, toTarget.normalized, Time.deltaTime * 30);

        rigidbody2D.velocity = newDirection * speed * 2;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            hit = true;
        base.OnTriggerEnter2D(other);
    }

    private void OnDestroy()
    {
        if (hit)
            GameObject.Find("KO").GetComponent<KO>().healKO(damageType.damage);
    }
}