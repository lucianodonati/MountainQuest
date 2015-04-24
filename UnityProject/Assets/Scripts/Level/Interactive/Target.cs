using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arrow")
            Destroy(this.gameObject);
    }
}