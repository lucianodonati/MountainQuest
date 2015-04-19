using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayVolumeLevelText : MonoBehaviour {

	public GameObject slider;

	// Update is called once per frame
	void Update () {
		GetComponent<Text> ().text = slider.GetComponent<Slider> ().value.ToString();
	}
}