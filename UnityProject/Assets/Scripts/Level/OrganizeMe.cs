using System.Collections;
using UnityEngine;

public class OrganizeMe : MonoBehaviour
{
    public string daddy;

    // Use this for initialization
    private void Start()
    {
        GameObject dad = GameObject.Find(daddy);
        if (dad == null)
            dad = new GameObject(daddy);

        transform.parent = dad.transform;
    }
}