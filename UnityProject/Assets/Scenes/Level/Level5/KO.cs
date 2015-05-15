using System.Collections;
using UnityEngine;

public class KO : Entity
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void die()
    {
        // Play animation here

        base.die(); // Play sound and set bool "dead" to true
    }
}