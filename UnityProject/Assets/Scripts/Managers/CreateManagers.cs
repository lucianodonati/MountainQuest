using System.Collections;
using UnityEngine;

public class CreateManagers : MonoBehaviour
{
    public GameObject gameManagerPrefab;
    public GameObject soundManagerPrefab;

    // Use this for initialization
    private void Start()
    {
        if (GameManager.instance == null)
            Instantiate(gameManagerPrefab).name = gameManagerPrefab.name;
        if (SoundManager.instance == null)
            Instantiate(soundManagerPrefab).name = soundManagerPrefab.name;
    }
}