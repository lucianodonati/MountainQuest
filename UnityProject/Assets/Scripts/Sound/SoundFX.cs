using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFX : MonoBehaviour
{
    [System.Serializable]
    public struct Sound
    {
        public string name;
        public List<AudioClip> sounds;
        public float volume;

        public AudioClip getRandom()
        {
            return sounds[Random.Range(0, (sounds.Count - 1))];
        }
    }

    public List<Sound> SFX;

    public void Play(string name)
    {
        Sound playMe = findSound(name);
        if (playMe.name != null && playMe.name.Length > 0)
        {
            AudioSource source = SoundManager.instance.createSource();
            //source.name = playMe.name + " Sound";
            source.transform.parent = SoundManager.instance.transform;
            source.clip = playMe.getRandom();
            source.volume = playMe.volume;
            source.Play();
        }
    }

    private Sound findSound(string _name)
    {
        Sound theSound = new Sound();
        foreach (Sound sound in SFX)
        {
            if (sound.name == _name)
                theSound = sound;
        }
        return theSound;
    }
}