using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private AudioSource audioSource;

    // Dictionary to map scene names to music clips
    private Dictionary<string, AudioClip> sceneToMusic = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Add an AudioSource component to the GameObject if not already present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Load and map music clips for each scene
        LoadSceneMusic("StartScene", "Music/StartMusic");
        LoadSceneMusic("Scene2", "Music/Scene2Music");

        // Play the music for the current scene
        PlayCurrentSceneMusic();
    }

    public static MusicManager GetInstance()
    {
        return instance;
    }

    private void LoadSceneMusic(string sceneName, string musicResourcePath)
    {
        // Load the music clip from Resources/Music
        AudioClip musicClip = Resources.Load<AudioClip>(musicResourcePath);

        // Add the mapping to the dictionary
        sceneToMusic[sceneName] = musicClip;
    }

    private void PlayCurrentSceneMusic()
    {
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Check if the scene has an associated music clip
        if (sceneToMusic.TryGetValue(currentSceneName, out AudioClip musicClip))
        {
            // Play the music clip
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }

    public void PlayMusic(string sceneName)
    {
        // Check if the scene has an associated music clip
        if (sceneToMusic.TryGetValue(sceneName, out AudioClip musicClip))
        {
            // Play the music clip
            audioSource.clip = musicClip;
            audioSource.Play();
        }
    }

    private void OnEnable()
    {
        // Subscribe to the scene changed event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the scene changed event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Play the music for the newly loaded scene
        PlayCurrentSceneMusic();
    }
}