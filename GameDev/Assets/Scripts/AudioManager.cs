using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    
    public Sound[] sounds;

    public static AudioManager instance;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {

    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        

        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Play();
    }
    
    public void Pause (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Pause();
    }

    public void RandomPlay(string[] audioNameList)
    {
        //randomly play a sound and return that sound object
        string soundName = audioNameList[UnityEngine.Random.Range(0, audioNameList.Length)];

        Play(soundName);

    }

    public static void MuteAllSound()
    {
        AudioListener.volume = 0;
    }

    public static void UnMuteAllSound()
    {
        AudioListener.volume = 1;
    }

    public void PlayOnce(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        if (!s.source.isPlaying)
        {
            s.source.Play();
        }
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null || s.source == null)
        {
            Debug.LogWarning("Sound: " + name + " not found.");
            return;
        }

        s.source.Stop();
    }
}
