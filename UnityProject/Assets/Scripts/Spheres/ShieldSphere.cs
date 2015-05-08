using UnityEngine;
using System.Collections;

public class ShieldSphere : BaseSphere {
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
        if (proj != null)
        {
            if (other.rigidbody2D.velocity.magnitude < 32)
            {
                Vector3 pushVector = other.transform.position - transform.position;
                other.rigidbody2D.velocity += new Vector2(pushVector.x, pushVector.y);
                if (other.rigidbody2D.velocity.magnitude > 32)
                    other.rigidbody2D.velocity = other.rigidbody2D.velocity.normalized * 32;
            }
        }
    }

    public void SetOwner(Player owner)
    {
        Owner = owner;
    }
}