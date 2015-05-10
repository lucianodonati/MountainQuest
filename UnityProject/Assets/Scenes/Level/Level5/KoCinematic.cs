using System.Collections;
using UnityEngine;

public class KoCinematic : MonoBehaviour
{
    public float moveSpeed = 1.0f;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameObject.Find("Portal") != null)
            transform.position = Vector3.Lerp(transform.position, GameObject.Find("Portal").transform.position, moveSpeed * Time.deltaTime);
        this.transform.up = Vector2.up;
    }
}