using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float musicVol = 100.0f, sfxVol = 100.0f;
    public AudioSource music;
    public GameObject TitleScreen;
    private int currentLevel = 0;

    private void Start()
    {
        TitleScreen.SetActive(true);
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        if (currentLevel < Application.levelCount)
            currentLevel++;
        Application.LoadLevel(currentLevel);
    }

    // Update is called once per frame
    public void UpdateMusic(float newMusicVol)
    {
        musicVol = newMusicVol;
        if (music != null)
            music.volume = musicVol / 100;
        else
            Debug.LogWarning("Trying to adjust music volume with no AudioSource attached.");
    }

    public void UpdateSFx(float newSfxVol)
    {
        sfxVol = newSfxVol;
        AudioListener.volume = sfxVol / 100;
    }

    public void Exit()
    {
        Application.Quit();
    }
}