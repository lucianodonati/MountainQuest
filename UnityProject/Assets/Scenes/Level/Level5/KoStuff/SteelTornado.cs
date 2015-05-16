using System.Collections;
using UnityEngine;

public class SteelTornado : KOAttack
{
    public float TurnTimer;
    public bool CurrDirection;

    // Use this for initialization
    public override void Start()
    {
        base.Start();

        TurnTimer = 1.25f;
        Vector3 toPlayer = player.transform.position - transform.position;
        toPlayer.y = toPlayer.z = 0;

        if (toPlayer.x < 0)
            CurrDirection = false; //left
        else if (toPlayer.x > 0)
            CurrDirection = true; //right
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        GameObject PBL = GameObject.Find("BottomLeft");
        Vector3 PlayerBottomLeft = PBL.transform.position;

        GameObject PTR = GameObject.Find("TopRight");
        Vector3 PlayerTopRight = PTR.transform.position;

        if (transform.position.x <= PlayerBottomLeft.x) // off the left side
        {
            CurrDirection = !CurrDirection;
        }
        if (transform.position.x >= PlayerTopRight.x) // off the right side
        {
            CurrDirection = !CurrDirection;
        }

        TurnTimer -= Time.deltaTime;
        if (TurnTimer <= 0)
        {
            float rand = Random.Range(-9, 10);

            if (transform.position.y <= PlayerBottomLeft.y) // off the bottom
            {
                rand = 10;
            }
            if (transform.position.y >= PlayerTopRight.y) // off the top
            {
                rand = -10;
            }

            Vector3 randVec = new Vector3(0, rand, 0);
            if (CurrDirection == false)
            {
                rigidbody2D.velocity = (Vector3.left.normalized * 35f) + randVec;
            }
            else if (CurrDirection == true)
            {
                rigidbody2D.velocity = (Vector3.right.normalized * 35) + randVec;
            }

            TurnTimer = 0.67f;
            CurrDirection = !CurrDirection;
        }
        // Play animation
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name == "Player")
        {
            player.GetComponent<Health>().TakeDamage(30, false);
        }
    }

    private void OnDestroy()
    {
        rigidbody2D.velocity = new Vector3();
    }
}