using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class InitializeSaveFiles : MonoBehaviour {
    public List<string> fileNames = new List<string>();

	// Use this for initialization
	void Start ()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);
        FileInfo[] info = dir.GetFiles("*.dat");

        for (int i = 0; i < info.Length && i < 6; i++)
        {
            fileNames.Add(info[i].Name);
            string name = info[i].Name.Substring(0, info[i].Name.Length - 4);
            gameObject.transform.FindChild("Save" + (i + 1).ToString() + "InputField").GetComponent<InputField>().text = name;
        }
	}

    public bool DeletePreviousSaveFile(int fileNumber, string newFileName)
    {
        if (fileNames[fileNumber].Equals(newFileName))
            return false;
        return true;
    }
}
