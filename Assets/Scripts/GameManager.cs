using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    public List<string> availableLevels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAvailableLevels();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAvailableLevels()
    {
        availableLevels = new List<string> { "Level1", "Level2", "Level3", "Level4", "Level5" };
    }
}
