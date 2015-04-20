using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Health health;

	private Rect healthRemainingRect;
	private Rect healthMaxRect;

	public float leftwardOffset;
	public float upwardOffset;

	public Texture2D currHealthTexture;
	public Texture2D maxHealthTexture;

	private GUIStyle currHealthStyle;
	private GUIStyle maxHealthStyle;
	
	// Use this for initialization
	void Start () {
		currHealthStyle = new GUIStyle ();
		currHealthStyle.normal.background = currHealthTexture;

		maxHealthStyle = new GUIStyle ();
		maxHealthStyle.normal.background = maxHealthTexture;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.parent.position - new Vector3(0.25f,-0.75f,0));
		screenPosition.y = Screen.height - screenPosition.y;
		
		healthMaxRect = new Rect (screenPosition.x - leftwardOffset, screenPosition.y - upwardOffset, health.maxHP, 20);
		healthRemainingRect = new Rect (screenPosition.x - leftwardOffset, screenPosition.y - upwardOffset, health.currentHP, 20);
	}

	void OnGUI(){
		GUI.Label (healthMaxRect, GUIContent.none, maxHealthStyle);
		GUI.Label (healthRemainingRect, GUIContent.none, currHealthStyle);
	}
}
