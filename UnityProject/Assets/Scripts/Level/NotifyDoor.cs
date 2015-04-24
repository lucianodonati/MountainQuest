using UnityEngine;
using System.Collections;

public class NotifyDoor : MonoBehaviour {
    public GameObject attachedDoor;

    void OnDestroy()
    {
        if (attachedDoor != null)
            attachedDoor.GetComponent<Door>().RemoveFromKeyList(gameObject);
    }
}
