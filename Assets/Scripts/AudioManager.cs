using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;   

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    private static AudioManager instance;
    public int volume;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.Clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    
    public Boolean isPlayed(String name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name && sound.source.isPlaying)
            {
                return true;
            }
        }
        return false;
    }

    public void StopAll()
    {
        foreach (Sound sound in sounds)
        {
            if (sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }

    public void Stop(String name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name && sound.source.isPlaying)
            {
                sound.source.Stop();
            }
        }
    }
    
    public void Pause(String name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name && sound.source.isPlaying)
            {
                sound.source.Pause();
            }
        }
    }
    
    public void Resume(String name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.UnPause();
            }
        }
    }
    
    public void Play(String name)
    {
        foreach (Sound sound in sounds)
        {
            if (sound.name == name)
            {
                sound.source.Play();
            }
        }
    }

    public void setVolume(float volume)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = volume;
        }
    }
    
    public static AudioManager Instance
    {
        get
        {
            // If instance does not exist, create one.
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "AudioManager";
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }
}
