using System.Collections;
using UnityEngine;

public class ShieldSphere : BaseSphere
{
    private float enterSpeed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (proj != null && !proj.stuck && !proj.createdInsideShield)
        {
            enterSpeed = other.rigidbody2D.velocity.magnitude;

            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("EnterSphere");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (proj != null && !proj.stuck && !proj.createdInsideShield && proj.owner != null && proj.owner.name != "Boss 2")
        {
            Vector3 pushVector = (other.transform.position - transform.position) * GetComponent<CircleCollider2D>().radius;

            Debug.Log("Push: " + pushVector + " Arrow: " + other.rigidbody2D.velocity);

            other.rigidbody2D.velocity += new Vector2(pushVector.x, pushVector.y);
            if (other.rigidbody2D.velocity.magnitude > enterSpeed)
                other.rigidbody2D.velocity = other.rigidbody2D.velocity.normalized * enterSpeed;

            other.transform.rotation = Quaternion.FromToRotation(transform.up, other.rigidbody2D.velocity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Arrow theArrow = other.GetComponent<Arrow>();
        if (theArrow != null)
        {
            theArrow.owner = gameObject;

            if (theArrow.createdInsideShield)
                theArrow.createdInsideShield = false;
        }
    }
}