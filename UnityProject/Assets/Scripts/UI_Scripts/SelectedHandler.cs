using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
        selected.GetComponent<Image>().color = new Color(255, 255, 255, 255);        
        selected = newSelection;
        selected.GetComponent<Image>().color = new Color(0, 255, 255, 255);
    }
}
