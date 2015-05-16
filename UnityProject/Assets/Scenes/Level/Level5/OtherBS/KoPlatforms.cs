using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoPlatforms : MonoBehaviour
{
    public bool right = true;

    [HideInInspector]
    public List<GameObject> koPlats;

    public List<GameObject> leftPlats, rightPlats, coolAttackPos;

    // Use this for initialization
    private void Start()
    {
        koPlats = rightPlats;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            right = true;
            koPlats = rightPlats;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            right = false;
            koPlats = leftPlats;
        }
    }
}