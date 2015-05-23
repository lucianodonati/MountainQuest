using System.Collections;
using UnityEngine;

public class KeyBehavior : MonoBehaviour
{
    public GameObject emitter;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Instantiate(emitter, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}