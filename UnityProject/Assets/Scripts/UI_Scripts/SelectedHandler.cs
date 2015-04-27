using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class SelectedHandler : MonoBehaviour {
    public List<InputField> slots;
    public InputField selected;
    
	// Use this for initialization
	void Start () {
        selected.GetComponent<Image>().color = new Color(0, 255, 255, 255);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeSelected(InputField newSelection)
    {
        if (gameObject.name == "Save Menu")
        {
            if (File.Exists(Application.persistentDataPath + "/" + newSelection.text + ".dat"))
                gameObject.transform.FindChild("DeleteButton").GetComponent<Button>().interactable = true;
            else
                gameObject.transform.FindChild("DeleteButton").GetComponent<Button>().interactable = false;
            gameObject.transform.FindChild("SaveButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            if (File.Exists(Application.persistentDataPath + "/" + newSelection.text + ".dat"))
                gameObject.transform.FindChild("LoadButton").GetComponent<Button>().interactable = true;
        }
        selected.GetComponent<Image>().color = new Color(255, 255, 255, 255);        
        selected = newSelection;
        selected.GetComponent<Image>().color = new Color(0, 255, 255, 255);
    }
}