using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Arrow")
            Destroy(this.gameObject);
    }
}