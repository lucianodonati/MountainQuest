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
        int fileNumber = -1;
        if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("1"))
            fileNumber = 1;
        else if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("2"))
            fileNumber = 2;
        else if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("3"))
            fileNumber = 3;
        else if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("4"))
            fileNumber = 4;
        else if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("5"))
            fileNumber = 5;
        else if (GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.name.Contains("6"))
            fileNumber = 6;

        int count = transform.parent.GetComponent<InitializeSaveFiles>().fileNames.Count;
        if (count >= fileNumber &&
            transform.parent.GetComponent<InitializeSaveFiles>().DeletePreviousSaveFile(fileNumber, GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.gameObject.transform.FindChild("Text").GetComponent<Text>().text))
            File.Delete(Application.persistentDataPath + "/" + transform.parent.GetComponent<InitializeSaveFiles>().fileNames[fileNumber - 1]);

        BinaryFormatter binForm = new BinaryFormatter();
        string fileName = "/" + GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.gameObject.transform.FindChild("Text").GetComponent<Text>().text + ".dat";
        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate);
        SaveInfo info = new SaveInfo();
        info.info = "Yes";

        binForm.Serialize(file, info);
        file.Close();
    }

    public void Delete()
    {
        File.Delete(Application.persistentDataPath + "/" + GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.text + ".dat");
        GameObject.Find("Save Menu").GetComponent<SelectedHandler>().selected.text = "";
        GameObject.Find("SaveButton").GetComponent<Button>().interactable = false;
    }

    public void LoadSaveGame()
    {
        BinaryFormatter binForm = new BinaryFormatter();
        string fileName = "/" + GameObject.Find("Load Menu").GetComponent<SelectedHandler>().selected.gameObject.transform.FindChild("Text").GetComponent<Text>().text + ".dat";
        FileStream file = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate);
        SaveInfo info = (SaveInfo)binForm.Deserialize(file);
        file.Close();
        Debug.Log(info.info);
    }
}

[Serializable]
class SaveInfo
{
    public string info;
}