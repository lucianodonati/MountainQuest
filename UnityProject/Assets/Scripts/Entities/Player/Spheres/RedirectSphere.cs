using System.Collections;
using UnityEngine;

public class RedirectSphere : MonoBehaviour
{
    public float RotationDirection;
    public Vector3 Direction;
    public float DamageModifier = 5;
    public float AliveTimer = 7;
    public Player Owner;

    private void Start()
    {
        Direction = new Vector3(0, 1, 0);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, RotationDirection));
        if (AliveTimer != -1)
        {
            AliveTimer -= Time.deltaTime;
            if (AliveTimer <= 0)
            {
                if (Owner != null)
                    if (Owner.GetComponent<Player>())
                        Owner.GetComponent<Player>().RemoveRSphere();
                Destroy(this.gameObject);
            }
        }

        if (AliveTimer <= 2)
        {
            SpriteRenderer mySR = GetComponent<SpriteRenderer>();
            mySR.color = new Color(1, 1, 1, AliveTimer / 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();

        if (proj != null)
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("Poop");

            GameManager.instance.stats.arrowsRedirected++;
            other.rigidbody2D.position = this.transform.position;
            Direction.Normalize();
            Direction *= other.rigidbody2D.velocity.magnitude;
            other.rigidbody2D.velocity = Direction;
            other.rigidbody2D.rotation = RotationDirection + 0;

            proj.damageType.damage += DamageModifier;
        }
    }

    public void ChangeDirection(float rdir, Vector3 dir, float timer)
    {
        Direction = dir;
        RotationDirection = rdir;
        AliveTimer = timer;
    }

    public void SetOwner(Player owner)
    {
        Owner = owner;
    }
}