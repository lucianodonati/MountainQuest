using UnityEngine;
using System.Collections;

public class AOE : MonoBehaviour {
    public float duration = 1f;
    public float maxRadius = 1f;

	// Use this for initialization
	void Start () {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circle.radius = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        if (circle.radius < maxRadius)
            circle.radius += maxRadius / duration * Time.deltaTime;
        else
        {
            circle.enabled = false;
            enabled = false;
        }
	}
}
