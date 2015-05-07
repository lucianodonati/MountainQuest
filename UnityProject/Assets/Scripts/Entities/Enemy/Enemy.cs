using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    private bool disableSeek = true;
    public GameObject lastPlatform;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        transform.up = Vector2.up;
        Seek_Movement seek = gameObject.GetComponent<Seek_Movement>();
        if (seek != null)
            seek.enabled = disableSeek;
        base.Update();
    }

    public override void die()
    {
        base.die();
    }

    public void setAI(bool set)
    {
        Movement_Coordinator coord = gameObject.GetComponent<Movement_Coordinator>();
        if (coord != null)
            coord.enabled = set;
        Enemy_Movement[] movementAI = gameObject.GetComponents<Enemy_Movement>();
        foreach (Enemy_Movement aiMovement in movementAI)
        {
            aiMovement.enabled = set;
        }

        AttackAI[] attackAI = GetComponents<AttackAI>();
        foreach (AttackAI attack in attackAI)
        {
            attack.enabled = set;
        }
        disableSeek = set;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Platform")
            lastPlatform = coll.transform.parent.gameObject;
    }
}