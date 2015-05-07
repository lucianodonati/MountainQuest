using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private bool loading = true;
    public float minLoadingTime = 0;

    private void Awake()
    {
        //Application.LoadLevel((int)GameManager.instance.currentLevel);
    }

    private void Update()
    {
        if (!this.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("LoadingAnimation"))
        {
            GameManager.instance.FinishLoading();
        }
    }
}