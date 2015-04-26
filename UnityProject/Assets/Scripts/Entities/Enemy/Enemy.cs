using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        transform.up = Vector2.up;
        base.Update();
    }
}