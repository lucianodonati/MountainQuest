using UnityEngine;
using System.Collections;

public class ExperienceOrb : MonoBehaviour {

    private float changeTimer;
    public float changeTimerMax;

    private float Xrot, Yrot, Zrot;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (changeTimer > 0.0f)
            changeTimer -= Time.deltaTime;
        else
        {
            Xrot = Random.Range(0.0f, 5.0f);
            Yrot = Random.Range(0.0f, 5.0f);
            Zrot = Random.Range(0.0f, 5.0f);
            changeTimer = changeTimerMax;
        }

        transform.Rotate(Xrot,Yrot,Zrot);
	}
}
