using UnityEngine;
using System.Collections;

public class AttackAI : MonoBehaviour {

    protected float reloadTimer;
    public float reloadTimerMax;

    public GameObject weapon;

    protected Movement_Coordinator coordinator;

	// Use this for initialization
	protected virtual void Start () {
        coordinator = gameObject.GetComponent<Movement_Coordinator>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
	}
}
