using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerManager.GetInstance().CreatePlayer();
    }
}
