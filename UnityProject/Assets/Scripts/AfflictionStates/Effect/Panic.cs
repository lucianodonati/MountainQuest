using System.Collections;
using UnityEngine;

public class Panic : OverrideEffect
{
    public float faceOtherSideEvery_Min = 0.3f, faceOtherSideEvery_Max = 1.0f;
    public float speed_Min = 10.0f, speed_Max = 15.0f;
    private float timer = 0.0f;
    private bool left = true;
    private Vector2 movePos;

    // Update is called once per frame
    public override void Update()
    {
        duration -= Time.deltaTime;
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            movePos = new Vector2((left ? -1 : 1) * Random.Range(speed_Min, speed_Max), 0.0f);
            left = !left;
            gameObject.rigidbody2D.velocity = movePos;
            timer = Random.Range(faceOtherSideEvery_Min, faceOtherSideEvery_Max);
        }
        if (duration <= 0.0f)
            Destroy(this);
    }
}