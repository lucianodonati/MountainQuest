using System.Collections;
using UnityEngine;

public class AnimateLightning : Arrow
{
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