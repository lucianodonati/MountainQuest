using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool _Debug = false;
    private string log;
    private static GameManager _instance;
    public GameObject statsPrefab;

    // Use this for initialization

    public static GameManager instance
    {
        get
        {
            if (_instance != null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

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

    public Scenes currentLevel = Scenes.MainMenu;

    #endregion Scenes

    #region Pause

    private float timer;
    private float duration = 0.3f, startTime;
    private bool keyPressed = false, pause = false;

    #endregion Pause

    #region Sound

    public AudioSource music, slowmoSfx, speedupSfx, menuSelectionChange, menuSelect;
    public int musicVol = 30, sfxVol = 100;

    #endregion Sound

    #region Save

    private Dictionary<string, int> saveData = new Dictionary<string, int>();

    #endregion Save

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
        Instantiate(statsPrefab).name = statsPrefab.name;
        gameObject.transform.parent = Camera.main.transform;
    }

    private void Start()
    {
        if (Application.loadedLevelName == "MainMenu")
            OnLevelWasLoaded(0);
        music.Play();
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

        switch (_newMenu)
        {
            case Menus.Save:
                Savegame();
                break;

            case Menus.Load:
                Loadgame();
                break;

            case Menus.Previous:
                if (_Debug)
                    log += "previous (" + previous.ToString() + ").";
                disableCurrentMenu();
                activeMenu = previous;
                MenuInstances[(int)activeMenu].SetActive(true);
                break;

            default:
                log += _newMenu.ToString() + ".";
                previous = activeMenu;
                disableCurrentMenu();
                newCanvas(_newMenu).SetActive(true);
                activeMenu = _newMenu;
                break;
        }

        if (_Debug)
            Debug.Log(log);

        if (_newMenu == Menus.Options)
        {
            Slider[] slider = MenuInstances[(int)activeMenu].GetComponentsInChildren<Slider>();
            slider[0].value = musicVol;
            slider[1].value = sfxVol;
        }
    }

    public void Savegame()
    {
        if (!saveData.ContainsKey("lvl"))
            saveData.Add("lvl", (int)currentLevel);
        if (!saveData.ContainsKey("musicVol"))
            saveData.Add("musicVol", musicVol);
        if (!saveData.ContainsKey("sfxVol"))
            saveData.Add("sfxVol", sfxVol);

        if (!saveData.ContainsKey("date_Year"))
            saveData.Add("date_Year", DateTime.Now.Year);

        if (!saveData.ContainsKey("date_Month"))
            saveData.Add("date_Month", DateTime.Now.Month);

        if (!saveData.ContainsKey("date_Day"))
            saveData.Add("date_Day", DateTime.Now.Day);

        if (!saveData.ContainsKey("date_Hour"))
            saveData.Add("date_Hour", DateTime.Now.Hour);

        if (!saveData.ContainsKey("date_Minute"))
            saveData.Add("date_Minute", DateTime.Now.Minute);

        foreach (KeyValuePair<string, int> key in saveData)
            PlayerPrefs.SetInt(key.Key, key.Value);

        PlayerPrefs.Save();
    }

    public void Loadgame()
    {
        musicVol = PlayerPrefs.GetInt("musicVol");
        sfxVol = PlayerPrefs.GetInt("sfxVol");
        Load((Scenes)PlayerPrefs.GetInt("lvl"));
    }

    private void disableCurrentMenu()
    {
        if (MenuInstances[(int)activeMenu] != null)
            MenuInstances[(int)activeMenu].gameObject.SetActive(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName != "LoadingScreen")
        {
            MenuInstances.Clear();
            for (int i = 0; i < MenuPrefabsDONOTTOUCH.Count + 1; i++)
                MenuInstances.Add(null);
            transform.parent = Camera.main.transform;
            UpdateMusic(musicVol);
            UpdateSFx(sfxVol);
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
                if (PlayerPrefs.HasKey("lvl"))
                {
                    int hour = PlayerPrefs.GetInt("date_Hour");
                    string ampm = " AM";
                    if (hour > 12)
                    {
                        hour -= 12;
                        ampm = " PM";
                    }
                    string date = PlayerPrefs.GetInt("date_Month") + "/" + PlayerPrefs.GetInt("date_Day") + "/" + PlayerPrefs.GetInt("date_Year") + "\t" + hour + ":" + PlayerPrefs.GetInt("date_Minute") + ampm;
                    Button loadButton = GameObject.Find("LoadButton").GetComponent<Button>();
                    loadButton.interactable = true;
                    loadButton.GetComponentsInChildren<Text>()[1].text = date;
                }
                activeMenu = Menus.Title;
            }
        }
    }

    private void Update()
    {
        if (currentLevel != Scenes.MainMenu && Application.loadedLevelName != "LoadingScreen")
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
        StatsManager.instance.timePlayed += Time.deltaTime;
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
        Application.LoadLevel("LoadingScreen");
    }

    public void FinishLoading()
    {
        Application.LoadLevel((int)currentLevel);
    }

    public void LoadNextLevel()
    {
        Load(++currentLevel);
    }

    public void UpdateMusic(int newMusicVol)
    {
        musicVol = newMusicVol;
        if (music != null)
        {
            music.volume = (float)musicVol / 100;
            music.ignoreListenerVolume = true;
        }
        else
            Debug.LogWarning("Trying to adjust music volume with no AudioSource attached.");
    }

    public void UpdateSFx(int newSfxVol)
    {
        sfxVol = newSfxVol;
        AudioListener.volume = (float)sfxVol / 100;
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