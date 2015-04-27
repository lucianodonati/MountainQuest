using System.Collections;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private GameObject player = null;

    private float halfHeight;
    private float halfWidth;
    public float deadHalfHeight = 4;

    //KEEP IN MIND: camera stops at their POSITIONS.
    //Minbound is in the bottom left of the level
    //Maxbound is in the top right of the level
    public GameObject MinBound = null;

    public GameObject MaxBound = null;

    public GameObject looktarget;
    public float targSnapSpeed = 4;

    private float originalSize;
    public float toSize;
    public float cameraResizeSpeed = 8;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        originalSize = Camera.main.orthographicSize;

        toSize = originalSize;
    }

    // Update is called once per frame
    private void Update()
    {
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        Vector3 newpos;

        if (looktarget == null)
        {
            if (camera.orthographicSize < originalSize)
                camera.orthographicSize = originalSize;

            newpos = new Vector3(player.transform.position.x, player.transform.position.y + deadHalfHeight, transform.position.z);

            if ((player.transform.position.y > transform.position.y - deadHalfHeight))
            {
                newpos.y = transform.position.y;
            }
            if (player != null)
            {
                if (player.GetComponent<PlayerController>() != null)
                {
                    if (player.GetComponent<PlayerController>().grounded && transform.position.y - deadHalfHeight < player.transform.position.y)
                    {
                        newpos.y = Mathf.SmoothStep(transform.position.y, player.transform.position.y + deadHalfHeight, 8 * Time.deltaTime);
                    }
                    else if (transform.position.y + (3 * halfHeight / 4) < player.transform.position.y)
                    {
                        newpos.y = player.transform.position.y - (3 * halfHeight / 4);
                    }
                }
            }

            newpos = AdjustForBounds(newpos);
        }
        else
        {
            newpos = SnapTo(looktarget.transform.position, targSnapSpeed);
            Resize(toSize, cameraResizeSpeed);
        }

        transform.position = newpos;
    }

    private Vector3 AdjustForBounds(Vector3 pos)
    {
        if (MinBound != null)
        {
            if (pos.x - halfWidth < MinBound.transform.position.x)
                pos.x = MinBound.transform.position.x + halfWidth;

            if (pos.y - halfHeight < MinBound.transform.position.y)
                pos.y = MinBound.transform.position.y + halfHeight;
        }

        if (MaxBound != null)
        {
            if (pos.x + halfWidth > MaxBound.transform.position.x)
                pos.x = MaxBound.transform.position.x - halfWidth;

            if (pos.y + halfHeight > MaxBound.transform.position.y)
                pos.y = MaxBound.transform.position.y - halfHeight;
        }

        return pos;
    }

    private Vector3 SnapTo(Vector3 pos, float speed)
    {
        return new Vector3(Mathf.Lerp(transform.position.x, pos.x, speed * Time.deltaTime),
                           Mathf.Lerp(transform.position.y, pos.y, speed * Time.deltaTime),
                           transform.position.z);
    }

    private void Resize(float newSize, float speed)
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, newSize, speed * Time.deltaTime);
    }
}