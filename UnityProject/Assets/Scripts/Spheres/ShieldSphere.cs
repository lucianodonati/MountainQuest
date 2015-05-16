using System.Collections;
using UnityEngine;

public class ShieldSphere : BaseSphere
{
    public float magnitude;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (proj != null)
        {
            magnitude = Vector3.Distance(transform.position, other.transform.position);

            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("EnterSphere");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (proj == null || proj.owner.name != "Boss 2")
        {
            if (proj != null && !proj.stuck && !proj.createdInsideShield)
            {
                if (other.rigidbody2D.velocity.magnitude < 32)
                {
                    Vector3 pushVector = other.transform.position - transform.position;
                    other.rigidbody2D.velocity += new Vector2(pushVector.x, pushVector.y);
                    if (other.rigidbody2D.velocity.magnitude > 32)
                        other.rigidbody2D.velocity = other.rigidbody2D.velocity.normalized * 32;

                    other.transform.rotation = Quaternion.FromToRotation(transform.up, other.rigidbody2D.velocity);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Arrow theArrow = other.GetComponent<Arrow>();
        if (theArrow != null)
            theArrow.owner = gameObject;
    }

    public void SetOwner(Entity _owner)
    {
        Owner = _owner;
    }
}