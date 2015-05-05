using System.Collections;
using UnityEngine;

public class CreateManagers : MonoBehaviour
{
    public GameObject gameManagerPrefab;

    // Use this for initialization
    private void Start()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManagerPrefab);
            GameObject go = new GameObject();
            go.name = "SoundManager";
            go.AddComponent<SoundManager>();
        }
    }
}