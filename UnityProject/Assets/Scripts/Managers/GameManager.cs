using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum Scenes
    {
        MainMenu, Tutorial, Level1, Level2, Level3, Level4, Level5
    };

    private Scenes currentLevel = Scenes.MainMenu;
    private float musicVol = 20.0f, sfxVol = 100.0f, timer;
    public AudioSource music, slowmoSfx, speedupSfx;
    public GameObject TitleScreen, pauseMenuPrefab;
    private Canvas pauseMenu;
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
        if (currentLevel != Scenes.MainMenu)
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
                    music.Play();
                }

                poop = true;
            }

            if (poop)
            {
                timer = (Time.time - startTime) / duration;
                if (pause)
                    Pause();
                else
                    UnPause();
                if (Time.timeScale == 1 || Time.timeScale == 0)
                    poop = false;
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = Mathf.SmoothStep(0.999f, 0.0f, timer);
        music.pitch = 0.7f;

        //clamp
        if (Time.timeScale <= 0.09f)
        {
            Time.timeScale = 0;
            music.Pause();
        }
    }

    private void UnPause()
    {
        Time.timeScale = Mathf.SmoothStep(0.099f, 1.0f, timer);
        music.pitch = 1.0f;
        //clamp
        if (Time.timeScale > 0.999f)
            Time.timeScale = 1.0f;
    }

    public void Load(Scenes _scene)
    {
        currentLevel = _scene;
        Time.timeScale = 1.0f;
        transform.parent = null;
        Application.LoadLevel((int)currentLevel);
    }

    public void LoadNextLevel()
    {
        Load(++currentLevel);
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