using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Health health;
    public List<Affliction> affectedBy;

    // Use this for initialization
    private void Start()
    {
        health = gameObject.AddComponent<Health>();
        affectedBy = new List<Affliction>();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void die()
    {
        Destroy(gameObject);
    }
}