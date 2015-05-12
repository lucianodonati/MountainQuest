using UnityEngine;
using System.Collections;

public class AnimateLightning : Arrow {

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.GetComponent<Entity>())
        {
            if (name.Contains("WindArrow") && numCollisions > 0)
            {
                numCollisions--;
                Physics2D.IgnoreCollision(coll.collider, this.collider2D);
            }

            Entity isEntity = coll.gameObject.GetComponent<Entity>();
            if (isEntity != null)
                damageType.attachToEnemy(isEntity);

            GetComponent<Animator>().SetBool("animate", true);
        }

        if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Boss")
            GameManager.instance.stats.shotsHit++;

        if (!name.Contains("WindArrow") || (coll.gameObject.tag != "Enemy" && coll.gameObject.tag != "Boss"))
            GetStuck(coll.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (name.Contains("ExplodingArrow") || name.Contains("ShatteringArrow"))
        {
            CircleCollider2D circle = GetComponent<CircleCollider2D>();
            if (circle.enabled &&
                (other.tag == "Enemy" || other.tag == "Boss") &&
                other.Equals((Collider2D)other.gameObject.GetComponent<BoxCollider2D>()))
            {
                Entity isEntity = other.gameObject.GetComponent<Entity>();
                if (isEntity != null)
                    damageType.attachToEnemy(isEntity);
            }
        }
        if ((name.Contains("WindArrow") && (LayerMask.LayerToName(other.gameObject.layer) == "Platform" || other.name.Contains("Platforms"))))
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            GetStuck(other);
        }
        else if (name.Contains("WindArrow") && other.gameObject.GetComponent<Enemy>())
        {
            if (other.gameObject.GetComponent<CircleCollider2D>() != null)
            {
                if (other.isTrigger == false)
                {
                    numCollisions--;

                }
            }
        }
    }
}
