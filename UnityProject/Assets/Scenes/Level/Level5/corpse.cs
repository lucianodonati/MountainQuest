using System.Collections;
using UnityEngine;

public class corpse : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
            GetComponent<Animator>().enabled = true;
    }
}