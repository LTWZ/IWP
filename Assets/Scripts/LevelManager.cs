using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    // Define the names or IDs of your levels
    public string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5" };

    private List<string> availableLevels = new List<string>();

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
        string[] levelNames = { "Level1", "Level2", "Level3", "Level4", "Level5" };

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

            // Load the selected level
            SceneManager.LoadScene(randomLevelName);
        }
        else
        {
            Debug.Log("No more available levels.");
            // Handle the case when there are no more levels to load.
        }
    }

    // Call this function to load the boss level
    public void LoadBossLevel()
    {
        SceneManager.LoadScene("BossLevel");
    }
}