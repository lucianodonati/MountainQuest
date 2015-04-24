﻿using System.Collections;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameManager.Menus MenuToSwitch;
    public GameManager.Scenes SceneToLoad;

    public void SwitchMenu()
    {
        GameManager.instance.switchToMenu(MenuToSwitch);
    }

    public void LoadScene()
    {
        GameManager.instance.Load(SceneToLoad);
    }

    public void Exit()
    {
        GameManager.instance.Exit();
    }
}