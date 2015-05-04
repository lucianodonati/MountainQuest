using UnityEngine;
using System.Collections;

public class AOE : MonoBehaviour {
    public float duration;
    public float maxRadius;

	// Use this for initialization
	void Start () {
        GetComponent<CircleCollider2D>().radius = 0;
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
