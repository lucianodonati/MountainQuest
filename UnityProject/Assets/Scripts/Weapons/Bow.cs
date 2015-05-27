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
            if (playercontroller.Arrow.active)
                GetComponent<SpriteRenderer>().color = playercontroller.Arrow.prefab.GetComponent<SpriteRenderer>().color;

            if (playercontroller.Arrow.prefab.name.Contains("Exploding") ||
                playercontroller.Arrow.prefab.name.Contains("Shattering") ||
                playercontroller.Arrow.prefab.name.Contains("Lightning") ||
                playercontroller.Arrow.prefab.name.Contains("Earthquake") ||
                playercontroller.Arrow.prefab.name.Contains("Plague"))
            {
                pSys.startColor = playercontroller.Arrow.prefab.GetComponent<SpriteRenderer>().color;
                pSys.emissionRate = eRate;
            }
            else
                pSys.emissionRate = 0;

            transform.up = (Vector3)((Vector2)(Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized);

            if (owner.transform.localScale.x < 0)
                transform.up = new Vector3(-1 * transform.up.x, transform.up.y, transform.up.z);

            transform.Rotate(0, 0, 45);
        }
    }
}