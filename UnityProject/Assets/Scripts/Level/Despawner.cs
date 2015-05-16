using System.Collections;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    public bool killPlayer = true;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (!coll.isTrigger)
        {
            if (coll.GetComponent<Entity>() != null)
            {
                if (coll.GetComponent<PlayerController>() == null)
                    Destroy(coll.gameObject);
                else if (killPlayer)
                    coll.GetComponent<Player>().health.TakeDamage(coll.GetComponent<Player>().maxHealth * Random.Range(1.0f, 100.0f), true);
            }
        }
    }
}