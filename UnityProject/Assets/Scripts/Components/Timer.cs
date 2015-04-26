using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float waitTime;

    // Update is called once per frame
    private void Update()
    {
        waitTime -= Time.deltaTime;

        if (waitTime <= 0)
            GameManager.instance.Load(GameManager.Scenes.MainMenu);
    }
}