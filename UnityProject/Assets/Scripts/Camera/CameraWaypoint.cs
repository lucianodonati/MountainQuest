using System.Collections;
using UnityEngine;

public class CameraWaypoint : MonoBehaviour
{
    public bool isStationary, destroyOnContact = false, justPlayer = false;
    public Transform stationaryFocus;

    public float cameraSize = 10;

    public GameObject MinBound;
    public GameObject MaxBound;

    public float moveLerpSpeed = 8;
    public float scaleLerpSpeed = 16;

    public bool checkpoint = false;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" || (!justPlayer && coll.gameObject.tag == "TriggersWaypoint"))
        {
            Camera.main.GetComponent<CameraBehavior>().SendMessage("SetView", this);
            if (destroyOnContact)
                Destroy(gameObject);
        }
    }
}