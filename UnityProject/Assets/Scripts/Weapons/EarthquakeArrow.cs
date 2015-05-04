using System.Collections;
using UnityEngine;

public class EarthquakeArrow : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
        {
            if (coll.gameObject.transform.parent.gameObject.GetComponents<Shake>().Length < 1)
                coll.gameObject.transform.parent.gameObject.AddComponent<Shake>();
            else
                coll.gameObject.transform.parent.gameObject.GetComponent<Shake>().duration = 2.0f;
        }
        else if (coll.gameObject.tag == "Enemy")
        {
            Enemy theEnemy = coll.gameObject.GetComponent<Enemy>();
            if (theEnemy.lastPlatform.GetComponents<Shake>().Length < 1)
                theEnemy.lastPlatform.AddComponent<Shake>();
            else
                theEnemy.lastPlatform.GetComponent<Shake>().duration = 2.0f;
        }
    }
}