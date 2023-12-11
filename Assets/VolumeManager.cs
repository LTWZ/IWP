using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeManager : MonoBehaviour
{
    public Slider VolSlider, SFXSlider;
    private bool musicVolumeChanged = false;
    private bool sfxVolumeChanged = false;

    private void Awake()
    {
        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Subscribe to the onValueChanged events of the sliders
        VolSlider.onValueChanged.AddListener(delegate { MusicVolume(); });
        SFXSlider.onValueChanged.AddListener(delegate { SFXVolume(); });
    }

    private void Start()
    {
        // Check if this is the first launch of the game
        if (!PlayerPrefs.HasKey("VolumeSet"))
        {
            // Set the volume sliders to their maximum values
            VolSlider.value = VolSlider.maxValue;
            SFXSlider.value = SFXSlider.maxValue;

            // Save a key to indicate that the volume has been set
            PlayerPrefs.SetInt("VolumeSet", 1);

            // Save the maximum values to PlayerPrefs
            PlayerPrefs.SetFloat("VolumeValue", VolSlider.maxValue);
            PlayerPrefs.SetFloat("SFXValue", SFXSlider.maxValue);

            // Apply the volume settings
            AudioManager.instance.MusicVolume(VolSlider.maxValue);
            AudioManager.instance.SFXVolume(SFXSlider.maxValue);

            // Mark that the volume has been changed
            musicVolumeChanged = true;
            sfxVolumeChanged = true;
        }
        else
        {
            // Load saved volume levels
            LoadVolumeLevels();
            LoadSFXValues();
        }
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }

    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }

    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(VolSlider.value);
        musicVolumeChanged = true;
        PlayerPrefs.SetFloat("VolumeValue", VolSlider.value);
    }

    public void LoadVolumeLevels()
    {
        float volvalue = PlayerPrefs.GetFloat("VolumeValue");
        VolSlider.value = volvalue;
        AudioManager.instance.MusicVolume(volvalue); // Set the music volume directly
    }

    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(SFXSlider.value);
        sfxVolumeChanged = true;
        PlayerPrefs.SetFloat("SFXValue", SFXSlider.value);
    }

    public void LoadSFXValues()
    {
        float SFXvalue = PlayerPrefs.GetFloat("SFXValue");
        SFXSlider.value = SFXvalue;
        AudioManager.instance.MusicVolume(SFXvalue); // Set the music volume directly
    }

    // Call this method whenever a scene is changed
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Load volume levels only if they have been changed
        if (!musicVolumeChanged)
        {
            LoadVolumeLevels();
        }

        if (!sfxVolumeChanged)
        {
            LoadSFXValues();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}