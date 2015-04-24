using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {
    public float waitTime;

	// Update is called once per frame
	void Update () {
        waitTime -= Time.deltaTime;

        //if(waitTime <= 0)
            
	}
}
