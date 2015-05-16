using System.Collections;
using UnityEngine;

public class CloseDoor : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameObject.Find("wall-e").GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }
}