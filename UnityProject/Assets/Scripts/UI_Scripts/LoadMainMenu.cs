using System.Collections;
using UnityEngine;

public class LoadMainMenu : MonoBehaviour
{
    // Use this for initialization
    public void LoadMMenu()
    {
        GameManager manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (manager != null)
            manager.Load(GameManager.Scenes.MainMenu);
        else
            Debug.LogError("No game manager ?");
    }
}