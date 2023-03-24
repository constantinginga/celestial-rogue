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
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
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

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Play("MenuSong");
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
}