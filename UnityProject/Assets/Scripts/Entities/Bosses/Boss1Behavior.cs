using UnityEngine;
using System.Collections;

public class Boss1Behavior : MonoBehaviour
{
    public float attackDelay = 0.9f;
    public bool attacking = false;
    public float DistanceFromPlayer = 0;
    public Player player;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DistanceFromPlayer = Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position);
        print("meh");
        if (DistanceFromPlayer <= 6.5)
            attacking = true;
        
        if (attacking == true)
        {
            AttackPlayer();
        }

    }


    void AttackPlayer()
    {
        bool startAtkDir = GetComponent<Boss1Movement>().direction;

        attackDelay -= Time.deltaTime;
        if (attackDelay <= 0)
        {
            print("Danger Will Robinson");
            attacking = false;
            attackDelay = 0.9f;
            // attack animation
            if (startAtkDir == GetComponent<Seek_Movement>().direction && Vector3.Distance(transform.position, player.GetComponent<Player>().transform.position) <= 6.5)
            {
                player.GetComponent<Player>().health.TakeDamage(20);
            }
 
        }
      
    }



    //void Delay(float delay)
    //{
    //    while (delay > 0)
    //    {
    //        delay -= Time.deltaTime;
    //        Debug.Log("BAH!");
    //    }
    //}



}
