using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public List<GameObject> requisites;
    private float duration = 1.0f, startTime;
    private bool open = false;
    private SpriteRenderer sprite;

    // Use this for initialization
    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        OpenDoor();
        if (open)
        {
            if (sprite != null)
            {
                float t = (Time.time - startTime) / duration;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.SmoothStep(1.0f, 0.0f, t));
            }
            if (sprite.color.a <= 0.0f)
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" /*|| collision.gameObject.GetComponent<Arrow>.getOwner().gameobject.tag == "Player"*/)
        {
            OpenDoor();
            startTime = Time.time;
        }
    }

    public void RemoveFromKeyList(GameObject thisGuy)
    {
        startTime = Time.time;
        requisites.Remove(thisGuy);
    }

    private void OpenDoor()
    {
        if (requisites.Count == 0)
            open = true;
    }
}