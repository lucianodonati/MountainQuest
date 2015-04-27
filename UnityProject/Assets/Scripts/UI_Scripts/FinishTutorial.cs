using UnityEngine;
using System.Collections;

public class FinishTutorial : MonoBehaviour {

	void OnDestroy()
    {
        GetComponent<ButtonScript>().SwitchMenu();
    }
}
