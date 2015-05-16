using UnityEngine;
using System.Collections;

public class SteelTornado : MonoBehaviour
{





    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Play animation

          float rand = Random.Range(-5,5);
       
            Vector3 playerPos = GetComponent<KO>().player.transform.position;
            playerPos -= transform.position;
            playerPos.z = 0;
            playerPos.y += rand;

            rigidbody2D.velocity = playerPos.normalized * 4.5f;



    }
}
