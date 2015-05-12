using UnityEngine;
using System.Collections;

public class SpinObjInPlace : MonoBehaviour {
    public float spinSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
        transform.rotation = Quaternion.AngleAxis(360 * spinSpeed * Time.deltaTime, new Vector3(0.0f, 0.0f, 1.0f));
	}
}
