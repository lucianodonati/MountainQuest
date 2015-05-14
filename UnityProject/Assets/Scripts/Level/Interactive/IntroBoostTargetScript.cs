using UnityEngine;
using System.Collections;

public class IntroBoostTargetScript : MonoBehaviour {
    public GameObject instructions;
    public GameObject boostSphere;
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Arrow" || other.gameObject.tag == "Sword")
        {
            instructions.SetActive(true);
            boostSphere.SetActive(true);
        }
    }
}
