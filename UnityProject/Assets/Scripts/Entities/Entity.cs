using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	public Health health;
	public List<Affliction> affectedBy;
	
	// Use this for initialization
	protected void Start()
	{
		gameObject.AddComponent<Health>();
        health = GetComponent<Health>();
		affectedBy = new List<Affliction>();
	}
	
	// Update is called once per frame
	private void Update()
	{
	}
	
	public virtual void die()
	{
		Destroy(gameObject);
	}
}