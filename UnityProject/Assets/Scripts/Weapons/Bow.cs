using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    private GameObject owner;
    private PlayerController playercontroller;
    private ParticleSystem pSys;
    private float eRate;

    // Use this for initialization
    private void Start()
    {
        owner = transform.parent.gameObject;

        if (owner.tag == "Player")
            playercontroller = owner.GetComponent<PlayerController>();

        transform.Rotate(0, 0, 45);

        pSys = GetComponent<ParticleSystem>();
        eRate = pSys.emissionRate;
        pSys.emissionRate = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (playercontroller != null)
        {
            if (playercontroller.Arrow != null)
                GetComponent<SpriteRenderer>().color = playercontroller.Arrow.GetComponent<SpriteRenderer>().color;

            if (playercontroller.Arrow.name.Contains("ExplodingArrow") ||
                playercontroller.Arrow.name.Contains("ShatteringArrow") ||
                playercontroller.Arrow.name.Contains("LightningArrow") ||
                playercontroller.Arrow.name.Contains("EarthquakeArrow") ||
                playercontroller.Arrow.name.Contains("PlagueArrow"))
            {
                pSys.startColor = playercontroller.Arrow.GetComponent<SpriteRenderer>().color;
                pSys.emissionRate = eRate;
            }
            else
                pSys.emissionRate = 0;

            transform.up = (Vector3)((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);

            transform.Rotate(0, 0, 45);
        }
    }
}