using System.Collections;
using UnityEngine;

public class RedirectSphere : MonoBehaviour
{
    public float RotationDirection;
    public Vector3 Direction = new Vector3(0, 1, 0);
    public float DamageModifier = 5f;
    public float AliveTimer = 7f;
    public float PlayerRedirectMagnitude = 30f;
    public float antigravityTime = 0.2f;
    public Player Owner;

    private void Start()
    {
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

            if (AliveTimer <= 2)
            {
                SpriteRenderer mySR = GetComponent<SpriteRenderer>();
                mySR.color = new Color(1, 1, 1, AliveTimer / 2);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Arrow proj = other.GetComponent<Arrow>();
        if (other.tag == "Player")
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("EnterSphere");

            other.rigidbody2D.position = this.transform.position;
            Direction.Normalize();
            Direction *= PlayerRedirectMagnitude;
            other.rigidbody2D.velocity = Direction;
            other.GetComponent<PlayerController>().redirectedTimer = antigravityTime;
            other.rigidbody2D.gravityScale = 0;
        }
        else if (proj != null && proj.owner != null && proj.owner.name != "Boss 2")
        {
            SoundFX sfx = GetComponent<SoundFX>();
            if (sfx != null)
                sfx.Play("EnterSphere");

            GameManager.instance.statsManager.arrowsRedirected++;
            other.rigidbody2D.position = this.transform.position;
            Direction.Normalize();
            Direction *= other.rigidbody2D.velocity.magnitude;
            other.rigidbody2D.velocity = Direction;
            other.rigidbody2D.rotation = RotationDirection;

            other.GetComponent<Arrow>().damageType.damage += DamageModifier;
            other.GetComponent<Arrow>().owner = this.gameObject;
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