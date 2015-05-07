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
            if (_instance != null)
            {
                DontDestroyOnLoad(_instance.gameObject);
            }
            else
            {
                _instance = GameObject.FindObjectOfType<SoundManager>();
            }

            return _instance;
        }
    }

    public List<AudioSource> sources = new List<AudioSource>();
    private float audioSourceTimer = 0.0f;

    private void Awake()
    {
        gameObject.transform.parent = Camera.main.transform;
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
        gameObject.transform.parent = Camera.main.transform;
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