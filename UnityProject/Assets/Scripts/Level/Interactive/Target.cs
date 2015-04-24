using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Arrow")
            Destroy(this.gameObject);
    }
}