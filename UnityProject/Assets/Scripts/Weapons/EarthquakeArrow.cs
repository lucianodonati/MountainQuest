using System.Collections;
using UnityEngine;

public class EarthquakeArrow : MonoBehaviour
{
    public float duration = 2;

    private float instances = 0;

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
        if (!coll.isTrigger && instances <= 0)
        {
            if (coll.gameObject.tag == "Platform")
            {
                if (coll.gameObject.GetComponents<Shake>().Length < 1)
                    coll.gameObject.AddComponent<Shake>();
                else
                    coll.gameObject.GetComponent<Shake>().duration = duration;

                ++instances;
            }
            else if (coll.gameObject.tag == "Enemy")
            {
                Enemy theEnemy = coll.gameObject.GetComponent<Enemy>();
                if (theEnemy.lastPlatform.GetComponents<Shake>().Length < 1)
                    theEnemy.lastPlatform.AddComponent<Shake>();
                else
                    theEnemy.lastPlatform.GetComponent<Shake>().duration = duration;

                ++instances;
            }
        }
    }
}