using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

    public GameObject attachedDoor;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arrow")
        {
            // TODO:  Call function to open door or whatever
            attachedDoor.GetComponent<Door>().RemoveFromKeyList(gameObject);
            Destroy(this.gameObject);
        }
    }
}