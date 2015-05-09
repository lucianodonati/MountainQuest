using System.Collections;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    private GameObject player = null;

    private float halfHeight;
    private float halfWidth;
    public float deadHalfHeight = 4;

    //TRANSITION VARS

    //KEEP IN MIND: camera stops at their POSITIONS.
    //Minbound is in the bottom left of the level
    //Maxbound is in the top right of the level
    public GameObject MinBound = null;
    public GameObject MaxBound = null;

    private float size;
    private bool stationary;
    private Vector3 targetPosition;

    public bool focused;
    public float snapDistance = 0.05f;
    public float accelerateDistance = 2;

    public float moveLerpSpeed = 8;
    public float scaleLerpSpeed = 16;

    private GameObject lastWaypoint;

    //SHAKE VARS
    public bool shaking;
    private float currentShakeMagnitude;
    public float initialShakeMagnitude;
    public float shakeDampening;

    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        size = Camera.main.orthographicSize;

    }

    // Update is called once per frame
    private void Update()
    {
        halfHeight = camera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;

        Vector3 newpos = SetPos();

        newpos = AdjustForBounds(newpos);

        transform.position = newpos;

        AdjustSize();

        if (shaking)
        {
            ShakeScreen();
        }
    }

    Vector3 SetPos()
    {
        Vector3 pos;

        if(stationary)
        {
            pos = targetPosition;
        }
        else
        {
            pos = new Vector3(player.transform.position.x, player.transform.position.y + deadHalfHeight, transform.position.z);

            if ((player.transform.position.y > transform.position.y - deadHalfHeight))
                pos.y = transform.position.y;

            if (player.GetComponent<PlayerController>().grounded && transform.position.y - deadHalfHeight < player.transform.position.y)
            {
                pos.y = Mathf.SmoothStep(transform.position.y, player.transform.position.y + deadHalfHeight, 8 * Time.deltaTime);
            }
            else if (transform.position.y + (3 * halfHeight / 4) < player.transform.position.y)
            {
                pos.y = player.transform.position.y - (3 * halfHeight / 4);
            }
        }

        if ((transform.position - pos).magnitude <= snapDistance)
            focused = true;

        pos = Vector3.Lerp(transform.position,
                           pos,
                           (focused?1:((((transform.position - pos).magnitude <= accelerateDistance)?3:1)*moveLerpSpeed*Time.deltaTime)));

        return pos;
    }

    private Vector3 AdjustForBounds(Vector3 pos)
    {
        if (focused)
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
        }

        return pos;
    }

    void AdjustSize()
    {
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, size, scaleLerpSpeed * Time.deltaTime);
    }

    #region screenshake

    void ShakeScreen()
    {
        if (currentShakeMagnitude > 0.0f)
        {
            Vector2 shakeOffs = Random.insideUnitCircle * currentShakeMagnitude;
            transform.position += new Vector3(shakeOffs.x, shakeOffs.y, 0);

            currentShakeMagnitude -= Time.deltaTime * shakeDampening;
        }
        else
        {
            shaking = false;
        }
    }

    public void BeginShake(float magnitude)
    {
        currentShakeMagnitude = initialShakeMagnitude = magnitude;
        shaking = true;
    }

    public void BeginShake(float magnitude, float dampening)
    {
        currentShakeMagnitude = initialShakeMagnitude = magnitude;
        shakeDampening = dampening;
        shaking = true;
    }

    #endregion

    void SetView(CameraWaypoint hit)
    {
        if (hit.gameObject != lastWaypoint)
        {
            lastWaypoint = hit.gameObject;

            stationary = hit.isStationary;

            if (hit.isStationary)
            {
                targetPosition = new Vector3(hit.stationaryFocus.position.x, hit.stationaryFocus.position.y, transform.position.z);
            }

            size = hit.cameraSize;

            MinBound = hit.MinBound;
            MaxBound = hit.MaxBound;

            moveLerpSpeed = hit.moveLerpSpeed;
            scaleLerpSpeed = hit.scaleLerpSpeed;

            focused = false;
        }
    }
}