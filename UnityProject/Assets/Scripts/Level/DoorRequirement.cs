using System.Collections;
using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    public Door door;

    // Use this for initialization
    private void Start()
    {
        if (door != null)
            door.AddRequirement(gameObject);
        else
            Debug.LogError("Door requirement invalid (" + gameObject.name + ") <-- Go here and fix.");
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnDestroy()
    {
        if (door != null)
            door.RemoveFromKeyList(gameObject);
    }
}