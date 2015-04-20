using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> requisites;

    // Use this for initialization
    private void Start()
    {
        requisites = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" /*|| collision.gameObject.GetComponent<Arrow>.getOwner().gameobject.tag == "Player"*/)
        {
            if (requisites.Count == 0)
                openDoor();
        }
    }

    public void openDoor()
    {
        Destroy(gameObject);
    }
}