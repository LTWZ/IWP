using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Start playing music for the initial scene
        PlayMusic(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play music when a new scene is loaded
        PlayMusic(scene.name);
    }

    void PlayMusic(string sceneName)
    {
        // Load the audio clip dynamically based on the scene name
        AudioClip clip = Resources.Load<AudioClip>("Music/" + sceneName);

        if (clip == null)
        {
            Debug.LogWarning("Music clip not found for scene: " + sceneName);
            // If the music clip is not found, stop the current music
            StopMusic();
        }
        else
        {
            if (musicSource != null)
                musicSource.clip = clip;
                musicSource.Play();
        }
    }

    // Stop the current playing music
    void StopMusic()
    {
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            PlayMusic("Level 1");
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.audioname == name);

        if (s == null)
        {
            Debug.Log("SFX not Found!");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
