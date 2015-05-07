using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private void Update()
    {
        if (!this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LoadingAnimation"))
            GameManager.instance.FinishLoading();
    }
}