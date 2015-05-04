using System.Collections;
using UnityEngine;

public abstract class OverrideEffect : MonoBehaviour
{
    private Color originalColor;
    public Color color;
    public bool changeColor = false;
    public float duration = 3.0f;

    // Use this for initialization
    public virtual void Start()
    {
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.setAI(false);

        if (changeColor)
        {
            originalColor = gameObject.GetComponent<SpriteRenderer>().color;
            gameObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public virtual void Update()
    {
    }

    // Update is called once per frame
    public virtual void OnDestroy()
    {
        if (changeColor)
            gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if (enemy != null)
            enemy.setAI(true);
    }
}