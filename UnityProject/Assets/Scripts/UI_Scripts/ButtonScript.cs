using System.Collections;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

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

    public void Save()
    {
        BinaryFormatter binForm = new BinaryFormatter();
        string fileName = "/" + gameObject.transform.FindChild("Text").GetComponent<Text>().text + ".dat";
        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.Open);

        SaveInfo info = new SaveInfo();

        binForm.Serialize(file, info);
        file.Close();
    }

    public void Delete()
    {

    }
}

[Serializable]
class SaveInfo
{

}