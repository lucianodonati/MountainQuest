using System.Collections;
using UnityEngine;

public abstract class OverrideEffect : MonoBehaviour
{
    public float duration = 3.0f;

    // Use this for initialization
    public virtual void Start()
    {
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.setAI(false);
    }

    public virtual void Update()
    {
    }

    // Update is called once per frame
    public virtual void OnDestroy()
    {
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.setAI(true);
    }
}