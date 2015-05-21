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

    [HideInInspector]
    public StatsManager statsManager;

    public GameObject skillsPrefab;

    [HideInInspector]
    public SkillsManager skillsManager;

    [HideInInspector]
    public bool loadedFromSave = false;

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
        Title, Story, Pause, Options, Credits, GameOver, Upgrade, Save, Load, Previous
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
        MainMenu, Tutorial, Level1, Level2, Level3
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
    public int musicVol = 30, sfxVol = 50;

    #endregion Sound

    //#region Save

    //private Dictionary<string, int> saveData = new Dictionary<string, int>();

    //#endregion Save

    private PlayerController playerController = null;

    public List<AudioClip> musicFiles;

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

        statsManager = ((GameObject)Instantiate(statsPrefab)).GetComponent<StatsManager>();
        statsManager.name = statsPrefab.name;

        skillsManager = ((GameObject)Instantiate(skillsPrefab)).GetComponent<SkillsManager>();
        skillsManager.name = skillsPrefab.name;

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

    public void setMusic(AudioClip _music)
    {
        music.Stop();
        music.clip = _music;
        music.Play();
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

            case Menus.Credits:
                newCanvas(_newMenu).SetActive(true);
                activeMenu = _newMenu;
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
        Dictionary<string, int> saveData = new Dictionary<string, int>();

        saveData.Add("lvl", (int)currentLevel);
        saveData.Add("musicVol", musicVol);
        saveData.Add("sfxVol", sfxVol);

        saveData.Add("date_Year", DateTime.Now.Year);
        saveData.Add("date_Month", DateTime.Now.Month);
        saveData.Add("date_Day", DateTime.Now.Day);
        saveData.Add("date_Hour", DateTime.Now.Hour);
        saveData.Add("date_Minute", DateTime.Now.Minute);

        saveData.Add("ShieldSphere", (skillsManager.GetComponent<SkillsManager>().spheres[(int)SkillsManager.SpheresId.Shield].active ? 1 : 0));
        saveData.Add("WaveSphere", (skillsManager.GetComponent<SkillsManager>().spheres[(int)SkillsManager.SpheresId.Wave].active ? 1 : 0));

        saveData.Add("FireArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Fire].active ? 1 : 0));
        saveData.Add("IceArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Ice].active ? 1 : 0));
        saveData.Add("WindArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Wind].active ? 1 : 0));
        saveData.Add("EarthArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Earth].active ? 1 : 0));
        saveData.Add("ExplodingArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Exploding].active ? 1 : 0));
        saveData.Add("ShatteringArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Shattering].active ? 1 : 0));
        saveData.Add("LightningArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Lightning].active ? 1 : 0));
        saveData.Add("EarthquakeArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.EarthQuake].active ? 1 : 0));
        saveData.Add("PlagueArrow", (skillsManager.GetComponent<SkillsManager>().arrows[(int)SkillsManager.ArrowsId.Plague].active ? 1 : 0));

        foreach (KeyValuePair<string, int> key in saveData)
            PlayerPrefs.SetInt(key.Key, key.Value);

        PlayerPrefs.SetFloat("PlayerPosX", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().respawnPos.x);
        PlayerPrefs.SetFloat("PlayerPosY", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().respawnPos.y);

        SavePlayerInfo();
    }

    public void Loadgame()
    {
        musicVol = PlayerPrefs.GetInt("musicVol");
        sfxVol = PlayerPrefs.GetInt("sfxVol");
        skillsManager.SetSphere(SkillsManager.SpheresId.Boost, true);
        skillsManager.SetSphere(SkillsManager.SpheresId.Redirect, true);
        skillsManager.SetSphere(SkillsManager.SpheresId.Shield, PlayerPrefs.GetInt("ShieldSphere") == 1);
        skillsManager.SetSphere(SkillsManager.SpheresId.Wave, PlayerPrefs.GetInt("WaveSphere") == 1);

        skillsManager.SetArrow(SkillsManager.ArrowsId.Fire, PlayerPrefs.GetInt("FireArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Ice, PlayerPrefs.GetInt("IceArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Wind, PlayerPrefs.GetInt("WindArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Earth, PlayerPrefs.GetInt("EarthArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Exploding, PlayerPrefs.GetInt("ExplodingArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Shattering, PlayerPrefs.GetInt("ShatteringArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Lightning, PlayerPrefs.GetInt("LightningArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.EarthQuake, PlayerPrefs.GetInt("EarthquakeArrow") == 1);
        skillsManager.SetArrow(SkillsManager.ArrowsId.Plague, PlayerPrefs.GetInt("PlagueArrow") == 1);
        Load((Scenes)PlayerPrefs.GetInt("lvl"));
    }

    public void SavePlayerInfo()
    {
        PlayerPrefs.SetInt("Experience", GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().experience);

        PlayerPrefs.Save();
    }

    public void LoadPlayerInfo()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().experience = PlayerPrefs.GetInt("Experience");
        GameObject.FindGameObjectWithTag("Player").transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX"), PlayerPrefs.GetFloat("PlayerPosY"), 0);
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
            if (musicFiles.Count >= (int)currentLevel)
                setMusic(musicFiles[(int)currentLevel]);

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

                if (loadedFromSave)
                    LoadPlayerInfo();
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
            if (Input.GetKeyDown(KeyCode.P))
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
        GameManager.instance.statsManager.timePlayed += Time.deltaTime;
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
        if (currentLevel != Scenes.MainMenu)
            SavePlayerInfo();
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