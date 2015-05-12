using System.Collections;
using UnityEngine;

public class CameraWaypoint : MonoBehaviour
{
    public bool isStationary, destroyOnContact = false;
    public Transform stationaryFocus;

    public float cameraSize = 10;

    public GameObject MinBound;
    public GameObject MaxBound;

    public float moveLerpSpeed = 8;
    public float scaleLerpSpeed = 16;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<CameraBehavior>().SendMessage("SetView", this);
            if (destroyOnContact)
                Destroy(gameObject);
        }
    }
}