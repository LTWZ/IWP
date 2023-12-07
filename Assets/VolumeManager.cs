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
        LoadVolumeLevels();
        LoadSFXValues();
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene loaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}