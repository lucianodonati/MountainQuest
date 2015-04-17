using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float musicVol = 100.0f, fxVol = 100.0f;
    public AudioSource music;
    private int currentLevel = 0;

    private void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        if (currentLevel < Application.levelCount)
            currentLevel++;
        Application.LoadLevel(currentLevel);
    }

    // Update is called once per frame
    private void UpdateMusic()
    {
        if (music != null)
            music.volume = musicVol / 100;
        else
            Debug.LogWarning("Trying to adjust music volume with no AudioSource attached.");
    }

    private void UpdateFx()
    {
        AudioListener.volume = fxVol / 100;
    }

    public void musicVolUp()
    {
        musicVol += 5.0f;
        if (musicVol > 100.0f)
            musicVol = 100.0f;
        UpdateMusic();
    }

    public void musicVolDown()
    {
        musicVol -= 5.0f;
        if (musicVol < 0)
            musicVol = 0;
        UpdateMusic();
    }

    public void fxVolUp()
    {
        fxVol += 5.0f;
        if (fxVol > 100.0f)
            fxVol = 100.0f;
    }

    public void fxVolDown()
    {
        fxVol -= 5.0f;
        if (fxVol < 0)
            fxVol = 0;
    }
}