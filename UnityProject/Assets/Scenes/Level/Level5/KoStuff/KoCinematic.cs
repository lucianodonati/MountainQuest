using System.Collections;
using UnityEngine;

public class KoCinematic : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public bool goTransform = false;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        this.transform.up = Vector2.up;
        if (goTransform)
        {
            if (GameObject.Find("Portal") != null)
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Portal").transform.position, moveSpeed * Time.deltaTime);
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("EnterRoom").transform.position, moveSpeed * Time.deltaTime);
                if (Vector2.Distance(GameObject.Find("EnterRoom").transform.position, transform.position) < 5.0f)
                {
                    GetComponent<KO>().animating = false;
                    Destroy(this);
                }
            }
        }
    }
}