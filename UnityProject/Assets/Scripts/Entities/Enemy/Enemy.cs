using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    private bool disableSeek = true;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        transform.up = Vector2.up;
        base.Update();
        Seek_Movement seek = gameObject.GetComponent<Seek_Movement>();
        if (seek != null)
            seek.enabled = disableSeek;
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
}