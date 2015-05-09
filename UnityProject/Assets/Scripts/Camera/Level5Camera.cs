using System.Collections;
using UnityEngine;

public class Level5Camera : MonoBehaviour
{
    public CameraBehavior cameraBehaviour;
    public bool playAnim = false;
    public Transform startKo, endKo;
    public GameObject koPlat;
    public float speed = 1.0F, reSizeDuration = 3.0f;
    private float startTime;
    private float journeyLengthResize, journeyLengthMove;
    private bool seenKo = false;
    public AudioClip music;

    private void Awake()
    {
    }

    private void Start()
    {
        GameManager.instance.setMusic(music);
        startTime = Time.time;
        journeyLengthResize = Vector3.Distance(startKo.position, endKo.position);
        journeyLengthMove = Vector3.Distance(endKo.position, new Vector3(endKo.position.x, 7.7f, 0));
    }

    private void Update()
    {
        if (playAnim)
        {
            if (!seenKo)
            {
                float distCovered = (Time.time - startTime) * speed;
                float fracJourney = distCovered / journeyLengthMove;
                transform.position = Vector3.Lerp(startKo.position, endKo.position, fracJourney);
                if (transform.position == endKo.position)
                {
                    startTime = Time.time;
                    seenKo = true;
                }
            }
            else
            {
                float t = (Time.time - startTime) / reSizeDuration;
                Camera.main.orthographicSize = Mathf.Lerp(2.260251f, 12.94355f, t);

                if (Camera.main.orthographicSize == 12.94355f)
                {
                    float distCovered = (Time.time - startTime) * speed;
                    float fracJourney = distCovered / journeyLengthResize;
                    koPlat.GetComponent<Rigidbody2D>().isKinematic = false;
                    transform.position = Vector3.Lerp(endKo.position, new Vector3(endKo.position.x, -0.1f, -1.0f), fracJourney);
                    if (transform.position == new Vector3(endKo.position.x, -0.1f, -1.0f))
                    {
                        cameraBehaviour.enabled = true;
                        Destroy(this);
                        Destroy(GameObject.Find("Faia"));
                    }
                }
            }
        }
        else
            startTime = Time.time;
    }
}