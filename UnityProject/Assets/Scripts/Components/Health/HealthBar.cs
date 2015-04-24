using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	private Health health;

	private GameObject remainingHealthBar;
	private GameObject maxHealthBar;

	public float scaler = 10;

	// Use this for initialization
	void Start () {
		health = gameObject.GetComponent<Health> ();

		maxHealthBar = GameObject.CreatePrimitive (PrimitiveType.Cube);
		remainingHealthBar = GameObject.CreatePrimitive (PrimitiveType.Cube);

		maxHealthBar.renderer.material.color = Color.black;
		remainingHealthBar.renderer.material.color = Color.red;		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (maxHealthBar != null && remainingHealthBar != null) {

			maxHealthBar.transform.position = new Vector3(transform.position.x,
			                                              renderer.bounds.max.y + 1f,
			                                              1.0f);

			maxHealthBar.transform.localScale = new Vector3 ((transform.lossyScale.x/3),
			                                                 0.5f,
			                                                 1);
			remainingHealthBar.transform.localScale = new Vector3 ((transform.lossyScale.x/3) * (health.currentHP/health.maxHP),
			                                                       0.5f,
			                                                       1);

			remainingHealthBar.transform.position = new Vector3(maxHealthBar.renderer.bounds.min.x
			                                                    	- ((maxHealthBar.renderer.bounds.min - maxHealthBar.transform.position)
			   															* (health.currentHP/health.maxHP)).x,
			                                                    maxHealthBar.transform.position.y,
			                                                    0.5f);

			if(health.currentHP <= 0.0f){
				Destroy(maxHealthBar);
				Destroy(remainingHealthBar);
			}
		}
	}
}
