using System.Collections;
using UnityEngine;

public abstract class KOAttack : MonoBehaviour
{
    public bool doneAttacking = false;
    public float attackTimer = 5.0f;

    public virtual void Start()
    {
        attackTimer = 5.0f;
    }

    public virtual void Update()
    {
    }
}