using UnityEngine;
using System.Collections;

public class CameraBehavior : MonoBehaviour {

	GameObject player = null;

	float halfHeight;
	float halfWidth;

	//KEEP IN MIND: camera stops at their POSITIONS.
	//Minbound is in the bottom left of the level
	//Maxbound is in the top right of the level
	public GameObject MinBound = null;
	public GameObject MaxBound = null;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");

		halfHeight = camera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;
	}
	
	// Update is called once per frame
	void Update () {

		halfHeight = camera.orthographicSize;
		halfWidth = halfHeight * Screen.width / Screen.height;

		Vector3 newpos = new Vector3 (player.transform.position.x, player.transform.position.y, transform.position.z);

		if (newpos.x - halfWidth < MinBound.transform.position.x)
			newpos.x = MinBound.transform.position.x + halfWidth;
		else if(newpos.x + halfWidth > MaxBound.transform.position.x)
			newpos.x = MaxBound.transform.position.x - halfWidth;

		if (newpos.y - halfHeight < MinBound.transform.position.y)
			newpos.y = MinBound.transform.position.y + halfHeight;
		else if(newpos.y + halfHeight > MaxBound.transform.position.y)
			newpos.y = MaxBound.transform.position.y - halfHeight;

		transform.position = newpos;
	}
}
