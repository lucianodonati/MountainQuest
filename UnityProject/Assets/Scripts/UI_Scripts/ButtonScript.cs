using System.Collections;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameManager.Menus _menu = 0;
    public GameManager.Scenes _scene = 0;

    public void SwitchMenu()
    {
        GameManager.instance.switchToMenu(_menu);
    }

    public void LoadScene()
    {
        GameManager.instance.Load(_scene);
    }
}