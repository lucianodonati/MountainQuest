using System.Collections;
using UnityEngine;

public class startIntro : MonoBehaviour
{
    public AudioClip music;

    private void Start()
    {
        GameManager.instance.setMusic(music);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
            Camera.main.GetComponent<Level5Camera>().enabled = true;
        Destroy(gameObject);
    }
}