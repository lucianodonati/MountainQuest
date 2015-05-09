using UnityEngine;
using System.Collections;

public class CameraWaypoint : MonoBehaviour {

    public bool isStationary;
    public Transform stationaryFocus;

    public float cameraSize = 10;

    public GameObject MinBound;
    public GameObject MaxBound;

    public float moveLerpSpeed = 8;
    public float scaleLerpSpeed = 16;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Camera.main.GetComponent<CameraBehavior>().SendMessage("SetView", this);
        }
    }
}
