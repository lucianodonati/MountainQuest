using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    public GameManager.Menus MenuToSwitch;
    public GameManager.Scenes SceneToLoad;

    public void SwitchMenu()
    {
        if (MenuToSwitch == GameManager.Menus.Load)
            GameManager.instance.loadedFromSave = true;
        GameManager.instance.switchToMenu(MenuToSwitch);
    }

    public void LoadScene()
    {
        if (name.Contains("Play"))
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("Level", 1);
        }
        GameManager.instance.Load(SceneToLoad);
    }

    public void Exit()
    {
        GameManager.instance.Exit();
    }

    public void ToggleFullScreen()
    {
        GameManager.instance.ToggleFullScreen();
    }

    public void ChangeMusicVol(float _vol)
    {
        GameManager.instance.UpdateMusic((int)_vol);
    }

    public void ChangeSFxVol(float _vol)
    {
        GameManager.instance.UpdateSFx((int)_vol);
    }
}