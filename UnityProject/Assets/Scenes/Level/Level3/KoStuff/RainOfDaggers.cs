using System.Collections;
using UnityEngine;

public class RainOfDaggers : KOAttack
{
    private GameObject daggerSpawn;

    public override void Start()
    {
        base.Start();
        attackTimer = 1.5f;
        daggerSpawn = GameObject.Find("DaggerSpawn");

        for (int i = 0; i < 80; i += 5)
        {
            Vector3 daggerSpacing = new Vector3(i, 0, 0);
            Quaternion meh = new Quaternion(90, 0, 0, 0);
            GameObject currDagger = (GameObject)Instantiate(GetComponent<KO>().Dagger, (daggerSpawn.gameObject.transform.position - daggerSpacing), meh);

            currDagger.rigidbody2D.velocity = Vector2.right * 5.0f * Time.deltaTime;

            currDagger.GetComponent<Arrow>().owner = this.gameObject;
            //Vector3 down = new Vector3(0, -1, 0);

            //currDagger.rigidbody2D.velocity = down * 5.0f;
        }
    }

    public override void Update()
    {
        base.Update();
        if (attackTimer <= 0)
        {
        GameObject.Find("KO").GetComponent<Animator>().SetInteger("attack", 0);
        }
        //Vector3 playerPos = player.transform.position;
        //playerPos -= transform.position;
        //playerPos.z = 0;
    }
}