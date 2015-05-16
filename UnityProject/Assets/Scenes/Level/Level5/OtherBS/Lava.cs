using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    private float timer = 0.0f;
    private Player player;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (timer > 0.0f)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            if (player != null)
            {
                if (timer <= 0.0f)
                {
                    if (!player.dead)
                    {
                        timer = 0.5f;
                        player.health.TakeDamage(10, true);
                    }
                }
            }
        }
    }
}