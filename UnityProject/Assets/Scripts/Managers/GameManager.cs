using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool _Debug = false;
    private string log;
    private static GameManager _instance;
    public StatsManager stats;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    //static public GameManager instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = Object.FindObjectOfType(typeof(GameManager)) as GameManager;

    //            if (_instance == null)
    //            {
    //                GameObject go = new GameObject("GameManager");
    //                DontDestroyOnLoad(go);
    //                _instance = go.AddComponent<GameManager>();
    //                if (_instance._Debug)
    //                    Debug.Log("GameManager: Instance created.");
    //            }
    //        }
    //        return _instance;
    //    }
    //}

    #region Menus

    public enum Menus
    {
        Title, Story, Pause, Options, Credits, GameOver, Save, Load, Previous
    }

    // References
    private List<GameObject> MenuInstances = new List<GameObject>();

    // Prefabs
    public List<GameObject> MenuPrefabsDONOTTOUCH = new List<GameObject>();

    // Active
    private Menus activeMenu = Menus.Title, previous = Menus.Title;

    #endregion Menus

    #region Scenes

    public enum Scenes
    {
        MainMenu, Tutorial, Level1, Level2, Level3, Level4, Level5
    };

    private Scenes currentLevel = Scenes.MainMenu;

    #endregion Scenes

    #region Pause

    private float timer;
    private float duration = 0.3f, startTime;
    private bool keyPressed = false, pause = false;

    #endregion Pause

    #region Sound

    public AudioSource music, slowmoSfx, speedupSfx, menuSelectionChange, menuSelect;
    private float musicVol = 50.0f, sfxVol = 50.0f;

    #endregion Sound

    private PlayerController playerController = null;

    private void Awake()
    {
        if (_instance == null)
        {
            //If I am the first instance, make me the Singleton
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            //If a Singleton already exists and you find
            //another reference in scene, destroy it!
            if (this != _instance)
                Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        stats = gameObject.AddComponent<StatsManager>();
        OnLevelWasLoaded(0);

        UpdateMusic(musicVol);
        AudioListener.volume = sfxVol;
    }

    private GameObject newCanvas(Menus _newCanvas)
    {
        GameObject theCanvas = MenuInstances[(int)_newCanvas];
        if (theCanvas == null)
        {
            if (_Debug)
                Debug.Log("GameManager: " + _newCanvas.ToString() + " not present, creating object.");
            theCanvas = ((GameObject)Instantiate(MenuPrefabsDONOTTOUCH[(int)_newCanvas]));
            theCanvas.name = _newCanvas.ToString() + " Menu";
            MenuInstances[(int)_newCanvas] = theCanvas;
        }
        return theCanvas;
    }

    public void switchToMenu(Menus _newMenu)
    {
        if (_Debug)
            log = "GameManager: Switching from " + activeMenu.ToString() + " to ";

        if (_newMenu == Menus.Previous)
        {
            if (_Debug)
                log += "previous (" + previous.ToString() + ").";
            disableCurrentMenu();
            activeMenu = previous;
            MenuInstances[(int)activeMenu].SetActive(true);
        }
        else
        {
            log += _newMenu.ToString() + ".";
            previous = activeMenu;
            disableCurrentMenu();
            newCanvas(_newMenu).SetActive(true);
            activeMenu = _newMenu;
        }
        if (_Debug)
            Debug.Log(log);
    }

    private void disableCurrentMenu()
    {
        if (MenuInstances[(int)activeMenu] != null)
            MenuInstances[(int)activeMenu].gameObject.SetActive(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        MenuInstances.Clear();
        for (int i = 0; i < MenuPrefabsDONOTTOUCH.Count + 1; i++)
            MenuInstances.Add(null);
        transform.parent = Camera.main.transform;

        pause = false;
        music.pitch = 1.0f;
        Time.timeScale = 1.0f;
        if (currentLevel != Scenes.MainMenu)
        {
            if (playerController == null)
                playerController = (GameObject.FindGameObjectWithTag("Player")).GetComponent<PlayerController>();
        }
        else if (currentLevel == Scenes.MainMenu)
        {
            MenuInstances[0] = newCanvas(Menus.Title);
            activeMenu = Menus.Title;
        }
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
                    playerController.enabled = false;
                    switchToMenu(Menus.Pause);
                    slowmoSfx.Play();
                }
                else
                {
                    playerController.enabled = true;
                    disableCurrentMenu();
                    speedupSfx.Play();
                    music.Play();
                }

                keyPressed = true;
            }

            if (keyPressed)
            {
                timer = (Time.time - startTime) / duration;
                if (pause)
                    Pause();
                else
                    UnPause();
                if (Time.timeScale == 1 || Time.timeScale == 0)
                    keyPressed = false;
            }
        }
        //Stats
        stats.timePlayed += Time.deltaTime;
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
        {
            music.volume = musicVol / 100;
            music.ignoreListenerVolume = true;
        }
        else
            Debug.LogWarning("Trying to adjust music volume with no AudioSource attached.");
    }

    public void UpdateSFx(float newSfxVol)
    {
        sfxVol = newSfxVol;
        AudioListener.volume = sfxVol / 100;
    }

    public void ToggleFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void Exit()
    {
        Application.Quit();
    }
}