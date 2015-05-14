using UnityEngine;
using System.Collections;

public class LightningEffectObj : MonoBehaviour {
    Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.35f)
            Destroy(gameObject);
        //Debug.Log(animator.);
        //if (!animator.renderer.animation.IsPlaying("Lightning"))
	}
}
