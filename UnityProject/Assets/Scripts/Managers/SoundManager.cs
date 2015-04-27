using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;

    static public bool isActive
    {
        get
        {
            return _instance != null;
        }
    }

    static public SoundManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Object.FindObjectOfType(typeof(SoundManager)) as SoundManager;

                if (_instance == null)
                {
                    GameObject go = new GameObject("SoundManager");
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<SoundManager>();
                }
            }
            return _instance;
        }
    }

    public List<AudioSource> sources = new List<AudioSource>();
    private float audioSourceTimer = 0.0f;

    public AudioSource createSource()
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.priority = 255;
        sources.Add(source);
        return source;
    }

    // Update is called once per frame
    private void Update()
    {
        // Sounds
        audioSourceTimer -= Time.deltaTime;
        if (audioSourceTimer <= 0.0f)
        {
            audioSourceTimer = 1.0f;
            foreach (AudioSource source in sources)
            {
                if (!(source.isPlaying))
                {
                    AudioSource reference = source;
                    //sources.Remove(source);
                    Destroy(reference.gameObject);
                }
            }
        }
    }
}