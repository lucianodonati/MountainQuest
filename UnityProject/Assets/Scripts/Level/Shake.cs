using System.Collections;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float duration = 2.0f;
    private Vector3 originalPosition;

    // Use this for initialization
    private void Start()
    {
        originalPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        gameObject.transform.position += new Vector3(Random.Range(-0.06f, 0.06f), Random.Range(-0.06f, 0.06f), 0);
        duration -= Time.deltaTime;

        if (duration <= 0.0f)
            Destroy(this);
    }

    private void OnDestroy()
    {
        transform.position = originalPosition;
    }
}