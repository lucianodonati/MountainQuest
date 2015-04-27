using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    public static SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public List<AudioSource> sources = new List<AudioSource>();
    private float audioSourceTimer = 0.0f;

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

    public AudioSource createSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.priority = 255;
        sources.Add(source);
        return source;
    }

    private void OnLevelWasLoaded(int level)
    {
        //sources.Clear();
        //sources = new List<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Sounds
        audioSourceTimer -= Time.deltaTime;
        if (audioSourceTimer <= 0.0f)
        {
            audioSourceTimer = 1.0f;
            if (sources != null)
            {
                for (int i = 0; i < sources.Count; i++)
                {
                    if (!(sources[i].isPlaying))
                    {
                        AudioSource reference = sources[i];
                        sources.Remove(sources[i]);
                        Destroy(reference);
                        i--;
                    }
                }
            }
        }
    }
}