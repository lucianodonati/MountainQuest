using System.Collections;
using UnityEngine;

public class KO : Entity
{
    private Animator myAnimator;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void die()
    {
        myAnimator.SetBool("dead", true);

        base.die(); // Play sound and set bool "dead" to true
    }
}