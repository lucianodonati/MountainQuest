using UnityEngine;
using System.Collections;

public class Enemy_Movement : MonoBehaviour {

    public bool direction;

    protected Vector3 preserveUp;

	// Use this for initialization
	protected virtual void Start () {
        preserveUp = transform.up;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	
	}
}
