using UnityEngine;
using System.Collections;

public class OneWayCollider : MonoBehaviour {

    public Collider2D Actual, Predictor;

    public enum side {left,right, top, bottom};

    public side collidingside;

    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Transform other = coll.gameObject.transform;

        if(collidingside == side.left)
        {
            if (other.position.x < Actual.bounds.min.x)
                        Actual.isTrigger = false;
        }
        else if (collidingside == side.right)
        {
            if (other.position.x > Actual.bounds.max.x)
                Actual.isTrigger = false;
        }
        else if (collidingside == side.top)
        {
            if (other.position.y > Actual.bounds.max.y)
                Actual.isTrigger = false;
        }
        else if (collidingside == side.bottom)
        {
            if (other.position.y < Actual.bounds.min.y)
                Actual.isTrigger = false;
        }

    }

    void OnTriggerExit2D(Collider2D coll)
    {
        Actual.isTrigger = true;
    }
}
