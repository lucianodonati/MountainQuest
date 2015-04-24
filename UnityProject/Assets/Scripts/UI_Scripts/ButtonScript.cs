using System.Collections;
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

    public void ToggleFullScreen()
    {
        GameManager.instance.ToggleFullScreen();
    }

    public void ChangeMusicVol(float _vol)
    {
        GameManager.instance.UpdateMusic(_vol);
    }

    public void ChangeSFxVol(float _vol)
    {
        GameManager.instance.UpdateSFx(_vol);
    }
}