using UnityEngine;
using System.Collections;

public class RainOfDaggers : KOAttack
{
    public GameObject Dagger;
    private GameObject daggerSpawn;

    public override void Start()
    {
        base.Start();
        daggerSpawn = GameObject.Find("DaggerSpawn");
    }

    public override void Update()
    {
        base.Update();
        //Vector3 playerPos = player.transform.position;
        //playerPos -= transform.position;
        //playerPos.z = 0;

        for (int i = 0; i < 50; i += 5)
        {
            Vector3 daggerSpacing = new Vector3(i, 0, 0);
            Quaternion meh = new Quaternion(90, 0, 0, 0);
            GameObject currDagger = (GameObject)Instantiate(Dagger, (daggerSpawn.gameObject.transform.position - daggerSpacing), meh);

            currDagger.rigidbody2D.velocity = Vector2.right * 5.0f * Time.deltaTime;



            //Vector3 down = new Vector3(0, -1, 0);

            //currDagger.rigidbody2D.velocity = down * 5.0f;
        }
        attackTimer = 0;

    }
}
