using System.Collections;
using UnityEngine;

public class WindArrow : Arrow
{
    private bool hit = false;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D coll)
    {
        numCollisions += 1;
        if (coll.gameObject.layer == LayerMask.NameToLayer("Entity"))
        {
            if (!hit)
            {
                hit = true;
                GameManager.instance.statsManager.shotsHit++;
            }
            Entity isEntity = coll.gameObject.GetComponent<Entity>();
            if (isEntity != null)
                damageType.attachToEnemy(isEntity);
        }
        if (numCollisions <= 2)
        {
            GetStuck(coll.collider);
        }
    }
}