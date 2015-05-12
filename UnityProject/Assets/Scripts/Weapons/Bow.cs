using UnityEngine;
using System.Collections;

public class Bow : MonoBehaviour {

    GameObject owner;
    PlayerController playercontroller;

	// Use this for initialization
	void Start () {
        owner = transform.parent.gameObject;

        if (owner.tag == "Player")
            playercontroller = owner.GetComponent<PlayerController>();

        transform.Rotate(0, 0, 45);

	}
	
	// Update is called once per frame
	void Update () {
        if (playercontroller != null)
        {
            if (playercontroller.Arrow != null)
                GetComponent<SpriteRenderer>().color = playercontroller.Arrow.GetComponent<SpriteRenderer>().color;

            transform.up = (Vector3)((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);

            transform.Rotate(0, 0, 45);
        }
	}
}
