using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScrollingText : MonoBehaviour {
	public float waitToScrollTime;
	public float scrollSpeed;
	public float endScrollYPos;
	private bool createButton = true;
	public UnityEngine.UI.Button returnButton;

	// Update is called once per frame
	void Update () {
		if (createButton) {
			if (waitToScrollTime > 0)
				waitToScrollTime -= Time.deltaTime;
			else if (transform.position.y >= endScrollYPos) {
				returnButton.gameObject.SetActive(true);
				createButton = false;
			} else {
				transform.Translate (0, scrollSpeed * Time.deltaTime, 0);
			}
		}
	}
}