using System.Collections;
using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    public Door door;

    // Use this for initialization
    private void Start()
    {
        door.AddRequirement(gameObject);
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