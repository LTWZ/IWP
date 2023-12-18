using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    // Define the names or IDs of your levels
    public string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5" };

    private List<string> availableLevels = new List<string>();

    public event Action onSceneLoad;
    // Call this function to initialize the list of available levels

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static LevelManager GetInstance()
    {
        return instance;
    }

    void Start()
    {
        availableLevels.AddRange(levelNames);
    }

    // Call this function when the player completes a level
    public void LoadRandomLevel()
    {
        if (levelNames.Length > 0)
        {
            // Randomly select a level from the list
            int randomIndex = Random.Range(0, levelNames.Length);
            string randomLevelName = levelNames[randomIndex];

            // Remove the selected level from the list (optional if you don't want to repeat levels)
            List<string> levelList = new List<string>(levelNames);
            levelList.RemoveAt(randomIndex);
            levelNames = levelList.ToArray();

            // Log information
            Debug.Log("Loading level: " + randomLevelName);

            StartCoroutine(LoadScene(randomLevelName));
        }
        else
        {
            // else if(levelNames.Length < 2)
            Debug.Log("No more available levels.");
            // Handle the case when there are no more levels to load.
        }
    }

    // New function to load a specific level by name
    public IEnumerator LoadSpecificLevel(string levelName)
    {
        if (availableLevels.Contains(levelName))
        {
            Debug.Log("Loading specific level: " + levelName);



        }
        else
        {
            Debug.Log("Level not available: " + levelName);
            yield return null;
        }
    }

    // Call this function to load the boss level
    public void LoadBossLevel()
    {
        StartCoroutine(LoadScene("BossLevel"));
    }

    public void LoadWizardTutorialLevel()
    {
        StartCoroutine(LoadScene("WizardTutorial"));
    }

    public void LoadRogueTutorialLevel()
    {
        StartCoroutine(LoadScene("RogueTutorial"));
    }
    public void LoadFighterTutorialLevel()
    {
        StartCoroutine(LoadScene("FighterTutorial"));
    }

    public void LoadLeve1TempLevel()
    {
        StartCoroutine(LoadScene("Level1"));
    }

    public void LoadLevel2()
    {
        StartCoroutine(LoadScene("Level2"));
    }

    public void LoadLevel3()
    {
        StartCoroutine(LoadScene("Level3"));
    }

    public void LoadLevel4()
    {
        StartCoroutine(LoadScene("Level4"));
    }

    public void LoadLevel5()
    {
        StartCoroutine(LoadScene("Level5"));
    }


    public void LoadTutorialLevel()
    {
        StartCoroutine(LoadScene("TutorialLevel"));
    }

    IEnumerator LoadScene(string name)
    {
        PlayerManager.GetInstance().SavePlayerData();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        onSceneLoad?.Invoke();
    }
}