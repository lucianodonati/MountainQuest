using System.Collections;
using UnityEngine;

public class startIntro : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.GetComponent<PlayerController>().movementEnabled = false;
            GameObject.Find("KO").GetComponent<KoCinematic>().goTransform = true;
        }
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
            GameObject.Find("KO").GetComponent<KoCinematic>().goTransform = true;
    }
}