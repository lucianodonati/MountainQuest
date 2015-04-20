using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float musicVol = 10.0f, sfxVol = 100.0f;
    public AudioSource music, slowmoSfx, speedupSfx;
    public GameObject TitleScreen, pauseMenuPrefab;
    private Canvas pauseMenu;
    private int currentLevel = 0;
    private float timer;
    private float duration = 0.3f, startTime;
    private bool poop = false, pause = false;

    private void Start()
    {
        if (TitleScreen != null)
            TitleScreen.SetActive(true);
        GameObject.DontDestroyOnLoad(gameObject);
        UpdateMusic(musicVol);
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.parent = Camera.main.transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startTime = Time.time;
            pause = !pause;
            if (pause)
            {
                if (pauseMenu == null)
                    pauseMenu = ((GameObject)Instantiate(pauseMenuPrefab)).GetComponent<Canvas>();
                pauseMenu.gameObject.SetActive(true);
                slowmoSfx.Play();
            }
            else
            {
                pauseMenu.gameObject.SetActive(false);
                speedupSfx.Play();
            }

            poop = true;
        }

        if (poop)
        {
            float t = (Time.time - startTime) / duration;
            if (pause)
            {
                Time.timeScale = Mathf.SmoothStep(0.999f, 0.0f, t);
                music.pitch = 0.7f;

                //clamp
                if (Time.timeScale <= 0.09f)
                {
                    Time.timeScale = 0;
                    music.Pause();
                }
            }
            else
            {
                Time.timeScale = Mathf.SmoothStep(0.099f, 1.0f, t);
                music.pitch = 1.0f;
                //clamp
                if (Time.timeScale > 0.999f)
                {
                    music.Play();
                    Time.timeScale = 1.0f;
                }
            }

            if (Time.timeScale == 1 || Time.timeScale == 0)
                poop = false;
        }
    }

    private void Pause()
    {
        //Time.timeScale = Time.timeScale == 1.0f ? 0.0f : 1.0f;
        if (Time.timeScale >= 0.0f)
        {
            float t = (Time.time - startTime) / duration;
            Time.timeScale = Mathf.SmoothStep(1.0f, 0.0f, t);
        }
    }

    public void LoadNextLevel()
    {
        if (currentLevel < Application.levelCount)
            currentLevel++;
        transform.parent = null;
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